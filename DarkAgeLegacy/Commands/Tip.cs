namespace DarkAgeLegacyServer
{
    internal class Tip : Command
    {
        public Tip(Map map) : base(map)
        {
        }

        public override string Execute(Player player, string value)
        {
            string line = map.MapProp[player.CurrentRoom].RoomInfo();
            line += "\nSome rooms may contain puzzles. To check if the room has one use the command: puzzle.";
            if (map.MapProp[GameSettings.Instance.LockedThroneHintRoomId].SouthRoom == 0) {
                line += "\n" + GameSettings.Instance.LockedThroneHint;
            }
            return line;
        }

        public override bool Exit()
        {
            return false;
        }
    }
}
