using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DarkAgeLegacyServer
{
    public class Inventory
    {
        private Item[] items;

        public Inventory()
        {
            items = new Item[4];
        }

        public Item[] Items { get => items; }

        /**
         * @param item is an Item that being added
         * @return true if the item was added
         */
        public bool AddItem(Item item)
        {
            if (item.IsStackable)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null)
                    {
                        if (item.Name.Equals(items[i].Name))
                        {
                            items[i].ChangeAmount(item.Amount);
                            return true;
                        }
                    }
                }
            }
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    return true;
                }
            }
            return false;
        }

        /**
         * @return String with the information about the inventory
         */
        public string Description()
        {
            string line = "You have ";
            int i = 0;
            foreach (Item item in items)
            {
                if (item != null)
                {
                    line += item.Description();
                    if (i < GetSize() - 1)
                    {
                        line += "; ";
                    }
                    i++;
                }
            }
            if (line.Equals("You have "))
            {
                line += "nothing in your inventory.";
            }
            else
            {
                line += " in your inventory.";
            }
            return line;

        }

        public int GetSize()
        {
            int i = 0;
            foreach (Item item in items)
            {
                if (item != null)
                {
                    i++;
                }
            }
            return i;
        }
    }
}
