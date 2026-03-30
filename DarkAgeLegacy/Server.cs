using System.Net;
using System.Net.Sockets;

public class Server
{
    private TcpListener listener;
    private bool running = false;

    //sem bych pridal dictionary vsech prikazu

    public Server(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
    }

    public void Start()
    {
        listener.Start();
        running = true;

        Console.WriteLine("Server started...");

        while (running)
        {
            TcpClient tcpClient = listener.AcceptTcpClient();

            GameClient client = new GameClient(tcpClient);

            Console.WriteLine($"Client connected: {client.Id}");

            Thread thread = new Thread(ClientLoop);
            thread.Start(client);
        }
    }
    
    //klasicky klient loop pro kazdeho klenta
    private void ClientLoop(object obj)
    {
        GameClient client = (GameClient)obj;

        client.Send("Welcome to server");

        //v podstate hlavni cast programu
        try
        {
            while (true)
            {
                string message = client.Receive();

                if (message == "exit")
                    break;

                Console.WriteLine($"[{client.Id}] {message}");

                string response = HandleMessage(client, message);

                client.Send(response);
            }
        }
        catch
        {
            Console.WriteLine($"Client error: {client.Id}");
        }

        client.Close();
        Console.WriteLine($"Client disconnected: {client.Id}");
    }

    //sem pak prijdou komandy
    protected virtual string HandleMessage(GameClient client, string message)
    {
        return $"Echo: {message}";
    }
}