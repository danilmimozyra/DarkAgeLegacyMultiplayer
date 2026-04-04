using System;

namespace DarkAgeLegacyServer
{
    internal abstract class Command
    {
        protected Map map;

        public Command(Map map)
        {
            this.map = map;
        }

        public abstract String execute(Player player, string value);
        public abstract bool exit();
    }
}
