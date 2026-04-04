using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkAgeLegacyServer
{
    public class Map
    {
        public Dictionary<int, Room> map;
        private Random rd;

        public Map()
        {
            map = new Dictionary<int, Room>();
            rd = new Random();
            LoadMap();
        }

        private void LoadMap()
        {

            string filePath = "res/map.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    String[] roomInfo = line.Split(",");
                    Room r = new Room(roomInfo[1],
                            Int32.Parse(roomInfo[2]),
                            Int32.Parse(roomInfo[3]),
                            Int32.Parse(roomInfo[4]),
                            Int32.Parse(roomInfo[5]));

                    map.Add(Int32.Parse(roomInfo[0]), r);
                }
            }
        }
    }
}
