using System.Net.Sockets;
using System.Text;


//tohle je nas klient, spousti se na zacatku server, takze DarkAgeLegacyServer a pak -||-Client
public class ConsoleClient
{
    private const string EndMessage = "<END_MESSAGE>";

    public void Start()
    {
        TcpClient client = new TcpClient("localhost", 5000);

        var reader = new StreamReader(client.GetStream(), Encoding.UTF8);
        var writer = new StreamWriter(client.GetStream(), Encoding.UTF8)
        {
            AutoFlush = true
        };

        PrintServerMessage(ReadServerMessage(reader));

        //vlakno pro ziskavani zpetne vazby ze serveru
        new Thread(() =>
        {
            while (true)
            {
                string? msg = ReadServerMessage(reader);
                if (msg == null) break;
                PrintServerMessage(msg);
            }
        })
        { IsBackground = true }.Start();

        //psani do konzole
        while (true)
        {
            string? input = Console.ReadLine();

            if (input == null)
            {
                break;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            writer.WriteLine(input);
            PrintSeparator();

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
        }
    }

    private void PrintServerMessage(string? message)
    {
        if (message == null)
        {
            return;
        }

        Console.WriteLine(message);
        PrintSeparator();
    }

    private string? ReadServerMessage(StreamReader reader)
    {
        StringBuilder message = new StringBuilder();

        while (true)
        {
            string? line = reader.ReadLine();

            if (line == null)
            {
                return message.Length == 0 ? null : message.ToString();
            }

            if (line == EndMessage)
            {
                return message.ToString().TrimEnd();
            }

            message.AppendLine(line);
        }
    }

    private void PrintSeparator()
    {
        int width = Console.WindowWidth;

        if (width <= 0)
        {
            width = 20;
        }

        Console.WriteLine(new string('=', width));
    }
}
