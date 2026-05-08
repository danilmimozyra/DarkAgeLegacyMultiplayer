namespace DarkAgeLegacyServer
{
    internal class Go : Command
    {
        private bool dead;
        
        public Go(Map map) : base(map)
        {
            dead = false;
        }

        public override string Execute(Player player, string value)
        {
            if (value == "")
            {
                return "You don't know in which direction to go.";
            } 
            int id;
            string? line =  AttackPlayer(player, map.MapProp[player.CurrentRoom].AttackedEnemy);
            if (player.Health <= 0) {
                dead = true;
                line += "\nYou died.";
                return line;
            }
            switch (value)
            {
                case "west":
                    id = map.MapProp[player.CurrentRoom].WestRoom;
                    break;
                case "north":
                    id = map.MapProp[player.CurrentRoom].NorthRoom;
                    break;
                case "east":
                    id = map.MapProp[player.CurrentRoom].EastRoom;
                    break;
                case "south":
                    id = map.MapProp[player.CurrentRoom].SouthRoom;
                    break;
                default:
                    return "You seem confused.";
            }
            if (id != 0) {
                player.CurrentRoom = id;
                return line + map.MapProp[player.CurrentRoom].RoomDescription();
            }
            return "This room doesn't have an entrance there!";
            
        }

        public override bool Exit()
        {
            return dead;
        }
    }
}
