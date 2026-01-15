using UnityEngine;

namespace VertigoCase.Systems.ZoneSystem
{
    
    [CreateAssetMenu(fileName = "Reward_", menuName = "Vertigo/WheelGame/ZoneSystem/Reward")]
    public class RewardSO : ScriptableObject
    {
        [Header("Identity")][Tooltip("Genel Kimlik Bilgileri")]
        public RewardType rewardType= RewardType.Chest;
        public string rewardName;
        public ZoneType zoneType;
        [Tooltip("Super Zone bölgesinden bir ödül mü? değil mi?")]internal bool isSpecial=>  (int)rewardType >= 30;

        [Header("Visual")][Tooltip("Resim ve ikon bilgileri")] 
        public Sprite icon;

        [Header("Value")][Tooltip("Minimum Taban Değeri")] 
        [SerializeField] private int baseAmount;

        [Header("Scale Settings")][Tooltip("Boyutlandırma ve ölçeklendirme ayarları")] 
        [SerializeField] private float scale;
        [SerializeField] private float scaleMultiplier = 1.03f;

        public float CalculateScale(int zoneLevel)
        {
            return scale * Mathf.Pow(scaleMultiplier, zoneLevel);
        }

        public int CalculateRewardAmount(float multiplier)
        {
            return Mathf.RoundToInt(baseAmount + baseAmount * multiplier);
        }
    }
}
