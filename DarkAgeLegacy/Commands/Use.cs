namespace DarkAgeLegacyServer
{
    internal class Use : Command
    {
        public Use(Map map) : base(map)
        {
        }

        public override string Execute(Player player, string value)
        {
            if (value == "") return "This item has no abilities.";
            
            Item item = player.FindItem(value);
            string line;

            if (item != null)
            {
                switch (item.Ability)
                {
                    case Ability.h:
                        if (player.Health == player.MaxHealth)
                        {
                            line = "Your health is full.";
                        }
                        else
                        {
                            player.Health += 50;
                            if (player.Health > player.MaxHealth)
                            {
                                player.Health = player.MaxHealth;
                            }

                            player.ChangeAmount(value, -1);
                            var checkItem = player.FindItem(value);
                            if (checkItem != null && checkItem.Amount <= 0)
                            {
                                player.RemoveItem(value);
                            }

                            line = $"Your health is {player.Health}/{player.MaxHealth}.";
                        }

                        break;

                    case Ability.k:
                        if (map.MapProp[player.CurrentRoom].RoomName() == "Catacombs")
                        {
                            map.MapProp[player.CurrentRoom].SouthRoom = 10;
                            player.RemoveItem(value);
                            line = "The Throne Room has been opened.";
                        }
                        else
                        {
                            line = "Seems like there's nothing to be opened.";
                        }

                        break;

                    case Ability.s:
                        map.MapProp[player.CurrentRoom] = map.MapProp[1];
                        line = $"You have teleported to {map.MapProp[player.CurrentRoom].RoomName()}.";
                        break;

                    default:
                        line = "This item has no abilities.";
                        break;
                }

                return line + AttackPlayer(player, map.MapProp[player.CurrentRoom].AttackedEnemy);
            }
            return "This item has no abilities.";
        }

        public override bool Exit()
        {
            return false;
        }
    }
}