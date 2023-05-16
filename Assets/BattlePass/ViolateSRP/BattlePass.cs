using System.Collections.Generic;

namespace BattlePass.ViolateSRP
{
    // BattlePass.cs - class for the Battle Pass system
    public class BattlePass
    {
        public List<BattlePassReward> rewards;
        public int currentLevel;
        public int maxLevel;

        public BattlePass()
        {
            rewards = new List<BattlePassReward>();
            currentLevel = 1;
            maxLevel = 100;
        }

        public void ClaimReward()
        {
            BattlePassReward reward = rewards[currentLevel - 1];
            reward.Claim();
        }

        public void LevelUp()
        {
            if (currentLevel < maxLevel)
            {
                currentLevel++;
                CheckRewards();
            }
        }

        private void CheckRewards()
        {
            foreach (BattlePassReward reward in rewards)
            {
                if (currentLevel >= reward.level && !reward.claimed)
                {
                    reward.Notify();
                }
            }
        }
    }
}