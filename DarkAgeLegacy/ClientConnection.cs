using System.Net.Sockets;
using System.Text;

public class ClientConnection
{
    private const string EndMessage = "<END_MESSAGE>";

    private TcpClient client;
    private StreamReader reader;
    private StreamWriter writer;
    private object writerLock = new object();

    public string Id { get; }

    public ClientConnection(TcpClient client)
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
        lock (writerLock)
        {
            writer.WriteLine(message.TrimEnd());
            writer.WriteLine(EndMessage);
        }
    }

    public void Close()
    {
        client.Close();
    }
}
