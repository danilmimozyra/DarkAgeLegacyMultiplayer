using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkAgeLegacyServer
{
    public class NPC
    {
        Random rd;
        private string name;
        private int health;
        protected List<Dictionary<int, Item>> drops;

        protected string Name { get => name; }
        protected int Health { get => health; set => { health = value; } }

        public NPC(string name, int health)
        {
            rd = new Random();
            this.name = name;
            this.health = health;
            drops = new();
        }

        public virtual void SufferDamage(int damage)
        {
            health -= damage;
        }

        public void AddDrop(int dropRate, Item item)
        {
            Dictionary<int, Item> temp = new();
            temp.Add(dropRate, item);
            drops.Add(temp);
        }

        public List<Item> Drop()
        {
            List<Item> droppedItems = new();
            foreach (Dictionary<int, Item> map in drops)
            {
                foreach (int i in map.Keys)
                {
                    int j = rd.Next(i + 1);
                    if (j == i)
                    {
                        droppedItems.Add(map[j]);
                    }
                }
            }
            return droppedItems;
        }
    }
}
