using System.Net;
using System.Net.Sockets;

public class Server
{
    private TcpListener listener;
    private bool running = false;
    private Logger logger;

    //sem bych pridal dictionary vsech prikazu

    public Server(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
        logger = Logger.Instance;
    }

    public void Start()
    {
        listener.Start();
        running = true;

        Console.WriteLine("Server started...");

        while (running)
        {
            TcpClient tcpClient = listener.AcceptTcpClient();

            ClientConnection client = new ClientConnection(tcpClient);

            logger.Info($"Client connected: {client.Id}");

            Thread thread = new Thread(ClientLoop);
            thread.Start(client);
        }
    }
    
    //klasicky klient loop pro kazdeho klenta
    private void ClientLoop(object obj)
    {
        ClientConnection client = (ClientConnection)obj;

        client.Send("Welcome to server");

        //v podstate hlavni cast programu
        try
        {
            while (true)
            {
                string message = client.Receive();

                if (message == "exit")
                    break;

                string response = HandleMessage(client, message);

                client.Send(response);
            }
        }
        catch
        {
            logger.Error($"Client error: {client.Id}");
        }

        client.Close();
        logger.Info($"Client disconnected: {client.Id}");
    }

    //sem pak prijdou komandy
    private string HandleMessage(ClientConnection client, string message)
    {
        logger.Client($"[{client.Id}] {message}");

        return $"Echo: {message}";
    }
}