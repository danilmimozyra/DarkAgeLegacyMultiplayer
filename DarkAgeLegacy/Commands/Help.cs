using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkAgeLegacyServer
{
    internal class Help : Command
    {
        public Help(Map map) : base(map)
        {
        }

        public override string execute(Player player, string value)
        {
            return """
                
                go [direction] – relocate to another room if possible.
                help           – displaying help for available commands.
                tip            – additional advice for the current situation.
                take [item]    – picking up an item.
                put [item]     – leave an item.
                use [item]     – activates an item.
                attack [NPC]   – dealing damage to an enemy.
                craft [item]   - creating an item.
                give           - leaving an item for an NPC.
                puzzle         - activating the puzzle if the room has one.
                inventory      - displaying what you currently have in your bag.
                block          – protecting yourself from suffering bigger damage.
                exit           – ending the game.
                
                """;
        }

        public override bool exit()
        {
            return false;
        }
    }
}
