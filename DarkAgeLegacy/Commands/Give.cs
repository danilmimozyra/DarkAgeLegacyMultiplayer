namespace DarkAgeLegacyServer
{
    internal class Give : Command
    {
        private Random rd;

        public Give(Map map) : base(map)
        {
            rd = new Random();
        }

        public override string Execute(Player player, string value)
        {
            if (map.MapProp[player.CurrentRoom].FindNPC("wounded-knight") != null && value.Equals("healing-potion") &&
                player.HasItem("healing-potion"))
            {
                var potion = player.FindItem("healing-potion");
                potion.ChangeAmount(-1);

                if (potion.Amount <= 0)
                {
                    player.RemoveItem("healing-potion");
                }

                switch (rd.Next(3))
                {
                    case 0:
                        map.MapProp[player.CurrentRoom].AddItem(new Item("Quiver", 0, "n"));
                        return "Thank you, Blair, here's a 'Quiver' for you.";
                    case 1:
                        map.MapProp[player.CurrentRoom].AddItem(new Item("Grindstone", 0, "n"));
                        return "Thank you, Blair, here's a 'Grindstone' for you.";
                    case 2:
                        map.MapProp[player.CurrentRoom].AddItem(new Item("Talisman", 0, "n"));
                        return "Thank you, Blair, here's a 'Talisman' for you.";
                }
            }


            return "There's nothing you can give.";
        }

        public override bool Exit()
        {
            return false;
        }
    }
}