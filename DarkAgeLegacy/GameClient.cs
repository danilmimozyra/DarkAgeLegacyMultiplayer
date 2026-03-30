using System.Net.Sockets;
using System.Text;

public class GameClient
{
    private TcpClient client;
    private StreamReader reader;
    private StreamWriter writer;

    public string Id { get; }

    public GameClient(TcpClient client)
    {
        this.client = client;

        reader = new StreamReader(client.GetStream(), Encoding.UTF8);
        writer = new StreamWriter(client.GetStream(), Encoding.UTF8)
        {
            AutoFlush = true
        };

        Id = Guid.NewGuid().ToString();
    }

    //posila na server
    public string Receive()
    {
        return reader.ReadLine();
    }

    //pise do konzole klienta
    public void Send(string message)
    {
        writer.WriteLine(message);
    }

    public void Close()
    {
        client.Close();
    }
}