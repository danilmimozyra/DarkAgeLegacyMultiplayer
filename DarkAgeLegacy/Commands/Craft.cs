namespace DarkAgeLegacyServer
{
    internal class Craft : Command
    {
        private bool dead;
        private Dictionary<string, string[][]> crafts;

        public Craft(Map map) : base(map)
        {
            dead = false;
            crafts = new Dictionary<string, string[][]>();
            LoadCrafts();
        }

        public override string Execute(Player player, string value)
        {
            if (value == "") return "There is no such crafting recipe.";

            string name = value.ToLower();
            string line;
            if (crafts.TryGetValue(name, out string[][] recipe))
            {
                for (int i = 0; i < recipe.Length - 1; i++)
                {
                    var ingredientName = recipe[i][0];
                    int requiredAmount = int.Parse(recipe[i][1]);
                    var playerItem = player.FindItem(ingredientName);

                    if (playerItem == null || playerItem.Amount < requiredAmount)
                    {
                        return "You don't have enough materials.";
                    }
                }
                for (int i = 0; i < recipe.Length - 1; i++)
                {
                    string ingredientName = recipe[i][0];
                    int amountToRemove = int.Parse(recipe[i][1]) * -1;
                    player.ChangeAmount(ingredientName, amountToRemove);
                }
                Item item = CraftItem(name, recipe[recipe.Length - 1]);
                map.MapProp[player.CurrentRoom].AddItem(item);

                line = $"You've crafted {item.Name}. It is now laying on the ground.";
                line += AttackPlayer(player, map.MapProp[player.CurrentRoom].AttackedEnemy);

                if (player.Health <= 0)
                {
                    dead = true;
                    line += "\nYou had died.";
                }
            }
            else
            {
                line = "There is no such crafting recipe.";
            }

            return line;
        }

        private void LoadCrafts()
        {
            string filePath = "res/crafts.txt";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] craftsInfo = line.Split(';');

                    string[] name = new string[2];
                    name[0] = craftsInfo[0];
                    name[1] = craftsInfo[2];

                    string[] ingredients = craftsInfo[1].Split(',');
                    string[][] recipe = new string[ingredients.Length / 2 + 1][];

                    int j = 0;
                    for (int i = 0; i < ingredients.Length - 1; i += 2)
                    {
                        string[] temp = new string[2];
                        temp[0] = ingredients[i];
                        temp[1] = ingredients[i + 1];
                        recipe[j] = temp;
                        j++;
                    }

                    recipe[recipe.Length - 1] = name[1].Split(',');
                    crafts[name[0].ToLower()] = recipe;
                }
            }
        }

        private Item? CraftItem(string name, string[] craftsInfo)
        {
            return craftsInfo[0] switch
            {
                "0" => new Item(name, int.Parse(craftsInfo[1]), craftsInfo[2]),

                "1" => new OffHand(craftsInfo[4], int.Parse(craftsInfo[5]),
                    int.Parse(craftsInfo[6]), int.Parse(craftsInfo[7])),

                "2" => new Weapon(craftsInfo[4], int.Parse(craftsInfo[5])),

                _ => null
            };
        }

        public override bool Exit()
        {
            return dead;
        }
    }
}