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
        public string Name {
            get
            {
                string line = $"You are now in {name}. ";

                if (westRoom != 0)
                    line += "You can see an entrance on the west. ";

                if (northRoom != 0)
                    line += "There is a path on the north. ";

                if (eastRoom != 0)
                    line += "The doors on the east can be entered. ";

                if (southRoom != 0)
                    line += "You can go to the room on the south. ";

                return line;
            }
        }

        public string RoomInfo()
        {
            return Name + "\nTo move between rooms use the command 'go'. The entry should look like this: go [direction]." +
                    "\n" + ItemsList() + "\n" + NPCList();
        }

        public string RoomDescription()
        {
            return Name + "\n" + ItemsList() + "\n" + NPCList();
        }

        public NPC? FindNPC(string name)
        {
            foreach (NPC n in npcs)
            {
                if (n.Name.Equals(name))
                {
                    return n;
                }
            }
            return null;
        }

        public void AddNPC(NPC n)
        {
            npcs.Add(n);
        }

        public void RemoveNPC(NPC n)
        {
            npcs.Remove(n);
        }

        public void AddItem(Item item)
        {
            if (item != null && items.Contains(item))
            {
                int index = items.IndexOf(item);
                items[index].ChangeAmount(item.Amount);
            }
            else if (item != null)
            {
                items.Add(item);
            }
        }

        public void RemoveItem(Item item)
        {
            items.Remove(item);
        }

        public Item? FindItem(string name)
        {
            foreach (Item item in items)
            {
                if (item != null && item.Name.Equals(name))
                {
                    return item;
                }
            }
            return null;
        }

        public string ItemsList()
        {
            var validItems = items.Where(item => item != null).ToList();

            if (validItems.Count == 0)
            {
                return "There are no items in the room.";
            }

            StringBuilder sb = new StringBuilder("You can see a ");

            for (int i = 0; i < validItems.Count; i++)
            {
                Item item = validItems[i];

                sb.Append($"'{item.Name}'");

                if (item.Amount > 1)
                {
                    sb.Append($"({item.Amount})");
                }

                if (i < validItems.Count - 2)
                {
                    sb.Append(", ");
                }
                else if (i == validItems.Count - 2)
                {
                    sb.Append(" and ");
                }
                else
                {
                    sb.Append(" laying on the floor.");
                }
            }

            return sb.ToString();
        }

        public string NPCList()
        {
            var tempEnemy = npcs.OfType<Enemy>().ToList();
            var tempNPC = npcs.Where(n => !(n is Enemy)).ToList();

            if (tempNPC.Count > 0)
            {
                string line = NpcDescription(tempNPC);
                if (tempEnemy.Count > 0)
                {
                    line += "\n" + EnemyDescription(tempEnemy);
                }
                return line;
            }

            if (tempEnemy.Count > 0)
            {
                return EnemyDescription(tempEnemy);
            }

            return "There is no one in this room.";
        }

        private string NpcDescription(List<NPC> tempNpc)
        {
            if (tempNpc == null || tempNpc.Count == 0)
            {
                return "";
            }

            string startText = tempNpc.Count > 1 ? "There are NPCs " : "There is NPC ";
            StringBuilder sb = new StringBuilder(startText);

            for (int i = 0; i < tempNpc.Count; i++)
            {
                sb.Append($"'{tempNpc[i].Name}'");
                if (i < tempNpc.Count - 2)
                {
                    sb.Append(", ");
                }
                else if (i == tempNpc.Count - 2)
                {
                    sb.Append(" and ");
                }
                else
                {
                    sb.Append(" in the room.");
                }
            }

            return sb.ToString();
        }

        private string EnemyDescription(List<Enemy> tempEnemy)
        {
            if (tempEnemy == null || tempEnemy.Count == 0)
            {
                return "";
            }

            string startText = tempEnemy.Count > 1 ? "There are Enemies " : "There is an Enemy ";
            StringBuilder sb = new StringBuilder(startText);

            for (int i = 0; i < tempEnemy.Count; i++)
            {
                sb.Append($"'{tempEnemy[i].Name}'");
                if (i < tempEnemy.Count - 2)
                {
                    sb.Append(", ");
                }
                else if (i == tempEnemy.Count - 2)
                {
                    sb.Append(" and ");
                }
                else
                {
                    sb.Append(" in the room.");
                }
            }

            return sb.ToString();
        }

        public int GetSize()
        {
            return items.Count(item => item != null);
        }
    }
}
