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
                    id = map.map[player.CurrentRoom].WestRoom;
                    break;
                case "north":
                    id = map.map[player.CurrentRoom].NorthRoom;
                    break;
                case "east":
                    id = map.map[player.CurrentRoom].EastRoom;
                    break;
                case "south":
                    id = map.map[player.CurrentRoom].SouthRoom;
                    break;
                default:
                    return "You seem confused.";
            }
            if (id == 0) return "This room doesn't have an entrance there!";
            
            player.CurrentRoom = id;
            return map.map[id].RoomDescription();
            
            
        }

        public override bool exit()
        {
            return false;
        }
    }
}
