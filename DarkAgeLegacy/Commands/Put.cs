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
            map.MapProp[player.CurrentRoom].AddItem(item);
            if (item != null) {
                return "You have left " + item.Name + "." +
                       AttackPlayer(player, map.MapProp[player.CurrentRoom].AttackedEnemy);
            }
            return "You don't have this item.";
        }

        public override bool Exit()
        {
            return false;
        }
    }
}
