using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkAgeLegacyServer
{
    public class OffHand : Item
    {
        private int defenceBuff;
        private int healthBuff;
        private int damageBuff;

        public OffHand(String name, int defenceBuff, int healthBuff, int damageBuff) : base(name, 0, "")
        {
            this.defenceBuff = defenceBuff;
            this.healthBuff = healthBuff;
            this.damageBuff = damageBuff;
        }

        public int DefenceBuff { get => defenceBuff; }
        public int HealthBuff { get => healthBuff; }
        public int DamageBuff { get => damageBuff; }

        /**
         * @return String with the information about the inventory
         */
        public override string Description()
        {
            String line = "Your off-hand is " + Name + ". It grants you";
            if (defenceBuff != 0)
            {
                line += " +" + defenceBuff + " defence";
            }
            if (healthBuff != 0)
            {
                line += " +" + healthBuff + " health";
            }
            if (damageBuff != 0)
            {
                line += " +" + damageBuff + " damage";
            }
            return line + ".";
        }
    }
}
