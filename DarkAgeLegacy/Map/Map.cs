using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkAgeLegacyServer
{
    public class Map
    {
        private Dictionary<int, Room> map;
        private Room currentRoom;
        private Random rd;
        public Room CurrentRoom { get => currentRoom; set => currentRoom = value; }
        public Dictionary<int, Room> MapProp { get => map; set => map = value; }

        public Map()
        {
            map = new Dictionary<int, Room>();
            rd = new Random();
            LoadMap();
            LoadNPCs();
            LoadDrops();
            currentRoom = map[1];
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

        private void LoadNPCs()
        {
            string filePath = "res/npcs.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] npcInfo = line.Split(",");
                    NPC npc = CreateNPC(npcInfo);
                    if (npc is Enemy || npc is Boss) 
                    {
                        line = reader.ReadLine();
                        string[] cycle = line.Split(",");
                        ((Enemy) npc).AttackCycle = cycle;
                    }
                    map[int.Parse(npcInfo[0])].AddNPC(npc);
                }
            } 
        }

        private NPC CreateNPC(string[] npcInfo)
        {
            NPC? n = npcInfo[1] switch
            {
                "0" => new NPC(npcInfo[2], int.Parse(npcInfo[3])),

                "1" => new Enemy(npcInfo[2], int.Parse(npcInfo[3]),
                                 int.Parse(npcInfo[4]), int.Parse(npcInfo[5])),

                "2" => new Boss(npcInfo[2], int.Parse(npcInfo[3]),
                                int.Parse(npcInfo[4]), int.Parse(npcInfo[5]), int.Parse(npcInfo[6])),
                _ => null
            };

            return n;
        }

        private void LoadDrops()
        {
            string filePath = "res/drops.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] dropsInfo = line.Split(",");
                    map[int.Parse(dropsInfo[0])].FindNPC(dropsInfo[1]).AddDrop(
                            int.Parse(dropsInfo[2]),
                            AddDropToNPC(dropsInfo)
                    );
                }
            } 
            map[rd.Next(map.Count) + 1].AddItem(
                    new OffHand("Torch", 0, 20, 0)
            );
        }

        private Item? AddDropToNPC(string[] dropsInfo)
        {
            return dropsInfo[3] switch
            {
                "0" => new Item(dropsInfo[4], int.Parse(dropsInfo[5]), dropsInfo[6]),

                "1" => new OffHand(dropsInfo[4], int.Parse(dropsInfo[5]),
                                   int.Parse(dropsInfo[6]), int.Parse(dropsInfo[7])),

                "2" => new Weapon(dropsInfo[4], int.Parse(dropsInfo[5])),

                _ => null
            };
        }
    }
    
}
