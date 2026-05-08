using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkAgeLegacyServer
{
    public class Weapon : Item
    {
        private int damage;

        public Weapon(String name, int damage) : base(name, 0, "")
        {
            this.damage = damage;
        }

        public int Damage { get => damage; }

        /**
         * @return String with the information about the inventory
         */
        public override string Description()
        {
            return "Your weapon is " + Name + ". Your weapon's damage is " + Damage + ".";
        }
    }
}
