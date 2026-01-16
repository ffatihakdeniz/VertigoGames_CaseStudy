using UnityEngine;
namespace VertigoCase.Systems.ZoneSystem
{
    [System.Serializable]
    public class ZoneInfoSuperReward
    {
        public ZoneInfoSuperReward(RewardSO reward, int level)
        {
            this.reward = reward;
            this.level = level;
        }
        public ZoneInfoSuperReward()
        {

        }
        public RewardSO reward;
        public int level;
    }
}
