namespace DarkAgeLegacyServer
{
    internal class Players : Command
    {
        public Players(Map map) : base(map)
        {
        }

        public override string Execute(Player player, string value)
        {
            return global::Server.Instance.OnlinePlayersDescription();
        }

        public override bool Exit()
        {
            return false;
        }
    }
}
