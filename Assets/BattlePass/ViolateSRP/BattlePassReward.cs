namespace BattlePass.ViolateSRP
{
    // BattlePassReward.cs - class for battle pass rewards
    public class BattlePassReward
    {
        public int level;
        public bool claimed;

        public BattlePassReward(int level)
        {
            this.level = level;
            claimed = false;
        }

        public void Notify()
        {
            // notify the player that the reward is available
        }

        public void Claim()
        {
            // give the reward to the player
            claimed = true;
        }
    }
}