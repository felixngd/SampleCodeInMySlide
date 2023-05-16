namespace BattlePass.AdhereSRP
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
    }
}