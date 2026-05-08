namespace DarkAgeLegacyServer
{
    internal class Give : Command
    {
        private Random rd;
        private List<GiveReward> rewards;

        public Give(Map map) : base(map)
        {
            rd = new Random();
            rewards = LoadRewards();
        }

        public override string Execute(Player player, string value)
        {
            List<GiveReward> possibleRewards = rewards.Where(reward =>
                map.MapProp[player.CurrentRoom].FindNPC(reward.NpcName) != null
                && value.Equals(reward.RequiredItem, StringComparison.OrdinalIgnoreCase)
                && player.HasItem(reward.RequiredItem)).ToList();

            if (possibleRewards.Count > 0)
            {
                GiveReward reward = possibleRewards[rd.Next(possibleRewards.Count)];
                var item = player.FindItem(reward.RequiredItem);
                item.ChangeAmount(-1);

                if (item.Amount <= 0)
                {
                    player.RemoveItem(reward.RequiredItem);
                }

                map.MapProp[player.CurrentRoom].AddItem(reward.CreateItem());
                return reward.Message;
            }


            return "There's nothing you can give.";
        }

        public override bool Exit()
        {
            return false;
        }

        private List<GiveReward> LoadRewards()
        {
            List<GiveReward> loadedRewards = new List<GiveReward>();
            string filePath = "res/give_rewards.txt";

            if (!File.Exists(filePath))
            {
                return loadedRewards;
            }

            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] rewardInfo = line.Split(",", 7);

                if (rewardInfo.Length == 7)
                {
                    loadedRewards.Add(new GiveReward(rewardInfo));
                }
            }

            return loadedRewards;
        }

        private class GiveReward
        {
            private readonly string[] rewardInfo;

            public string NpcName { get; }
            public string RequiredItem { get; }
            public string Message { get; }

            public GiveReward(string[] rewardInfo)
            {
                this.rewardInfo = rewardInfo;
                NpcName = rewardInfo[0];
                RequiredItem = rewardInfo[1];
                Message = rewardInfo[6];
            }

            public Item? CreateItem()
            {
                return rewardInfo[2] switch
                {
                    "0" => new Item(rewardInfo[3], int.Parse(rewardInfo[4]), rewardInfo[5]),
                    "1" => new OffHand(rewardInfo[3], int.Parse(rewardInfo[4]), int.Parse(rewardInfo[5]), int.Parse(rewardInfo[6])),
                    "2" => new Weapon(rewardInfo[3], int.Parse(rewardInfo[4])),
                    _ => null
                };
            }
        }
    }
}
