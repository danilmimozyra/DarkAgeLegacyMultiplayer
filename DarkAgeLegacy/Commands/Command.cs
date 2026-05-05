namespace DarkAgeLegacyServer
{
    internal abstract class Command
    {
        protected Map map;

        public Command(Map map)
        {
            this.map = map;
        }

        public abstract String Execute(Player player, string value);
        public abstract bool Exit();
        
        public virtual string? AttackPlayer(Player player, NPC? npc) {
            if (npc != null) {
                if (npc is Enemy or Boss) {
                    map.MapProp[player.CurrentRoom].AttackedEnemy = (Enemy) npc;
                    return "=================================================================================================" +
                           "=====================================================================\n" +
                           ((Enemy) npc).Attack(player);
                }
            }
            return "";
        }
    }
}
