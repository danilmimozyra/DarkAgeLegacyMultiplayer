namespace DarkAgeLegacyServer
{
    internal class Block : Command
    {
        private bool dead;
        private bool block;
        
        public Block(Map map) : base(map)
        {
        }

        public void Unblock(Player p)
        {
            if (block)
            {
                block = false;
                p.Defence = -10;
            }
        }

        public override string Execute(Player player, string value)
        {
            if (!block)
            {
                block = true;
                player.Defence = 10;
            }

            string line = "You are now blocking the next shot." + AttackPlayer(player, map.MapProp[player.CurrentRoom].AttackedEnemy);

            if (player.Health <= 0)
            {
                dead = true;
                line += "\nYou died.";
            }

            return line;
            
        }

        public override bool Exit()
        {
            return dead;
        }
    }
}
