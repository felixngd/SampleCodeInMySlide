using System.Collections.Generic;
using UnityEngine;

namespace BattlePass.AdhereSRP
{
    // BattlePass.cs - class for the Battle Pass system
    public class BattlePass
    {
        public List<BattlePassReward> rewards;
        public int currentLevel;
        public int maxLevel;
        private BattlePassRewardNotifier notifier;
        private BattlePassRewardClaimer claimer;

        public BattlePass()
        {
            rewards = new List<BattlePassReward>();
            currentLevel = 1;
            maxLevel = 100;
            notifier = new BattlePassRewardNotifier();
            claimer = new BattlePassRewardClaimer();
        }

        public void ClaimReward()
        {
            BattlePassReward reward = rewards[currentLevel - 1];
            claimer.Claim(reward);
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
                    notifier.Notify(reward);
                }
            }
        }
    }
}