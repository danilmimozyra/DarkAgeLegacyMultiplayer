namespace DarkAgeLegacyServer
{
    internal class Puzzle : Command
    {
        private bool sof;
        private bool lib;

        public Puzzle(Map map) : base(map)
        {
            sof = true;
            lib = true;
        }

        public override string Execute(Player player, string value)
        {
            string name = map.MapProp[player.CurrentRoom].RoomName();

            switch (name)
            {
                case "Prison Cell":
                    if (map.MapProp[player.CurrentRoom].FindNPC("wounded-knight") != null)
                    {
                        return
                            "Looking for puzzle, Blair, aren't you? There's no puzzles here, but if you can give me a Healing-potion, I might be able to help you.\n" +
                            "You can craft it using one piece Flesh and one Moss.\n" +
                            "- says the Wounded-Knight.\n";
                    }

                    break;

                case "Library":
                    if (lib)
                    {
                        Console.Write("I am a house with countless doors,\n" +
                                      "Each one leads to distant shores.\n" +
                                      "Silent guides in rows I keep,\n" +
                                      "Where stories wake and knowledge sleeps.\n" +
                                      "What am I?\n" +
                                      ">> ");

                        string answer = Console.ReadLine();
                        if (answer != null && answer.Equals("library", StringComparison.OrdinalIgnoreCase))
                        {
                            map.MapProp[player.CurrentRoom].WestRoom = 5;
                            lib = false;
                            return "A shelf on the west is starting to collapse. You can see some strange corridor.";
                        }
                        
                        return "- No";
                        
                    }

                    break;

                case "Sanctuary of Light":
                    if (sof)
                    {
                        Console.Write(" I am not seen, yet clear to all,\n" +
                                      "A single spark, and doubts will fall.\n" +
                                      "I dwell in minds, yet light the skies,\n" +
                                      "Destroying darkness with my rise.\n" +
                                      "What am I?\n" +
                                      ">> ");

                        string answer = Console.ReadLine();
                        if (answer != null && answer.Equals("knowledge", StringComparison.OrdinalIgnoreCase))
                        {
                            Item amulet = new Item("Amulet-of-Light", 0, "a");
                            map.MapProp[player.CurrentRoom].AddItem(amulet);
                            sof = false;
                            return "A bright beam of light gave you 'Amulet-of-Light'.";
                        }
                        return "- No";
                        
                    }

                    break;
            }

            return "Seems like there isn't any puzzles in this room.";
        }

        public override bool Exit()
        {
            return false;
        }
    }
}