namespace DarkAgeLegacyServer
{
    internal class InventoryComm : Command
    {
        public InventoryComm(Map map) : base(map)
        {
        }

        public override string Execute(Player player, string value)
        {
            return player.InventoryDescription();
        }

        public override bool Exit()
        {
            return false;
        }
    }
}
