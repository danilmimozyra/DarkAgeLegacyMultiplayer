using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkAgeLegacyServer
{
    internal class Go : Command
    {
        public Go(Map map) : base(map)
        {

        }

        public override string execute(Player player, string value)
        {
            int id;
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
            if (id == 0) return "This room doesn't have an entrance there!";
            
            player.CurrentRoom = id;
            return map.MapProp[id].RoomDescription();
            
            
        }

        public override bool exit()
        {
            return false;
        }
    }
}
