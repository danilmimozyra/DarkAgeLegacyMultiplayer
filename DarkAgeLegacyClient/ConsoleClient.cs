using System.Net.Sockets;
using System.Text;


//tohle je nas klient, spousti se na zacatku server, takze DarkAgeLegacyServer a pak -||-Client
public class ConsoleClient
{
    public void Start()
    {
        TcpClient client = new TcpClient("localhost", 5000);

        var reader = new StreamReader(client.GetStream(), Encoding.UTF8);
        var writer = new StreamWriter(client.GetStream(), Encoding.UTF8)
        {
            AutoFlush = true
        };

        Console.WriteLine(reader.ReadLine());

        //vlakno pro ziskavani zpetne vazby ze serveru
        new Thread(() =>
        {
            while (true)
            {
                string msg = reader.ReadLine();
                if (msg == null) break;
                Console.WriteLine(msg);
            }
        })
        { IsBackground = true }.Start();

        //psani do konzole
        while (true)
        {
            string input = Console.ReadLine();
            writer.WriteLine(input);
        }
    }
}