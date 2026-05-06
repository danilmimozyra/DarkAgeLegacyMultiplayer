using System.Net;
using System.Net.Sockets;
using DarkAgeLegacyServer;

public class Server
{
    private TcpListener listener;
    private bool running = false;
    private Logger logger;

    //sem bych pridal dictionary vsech prikazu
    private Dictionary<string, Command> commands;

    private Map map;
    private List<Player> players;

    public Server(int port)
    {
        //listener = new TcpListener(IPAddress.Any, port);
        //logger = Logger.Instance;
    }

    public void Start()
    {
        InitializeGame();
        ClientLoop(new object());

        /*listener.Start();
        running = true;

        Console.WriteLine("Server started...");

        while (running)
        {
            TcpClient tcpClient = listener.AcceptTcpClient();

            ClientConnection client = new ClientConnection(tcpClient);

            logger.Info($"Client connected: {client.Id}");

            Thread thread = new Thread(ClientLoop);
            thread.Start(client);
        }*/
    }
    
    //klasicky klient loop pro kazdeho klenta
    private void ClientLoop(object obj)
    {
        //ClientConnection client = (ClientConnection)obj;

        //client.Send("Welcome to server");

        // tady se uzivatel nejak prihlasi

        // username ziska z klienta, player se loadne ze savu
        Player player = new Player("pepa");

        //v podstate hlavni cast programu
        try
        {
            while (true)
            {
                //string message = client.Receive();
                string message = Console.ReadLine();

                if (message == "exit")
                    break;

                string response = HandleMessage(message, player);

                //client.Send(response);
                Console.WriteLine(response);
            }
        }
        catch
        {
            //logger.Error($"Client error: {client.Id}");
        }

        //client.Close();
        //logger.Info($"Client disconnected: {client.Id}");
    }

    //sem pak prijdou komandy
    private string HandleMessage(string message, Player player)
    {
        //logger.Client($"[{client.Id}] {message}");

        string[] splitMessage = message.Split(' ');
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

    private void InitializeGame()
    {
        map = new Map();
        players = new List<Player>();

        commands = new Dictionary<string, Command>();
        commands.Add("go", new Go(map));
        commands.Add("help", new Help(map));
        commands.Add("inv", new InventoryComm(map));
        commands.Add("take", new Take(map));
        commands.Add("put", new Put(map));
        commands.Add("tip",  new Tip(map));
        commands.Add("attack", new Attack(map));
        commands.Add("block", new Block(map));
        commands.Add("give",  new Give(map));
        commands.Add("puzzle", new Puzzle(map));
        commands.Add("craft", new Craft(map));
    }
}