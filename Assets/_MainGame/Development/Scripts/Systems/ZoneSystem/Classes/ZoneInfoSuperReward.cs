using UnityEngine;
namespace VertigoCase.Systems.ZoneSystem
{
    [System.Serializable]
    public class ZoneInfoSuperReward
    {
        public ZoneInfoSuperReward(RewardDataSO reward, int level)
        {
            this.reward = reward;
            this.level = level;
        }
        public ZoneInfoSuperReward()
        {

        }
        public RewardDataSO reward;
        public int level;
    }
}
