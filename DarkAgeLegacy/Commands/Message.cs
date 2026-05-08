namespace DarkAgeLegacyServer
{
    internal class Message : Command
    {
        public Message(Map map) : base(map)
        {
        }

        public override string Execute(Player player, string value)
        {
            return global::Server.Instance.SendPrivateMessage(player.Username, value);
        }

        public override bool Exit()
        {
            return false;
        }
    }
}
