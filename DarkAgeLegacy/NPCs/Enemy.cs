using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DarkAgeLegacyServer
{
    public class Enemy : NPC
    {
        private int damage;
        protected int defence;
        protected int attack;
        private string[] attackCycle;
        protected int additionalDefence;

        protected int Damage { get => damage; }
        public string[] AttackCycle { get => attackCycle; set => attackCycle = value; }

        public Enemy(string name, int health, int damage, int defence) : base(name, health)
        {
            this.damage = damage;
            this.defence = defence;
            attack = 0;
        }

        public override void SufferDamage(int damage)
        {
            int i = damage - GetDefence();
            if (i < 0)
            {
                i = 0;
            }
            Health -= i;
        }

        public int GetDefence()
        {
            return defence + additionalDefence;
        }

        public virtual string Attack(Player player)
        {
            string line = "";
            if (additionalDefence == 8)
            {
                additionalDefence = 0;
            }
            switch (attackCycle[attack])
            {
                case "a":
                    player.SufferDamage(damage);
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
                    additionalDefence = 8;
                    line = Name + " is blocking your next shot. ";
                    break;
                default:
                    line += Name + " is waiting for your move. ";
                    break;
            }
            if (attack == attackCycle.Length - 1)
            {
                attack = 0;
            }
            else
            {
                attack += 1;
            }
            switch (attackCycle[attack])
            {
                case "a":
                    line += Name + " is preparing for an attack. ";
                    break;
                case "b":
                    line += Name + " is preparing to block your next shot. ";
                    break;
                default:
                    line += Name + " is exhausted. ";
                    break;
            }
            return line;
        }
    }
}
