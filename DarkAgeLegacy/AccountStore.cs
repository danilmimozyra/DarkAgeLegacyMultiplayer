using System.Security.Cryptography;
using System.Text.Json;

public class AccountStore
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;

    private readonly string filePath;
    private readonly object lockObj = new object();
    private List<AccountRecord> accounts;

    public AccountStore(string filePath)
    {
        this.filePath = filePath;
        accounts = LoadAccounts();
    }

    public bool Register(string username, string password)
    {
        lock (lockObj)
        {
            if (accounts.Any(account => account.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = HashPassword(password, salt);

            accounts.Add(new AccountRecord
            {
                Username = username,
                Salt = Convert.ToBase64String(salt),
                PasswordHash = Convert.ToBase64String(hash)
            });

            SaveAccounts();
            return true;
        }
    }

    public bool ValidateLogin(string username, string password)
    {
        lock (lockObj)
        {
            AccountRecord? account = accounts.FirstOrDefault(account =>
                account.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (account == null)
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(account.Salt);
            byte[] expectedHash = Convert.FromBase64String(account.PasswordHash);
            byte[] actualHash = HashPassword(password, salt);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }

    private List<AccountRecord> LoadAccounts()
    {
        if (!File.Exists(filePath))
        {
            return new List<AccountRecord>();
        }

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<AccountRecord>>(json) ?? new List<AccountRecord>();
    }

    private void SaveAccounts()
    {
        string? directory = Path.GetDirectoryName(filePath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string json = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    private static byte[] HashPassword(string password, byte[] salt)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);
    }

    private class AccountRecord
    {
        public string Username { get; set; } = "";
        public string Salt { get; set; } = "";
        public string PasswordHash { get; set; } = "";
    }
}
