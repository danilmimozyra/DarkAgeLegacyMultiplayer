using System.Text;

namespace DarkAgeLegacyServer
{
    internal class Attack : Command
    {
        private bool dead;
        
        public Attack(Map map) : base(map)
        {
            dead = false;
        }

        public override string Execute(Player player, string value)
        {
            string line;
            NPC n = map.MapProp[player.CurrentRoom].FindNPC(value);
            if (n != null)
            {
                n.SufferDamage(player.GetDamage());
                if (n.Health <= 0)
                {
                    List<Item> items = n.Drop();
                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            map.MapProp[player.CurrentRoom].AddItem(item);
                        }
                    }
                    map.MapProp[player.CurrentRoom].RemoveNPC(n);
                    line = KillDescription(n, items);
                    map.MapProp[player.CurrentRoom].AttackedEnemy = null;
                }
                else
                {
                    line = $"You've attacked {n.Name}. His remaining health is {n.Health}.{AttackPlayer(player, n)}";
                    if (player.Health <= 0)
                    {
                        dead = true;
                        line += "\nYou died.";
                    }
                }
            }
            else
            {
                line = "There is no such enemy";
            }
            return line;
        }
        
        private string KillDescription(NPC n, List<Item> items)
        {
            var sb = new StringBuilder($"You've killed {n.Name}.");
            if (items != null && items.Count > 0)
            {
                sb.Append(" He dropped ");
                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    sb.Append($"'{item.Name}'");
                    if (item.Amount > 1)
                    {
                        sb.Append($"({item.Amount})");
                    }
                    if (i <= items.Count - 3)
                    {
                        sb.Append(", ");
                    }
                    else if (i <= items.Count - 2)
                    {
                        sb.Append(" and ");
                    }
                    else if (i == items.Count - 1)
                    {
                        sb.Append(".");
                    }
                }
            }
            if (string.Equals(n.Name, "Anthrax"))
            {
                sb.Append("""

                          Some strange portal has opened in the room. Seems like it leads to the surface.
                          "My work is done here, it is time to go home" - sayd Blair and jumped in the portal
                          """);
                dead = true;
            }
            return sb.ToString();
        }

        public override bool Exit()
        {
            return dead;
        }
    }
}
