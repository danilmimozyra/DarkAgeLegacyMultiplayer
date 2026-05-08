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
        private Weapon? weapon;
        private OffHand? offHand;

        public int CurrentRoom
        {
            get { return currentRoom; }
            set { currentRoom = value; }
        }

        public int Health
        {
            get => health;
            set => health = value;
        }

        public int Defence
        {
            get
            {
                if (offHand != null)
                {
                    return defence + offHand.DefenceBuff;
                }

                return defence;
            }
            set => defence += value;
        }

        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public string Username
        {
            get => username;
            set => username = value;
        }

        public Weapon Weapon
        {
            get => weapon;
            set => weapon = value;
        }

        public OffHand OffHand
        {
            get => offHand;
            set
            {
                offHand = value;
                SetMaxHealth();
            }
        }

        public Player(string username)
        {
            this.username = username;
            currentRoom = 1;
            maxHealth = 100;
            health = 100;
            defence = 0;
            inventory = new Inventory();
        }

        private void SetMaxHealth()
        {
            maxHealth = 100 + offHand.HealthBuff;
            if (maxHealth == health + offHand.HealthBuff)
            {
                Health = maxHealth;
            }
            else if (health > maxHealth)
            {
                Health = maxHealth;
            }
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
            for (int i = 0; i < inventory.Items.Length; i++)
            {
                if (inventory.Items[i] != null)
                {
                    if (inventory.Items[i].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int GetDamage()
        {
            int d = 0;
            if (weapon != null)
            {
                d += weapon.Damage;
                if (HasItem("Quiver") && weapon.Name.Equals("Crossbow", StringComparison.OrdinalIgnoreCase))
                {
                    d += 5;
                }
                else if (HasItem("Grindstone") && weapon.Name.Equals("Broadsword", StringComparison.OrdinalIgnoreCase))
                {
                    d += 5;
                }
                else if (HasItem("Talisman") && weapon.Name.Equals("Fire-Staff", StringComparison.OrdinalIgnoreCase))
                {
                    d += 5;
                }
            }
            else
            {
                d = 5;
            }

            if (offHand != null)
            {
                d += offHand.DamageBuff;
            }

            return d;
        }

        public bool AddItem(Item item)
        {
            return inventory.AddItem(item);
        }

        public Item FindItem(string name)
        {
            Item item;
            for (int i = 0; i < inventory.Items.Length; i++)
            {
                if (inventory.Items[i] != null)
                {
                    if (inventory.Items[i].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        item = inventory.Items[i];
                        return item;
                    }
                }
            }

            return null;
        }

        public Item? RemoveItem(string name)
        {
            Item item;
            for (int i = 0; i < inventory.Items.Length; i++)
            {
                if (inventory.Items[i] != null)
                {
                    if (inventory.Items[i].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        item = inventory.Items[i];
                        inventory.Items[i] = null;
                        return item;
                    }
                }
            }

            return null;
        }

        public void ChangeAmount(string name, int amount)
        {
            for (int i = 0; i < inventory.Items.Length; i++)
            {
                var item = inventory.Items[i];

                if (item != null && item.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    item.ChangeAmount(amount);

                    if (item.Amount <= 0)
                    {
                        RemoveItem(name);
                    }
                }
            }
        }

        public string InventoryDescription()
        {
            string line = "Your health is " + Health + "/" + MaxHealth + ".";
            if (weapon != null)
            {
                line += "\n" + weapon.Description();
            }

            if (offHand != null)
            {
                line += "\n" + offHand.Description();
            }

            line += "\n" + inventory.Description();
            return line;
        }
    }
}