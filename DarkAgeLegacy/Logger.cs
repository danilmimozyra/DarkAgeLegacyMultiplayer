using System;
using System.IO;

public sealed class Logger
{
    private static readonly Lazy<Logger> instance = new Lazy<Logger>(() => new Logger());
    private static readonly object lockObj = new object();

    private readonly string logPath = Path.Combine(
        Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "log.txt"
    );

    private Logger()
    {
    }

    public static Logger Instance => instance.Value;

    public void Log(string message)
    {
        string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}";

        lock (lockObj)
        {
            File.AppendAllText(logPath, line + Environment.NewLine);
        }
    }

    public void Info(string message)
    {
        Log("[INFO] " + message);
    }

    public void Error(string message)
    {
        Log("[ERROR] " + message);
    }

    public void Client(string message)
    {
        Log("[CLIENT] " + message);
    }

    public void Command(string message)
    {
        Log("[COMMAND] " + message);
    }
}