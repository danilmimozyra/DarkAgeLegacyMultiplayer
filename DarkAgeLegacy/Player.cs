using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DarkAgeLegacyServer
{
    public class Player
    {
        private string username;
        private int maxHealth;
        private int health;
        private int defence;
        private int currentRoom;
        private Inventory inventory;
        private Weapon weapon;
        private OffHand offHand;
        public int CurrentRoom { get { return currentRoom; } set { currentRoom = value; } }

        public int Health { get => health; set => health = value; }
        public int Defence { get => defence; set => defence = value; }
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }
        public string Username { get => username; set => username = value; }
        public Weapon Weapon { get => weapon; set => weapon = value; }
        public OffHand OffHand { get => offHand; set => offHand = value; }

        public Player(string username)
        {
            this.username = username;
            currentRoom = 1;
            maxHealth = 100;
            health = 100;
            defence = 0;
            inventory = new Inventory();
        }

        public void SufferDamage(int damage)
        {
            int i = damage - Defence;
            if (i < 0)
            {
                i = 0;
            }
            health -= i;
        }

        public bool HasItem(string name)
        {
            if (inventory.Items != null)
            {
                for (int i = 0; i < inventory.Items.Length; i++)
                {
                    if (inventory.Items[i] != null)
                    {
                        if (inventory.Items[i].Name.Equals(name))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
