namespace DarkAgeLegacyServer
{
    internal class Put : Command
    {
        public Put(Map map) : base(map)
        {
        }

        public override string Execute(Player player, string value)
        {
            Item? item = player.RemoveItem(value);
            map.CurrentRoom.AddItem(item);
            if (item != null) {
                return "You have left " + item.Name + "." +
                       AttackPlayer(player, map.CurrentRoom.AttackedEnemy);
            }
            return "You don't have this item.";
        }

        public override bool Exit()
        {
            return false;
        }
    }
}
