using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DarkAgeLegacyServer
{
    public class Item
    {
        private string name;
        private Ability ability;
        private bool isStackable;
        private int amount;

        public string Name { get => name; }
        public Ability Ability { get => ability; }
        public int Amount { get => amount; }
        public bool IsStackable { get => isStackable; }

        public Item(string name, int isStackable, string a)
        {
            this.name = name;
            if (isStackable == 1)
            {
                this.isStackable = true;
            }
            else
            {
                this.isStackable = false;
            }
            amount = 1;
            SetAbility(a);
        }

        public void ChangeAmount(int amount)
        {
            this.amount += amount;
        }

        /**
         * @return String with the information about the inventory
         */
        public virtual string Description()
        {
            if (isStackable && amount != 1)
            {
                return name + "(" + amount + ")" + AbilityDescription();
            }
            return name + AbilityDescription();
        }

        private void SetAbility(string a)
        {
            switch (a)
            {
                case "h":
                    ability = Ability.h;
                    break;
                case "k":
                    ability = Ability.k;
                    break;
                case "a":
                    ability = Ability.a;
                    break;
                case "s":
                    ability = Ability.s;
                    break;
                default:
                    ability = Ability.n;
                    break;
            }
           
        }

        private string AbilityDescription()
        {
            switch (ability)
            {
                case Ability.h:
                    return ", it can heal your health by 50";
                case Ability.k:
                    return ", it can open the Throne room in the Catacombs";
                case Ability.a:
                    return ", it reduces the damage taken from Anthrax";
                case Ability.s:
                    return ", it can teleport you out of danger";
                default:
                    return name switch
                    {
                        "Grindstone" => ", it increases Broadsword's damage by 5",
                        "Quiver" => ", it increases Crossbow's damage by 5",
                        "Talisman" => ", it increases Fire-Staff's damage by 5",
                        _ => ""
                    };

                    
            }
        }
    }
}
