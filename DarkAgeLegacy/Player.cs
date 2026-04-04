using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkAgeLegacyServer
{
    public class Player
    {
        private String username;
        private int maxHealth;
        private int health;
        private int defence;
        private int currentRoom;
        public int CurrentRoom { get { return currentRoom; } set { currentRoom = value; } }

        public Player(string username)
        {
            this.username = username;
            currentRoom = 1;
            maxHealth = 100;
            health = 100;
            defence = 0;
        }
    }
}
