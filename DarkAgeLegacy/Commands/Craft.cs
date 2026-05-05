using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkAgeLegacyServer
{
    internal class Craft : Command
    {
        public Craft(Map map) : base(map)
        {
        }

        public override string Execute(Player player, string value)
        {
            throw new NotImplementedException();
        }

        public override bool Exit()
        {
            return false;
        }
    }
}
