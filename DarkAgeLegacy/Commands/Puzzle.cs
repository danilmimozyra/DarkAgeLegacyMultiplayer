namespace DarkAgeLegacyServer
{
    internal class Puzzle : Command
    {
        private readonly Dictionary<string, PuzzleData> puzzles;
        private readonly HashSet<string> solvedRooms;

        public Puzzle(Map map) : base(map)
        {
            puzzles = LoadPuzzles();
            solvedRooms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public override string Execute(Player player, string value)
        {
            string roomName = map.MapProp[player.CurrentRoom].RoomName();

            if (!puzzles.TryGetValue(roomName, out PuzzleData? puzzle))
            {
                return "Seems like there isn't any puzzles in this room.";
            }

            if (!string.IsNullOrWhiteSpace(puzzle.RequiredNpc)
                && map.MapProp[player.CurrentRoom].FindNPC(puzzle.RequiredNpc) != null)
            {
                return puzzle.SuccessMessage;
            }

            if (solvedRooms.Contains(roomName))
            {
                return "This puzzle has already been solved.";
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return puzzle.Riddle + "\nUse: puzzle [answer]";
            }

            if (!value.Equals(puzzle.Answer, StringComparison.OrdinalIgnoreCase))
            {
                return "- No";
            }

            ApplyReward(player, puzzle);
            solvedRooms.Add(roomName);
            return puzzle.SuccessMessage;
        }

        public override bool Exit()
        {
            return false;
        }

        private Dictionary<string, PuzzleData> LoadPuzzles()
        {
            Dictionary<string, PuzzleData> loadedPuzzles = new Dictionary<string, PuzzleData>(StringComparer.OrdinalIgnoreCase);
            string filePath = "res/puzzles.txt";

            if (!File.Exists(filePath))
            {
                return loadedPuzzles;
            }

            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] puzzleInfo = line.Split('|');

                if (puzzleInfo.Length >= 8)
                {
                    PuzzleData puzzle = new PuzzleData(puzzleInfo);
                    loadedPuzzles[puzzle.RoomName] = puzzle;
                }
            }

            return loadedPuzzles;
        }

        private void ApplyReward(Player player, PuzzleData puzzle)
        {
            if (!string.IsNullOrWhiteSpace(puzzle.OpenDirection) && puzzle.OpenRoomId != 0)
            {
                switch (puzzle.OpenDirection.ToLower())
                {
                    case "west":
                        map.MapProp[player.CurrentRoom].WestRoom = puzzle.OpenRoomId;
                        break;
                    case "north":
                        map.MapProp[player.CurrentRoom].NorthRoom = puzzle.OpenRoomId;
                        break;
                    case "east":
                        map.MapProp[player.CurrentRoom].EastRoom = puzzle.OpenRoomId;
                        break;
                    case "south":
                        map.MapProp[player.CurrentRoom].SouthRoom = puzzle.OpenRoomId;
                        break;
                }
            }

            if (puzzle.RewardInfo.Length > 0)
            {
                map.MapProp[player.CurrentRoom].AddItem(map.CreateItem(puzzle.RewardInfo, 0));
            }
        }

        private class PuzzleData
        {
            public string RoomName { get; }
            public string RequiredNpc { get; }
            public string Answer { get; }
            public string OpenDirection { get; }
            public int OpenRoomId { get; }
            public string[] RewardInfo { get; }
            public string SuccessMessage { get; }
            public string Riddle { get; }

            public PuzzleData(string[] puzzleInfo)
            {
                RoomName = puzzleInfo[0];
                RequiredNpc = puzzleInfo[1];
                Answer = puzzleInfo[2];
                OpenDirection = puzzleInfo[3];
                OpenRoomId = int.TryParse(puzzleInfo[4], out int roomId) ? roomId : 0;
                RewardInfo = string.IsNullOrWhiteSpace(puzzleInfo[5])
                    ? Array.Empty<string>()
                    : puzzleInfo[5].Split(',');
                SuccessMessage = puzzleInfo[6].Replace("\\n", "\n");
                Riddle = puzzleInfo[7].Replace("\\n", "\n");
            }
        }
    }
}
