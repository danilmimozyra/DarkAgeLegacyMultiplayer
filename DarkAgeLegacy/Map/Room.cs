using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkAgeLegacyServer
{
    public class Room
    {
        private string name;
        private int westRoom;
        private int northRoom;
        private int eastRoom;
        private int southRoom;
        private Enemy attackedEnemy;
        private List<NPC> npcs;
        private List<Item> items;

        public Room(string name, int westRoom, int northRoom, int eastRoom, int southRoom)
        {
            this.name = name;
            this.westRoom = westRoom;
            this.northRoom = northRoom;
            this.eastRoom = eastRoom;
            this.southRoom = southRoom;
            npcs = new List<NPC>();
            items = new List<Item>();
        }

        public int WestRoom { get => westRoom; }
        public int NorthRoom { get => northRoom; }
        public int EastRoom { get => eastRoom; }
        public int SouthRoom { get => southRoom; }
        public string Name { get => name; }

        public string RoomDescription()
        {
            // placeholder return
            return "You entered " + name;
        }

        public void AddNPC(NPC n)
        {
            npcs.Add(n);
        }
    }
}
