namespace BattlePass.AdhereSRP
{
    // BattlePassRewardClaimer.cs - class for claiming rewards
    public class BattlePassRewardClaimer
    {
        public void Claim(BattlePassReward reward)
        {
            // give the reward to the player
            reward.claimed = true;
        }
    }
}