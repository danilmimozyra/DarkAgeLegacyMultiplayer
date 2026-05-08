using System.Net;
using System.Net.Sockets;
using DarkAgeLegacyServer;

public class Server
{
    public static Server Instance { get; private set; } = null!;

    private TcpListener listener;
    private bool running = false;
    private Logger logger;
    private AccountStore accountStore;

    //sem bych pridal dictionary vsech prikazu
    private Dictionary<string, Command> commands = new Dictionary<string, Command>();

    private Map map = null!;
    private List<Player> players = new List<Player>();
    private HashSet<string> activeUsers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    private Dictionary<string, ClientConnection> activeClients = new Dictionary<string, ClientConnection>(StringComparer.OrdinalIgnoreCase);
    private object activeUsersLock = new object();

    public Server(int port)
    {
        if (Instance != null)
        {
            throw new InvalidOperationException("Server has already been created.");
        }

        Instance = this;
        listener = new TcpListener(IPAddress.Any, port);
        logger = Logger.Instance;
        accountStore = new AccountStore(Path.Combine(AppContext.BaseDirectory, "users.txt"));
    }

    public void Start()
    {
        InitializeGame();

        listener.Start();
        running = true;

        Console.WriteLine("Server started...");

        while (running)
        {
            TcpClient tcpClient = listener.AcceptTcpClient();

            ClientConnection client = new ClientConnection(tcpClient);

            logger.Info($"Client connected: {client.Id}");

            Thread thread = new Thread(ClientLoop);
            thread.IsBackground = true;
            thread.Start(client);
        }
    }
    
    //klasicky klient loop pro kazdeho klenta
    private void ClientLoop(object obj)
    {
        ClientConnection client = (ClientConnection)obj;
        string username = "";
        Player? player = null;

        client.Send("Welcome to server");

        //v podstate hlavni cast programu
        try
        {
            username = AuthenticateClient(client);

            if (username == "")
            {
                return;
            }

            player = new Player(username);
            players.Add(player);
            client.Send("You are logged in as " + username + ".");

            while (true)
            {
                string message = client.Receive();

                if (message == null || message == "exit")
                    break;

                string response = HandleMessage(message, player);

                client.Send(response);
            }
        }
        catch
        {
            logger.Error($"Client error: {client.Id}");
        }
        finally
        {
            if (player != null)
            {
                players.Remove(player);
            }

            if (username != "")
            {
                Logout(username);
            }

            client.Close();
            logger.Info($"Client disconnected: {client.Id}");
        }
    }

    //sem pak prijdou komandy
    private string HandleMessage(string message, Player player)
    {
        //logger.Client($"[{client.Id}] {message}");

        if (string.IsNullOrWhiteSpace(message))
        {
            return "Not a valid command";
        }

        string[] splitMessage = message.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string command = splitMessage[0];

        if (!commands.ContainsKey(command)) return "Not a valid command";

        Block b = (Block)commands["block"];
        b.Unblock(player);
        if (splitMessage.Length > 1)
        {
            return commands[command].Execute(player, splitMessage[1]);
        }
        return commands[command].Execute(player, "");
    }

    public string OnlinePlayersDescription()
    {
        lock (activeUsersLock)
        {
            if (activeUsers.Count == 0)
            {
                return "No players are online.";
            }

            return "Online players: " + string.Join(", ", activeUsers);
        }
    }

    public string SendPrivateMessage(string senderUsername, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return "Usage: message userLogin hello world";
        }

        string[] splitMessage = message.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        if (splitMessage.Length < 2 || string.IsNullOrWhiteSpace(splitMessage[0]) || string.IsNullOrWhiteSpace(splitMessage[1]))
        {
            return "Usage: message userLogin hello world";
        }

        string receiverUsername = splitMessage[0];
        string text = splitMessage[1];
        ClientConnection receiver;

        lock (activeUsersLock)
        {
            if (!activeClients.TryGetValue(receiverUsername, out receiver))
            {
                return "Player is not online.";
            }
        }

        receiver.Send(senderUsername + " >>> " + text);
        return "Message sent to " + receiverUsername + ".";
    }

    private void InitializeGame()
    {
        map = new Map();
        players = new List<Player>();
        activeUsers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        activeClients = new Dictionary<string, ClientConnection>(StringComparer.OrdinalIgnoreCase);

        commands = new Dictionary<string, Command>(StringComparer.OrdinalIgnoreCase);
        commands.Add("go", new Go(map));
        commands.Add("help", new Help(map));
        commands.Add("inv", new InventoryComm(map));
        commands.Add("inventory", new InventoryComm(map));
        commands.Add("take", new Take(map));
        commands.Add("put", new Put(map));
        commands.Add("tip",  new Tip(map));
        commands.Add("attack", new Attack(map));
        commands.Add("block", new Block(map));
        commands.Add("give",  new Give(map));
        commands.Add("puzzle", new Puzzle(map));
        commands.Add("craft", new Craft(map));
        commands.Add("use", new Use(map));
        commands.Add("players", new Players(map));
        commands.Add("message", new Message(map));
    }

    private string AuthenticateClient(ClientConnection client)
    {
        while (true)
        {
            client.Send("Choose: login, register, or exit");
            string action = client.Receive();

            if (action == null)
            {
                return "";
            }

            action = action.Trim().ToLower();

            if (action == "exit")
            {
                return "";
            }

            if (action != "login" && action != "register")
            {
                client.Send("Unknown option. Type login, register, or exit.");
                continue;
            }

            client.Send("Username:");
            string username = client.Receive();

            if (username == null)
            {
                return "";
            }

            username = username.Trim();

            if (!IsValidUsername(username))
            {
                client.Send("Username must be 3-20 characters and cannot contain spaces.");
                continue;
            }

            client.Send("Password:");
            string password = client.Receive();

            if (password == null || string.IsNullOrWhiteSpace(password))
            {
                client.Send("Password cannot be empty.");
                continue;
            }

            if (action == "register")
            {
                if (!accountStore.Register(username, password))
                {
                    client.Send("This username already exists.");
                    continue;
                }

                if (!TryLogin(username, client))
                {
                    client.Send("This account is already playing.");
                    continue;
                }

                client.Send("Registration successful.");
                return username;
            }

            if (!accountStore.ValidateLogin(username, password))
            {
                client.Send("Wrong username or password.");
                continue;
            }

            if (!TryLogin(username, client))
            {
                client.Send("This account is already playing.");
                continue;
            }

            return username;
        }
    }

    private bool TryLogin(string username, ClientConnection client)
    {
        lock (activeUsersLock)
        {
            if (activeUsers.Contains(username))
            {
                return false;
            }

            activeUsers.Add(username);
            activeClients.Add(username, client);
            return true;
        }
    }

    private void Logout(string username)
    {
        lock (activeUsersLock)
        {
            activeUsers.Remove(username);
            activeClients.Remove(username);
        }
    }

    private bool IsValidUsername(string username)
    {
        return !string.IsNullOrWhiteSpace(username)
            && username.Length >= 3
            && username.Length <= 20
            && !username.Contains(' ');
    }
}
