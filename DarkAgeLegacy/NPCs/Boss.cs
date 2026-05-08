using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DarkAgeLegacyServer
{
    public class Boss : Enemy
    {
        private int specialDamage;

        public Boss(String name, int health, int damage, int defence, int specialDamage) : base(name, health, damage, defence)
        {
            this.specialDamage = specialDamage;
        }

        public int SpecialDamage { get => specialDamage; set => specialDamage = value; }

        public override string Attack(Player player)
        {
            string line = "";
            if (additionalDefence == 10)
            {
                additionalDefence = 0;
            }
            switch (AttackCycle[attack])
            {
                case "a":
                    if (Name.Equals("Anthrax") && player.HasItem("Amulet-of-Light"))
                    {
                        player.SufferDamage(Damage - 6);
                    }
                    else
                    {
                        player.SufferDamage(Damage);
                    }
                    line = Name + " has attacked you.\n";
                    if (player.Health > 0)
                    {
                        line += "Your health is " + player.Health + "/" + player.MaxHealth + ".\n";
                    }
                    else
                    {
                        line += "You died.  L";
                    }
                    break;
                case "b":
                    additionalDefence = 10;
                    line = Name + " is blocking your next shot. ";
                    break;
                case "s":
                    if (Name.Equals("Anthrax") && player.HasItem("Amulet-of-Light"))
                    {
                        player.SufferDamage(specialDamage - 6);
                    }
                    else
                    {
                        player.SufferDamage(specialDamage);
                    }
                    line = Name + " has dealt a powerful strike.\n";
                    if (player.Health > 0)
                    {
                        line += "Your health is " + player.Health + "/" + player.MaxHealth + ".\n";
                    }
                    else
                    {
                        line += "You died.  L";
                    }
                    break;
                default:
                    line += Name + " is waiting for your move. ";
                    break;
            }
            if (attack == AttackCycle.Length - 1)
            {
                attack = 0;
            }
            else
            {
                attack += 1;
            }
            switch (AttackCycle[attack])
            {
                case "a":
                    line += Name + " is preparing for an attack.";
                    break;
                case "b":
                    line += Name + " is preparing to block your next shot.";
                    break;
                case "s":
                    line += Name + " is preparing a powerful strike.";
                    break;
                default:
                    line += Name + " is exhausted.";
                    break;
            }
            return line;
        }
    }
}
