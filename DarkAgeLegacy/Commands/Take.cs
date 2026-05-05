namespace DarkAgeLegacyServer
{
    internal class Take : Command
    {
        public Take(Map map) : base(map)
        {
        }

        public override string Execute(Player player, string value)
        {
            Item item = map.CurrentRoom.FindItem(value);
            string line = "";
            if (item != null) {
                if (item is Weapon) {
                    map.CurrentRoom.RemoveItem(item);
                    map.CurrentRoom.AddItem(player.Weapon);
                    player.Weapon = (Weapon) item;
                    line = "You've picked up weapon " + item.Name + ".";
                } else if (item is OffHand) {
                    map.CurrentRoom.RemoveItem(item);
                    map.CurrentRoom.AddItem(player.OffHand);
                    player.OffHand = (OffHand) item;
                    line = "You've picked up off-hand " + item.Name + ".";
                } else {
                    if (player.AddItem(item)) {
                        map.CurrentRoom.RemoveItem(item);
                        line = "You've picked up " + item.Name + ".";
                    } else {
                        line = "You don't have enough space to pick this up!";
                    }
                }
            } else {
                line = "There is no such item.";
            }
            return line + AttackPlayer(player, map.CurrentRoom.AttackedEnemy);
        }

        public override bool Exit()
        {
            return false;
        }
        
        
    }
}
