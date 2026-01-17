using UnityEngine;

namespace VertigoCase.Systems.ZoneSystem
{

    [CreateAssetMenu(fileName = "Reward_", menuName = "Vertigo/WheelGame/ZoneSystem/Reward")]
    public class RewardSO : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Genel Kimlik Bilgileri")]
        public RewardType rewardType = RewardType.Chest;
        public string rewardName;
        public int rewardID;
        public ZoneType zoneType;


        [Header("Visual")]
        [Tooltip("Resim ve ikon bilgileri")]
        public Sprite icon;


        [Header("Value")]
        [Tooltip("Minimum Taban Değeri")]
        [SerializeField] private int baseAmount;


        [Header("Scale Settings")]
        [Tooltip("Boyutlandırma ve ölçeklendirme ayarları")]
        public float scaleMultiplier = 1f;


        public int CalculateRewardAmount(float multiplier)
        {
            return Mathf.RoundToInt(baseAmount + baseAmount * multiplier);
        }
        public Vector2 CalculateRectUIIconSize(Vector2 currentSize)
        {
            return currentSize * scaleMultiplier;
        }
    }
}
