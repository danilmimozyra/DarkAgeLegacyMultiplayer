using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkAgeLegacyServer
{
    internal class Talk : Command
    {
        public Talk(Map map) : base(map)
        {
        }

        public override string execute(Player player, string value)
        {
            throw new NotImplementedException();
        }

        public override bool exit()
        {
            return false;
        }
    }
}
