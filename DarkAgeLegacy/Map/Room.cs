using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkAgeLegacyServer
{
    public class Room
    {
        private String name;
        private int westRoom;
        private int northRoom;
        private int eastRoom;
        private int southRoom;

        public Room(String name, int westRoom, int northRoom, int eastRoom, int southRoom)
        {
            this.name = name;
            this.westRoom = westRoom;
            this.northRoom = northRoom;
            this.eastRoom = eastRoom;
            this.southRoom = southRoom;
        }

        public int WestRoom { get => westRoom; }
        public int NorthRoom { get => northRoom; }
        public int EastRoom { get => eastRoom; }
        public int SouthRoom { get => southRoom; }
        public string Name { get => name; }

        public String RoomDescription()
        {
            // placeholder return
            return "You entered " + name;
        }
    }
}
