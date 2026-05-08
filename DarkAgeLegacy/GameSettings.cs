namespace DarkAgeLegacyServer
{
    public class GameSettings
    {
        private static readonly Lazy<GameSettings> instance = new Lazy<GameSettings>(() => new GameSettings());
        private readonly Dictionary<string, string> values = new Dictionary<string, string>();

        private GameSettings()
        {
            string filePath = "res/game_settings.txt";

            if (!File.Exists(filePath))
            {
                return;
            }

            foreach (string line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    continue;
                }

                string[] parts = line.Split('=', 2);

                if (parts.Length == 2)
                {
                    values[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }

        public static GameSettings Instance => instance.Value;

        public int HealAmount => GetInt("heal_amount", 50);
        public string ThroneKeyRoomName => GetString("throne_key_room", "Catacombs");
        public int ThroneRoomId => GetInt("throne_room_id", 10);
        public int TeleportRoomId => GetInt("teleport_room_id", 1);
        public int LockedThroneHintRoomId => GetInt("locked_throne_hint_room_id", 8);
        public string LockedThroneHint => GetString("locked_throne_hint", "You have to open the Throne Room in the catacombs using the 'Throne-Room-Key'.");

        private string GetString(string key, string fallback)
        {
            return values.TryGetValue(key, out string? value) ? value : fallback;
        }

        private int GetInt(string key, int fallback)
        {
            return values.TryGetValue(key, out string? value) && int.TryParse(value, out int result)
                ? result
                : fallback;
        }
    }
}
