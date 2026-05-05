using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkAgeLegacyServer
{
    internal class Tip : Command
    {
        public Tip(Map map) : base(map)
        {
        }

        public override string Execute(Player player, string value)
        {
            string line = map.CurrentRoom.RoomInfo();
            line += "\nSome rooms may contain puzzles. To check if the room has one use the command: puzzle.";
            if (map.MapProp[8].SouthRoom == 0) {
                line += "\nYou have to open the Throne Room in the catacombs using the 'Throne-Room-Key'.";
            }
            return line;
        }

        public override bool Exit()
        {
            return false;
        }
    }
}
