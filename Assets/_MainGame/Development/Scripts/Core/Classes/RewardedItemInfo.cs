using UnityEngine;
using VertigoCase.Systems.ZoneSystem;

namespace VertigoCase.Runtime
{
    [System.Serializable]
    public class RewardedItemInfo
    {
        [SerializeField] private RewardDataSO RewardData;
        [SerializeField] public int RewardAmount;
        public readonly Sprite RewardIcon;
        public readonly int RewardID;
        public readonly string RewardName;
        public readonly RewardType RewardType;
        public readonly float ScaleMultiplierIcon;


        public RewardedItemInfo(RewardDataSO rewardSO, int rewardAmount)
        {
            RewardData = rewardSO;
            RewardAmount = rewardAmount;

            if (rewardSO == null)
            {
                RewardID = -1;
                RewardName = string.Empty;
                RewardIcon = null;
                return;
            }

            RewardID = rewardSO.rewardID;
            RewardName = rewardSO.rewardName;
            RewardIcon = rewardSO.icon;
            RewardType = rewardSO.rewardType;
            ScaleMultiplierIcon = RewardData.scaleMultiplier;
        }
        public Vector2 CalculateRectUIIconSize(Vector2 currentSize)
        {
            return RewardData.CalculateRectUIIconSize(currentSize);
        }
    }
}