using System.Collections.Generic;
using UnityEngine;

namespace VertigoCase.Systems.ZoneSystem
{
    public abstract class ZoneBaseSO : ScriptableObject
    {
        public abstract ZoneType ZoneType { get; }
        public List<RewardSO> rewardedData = new List<RewardSO>();

        [Header("Zone Level Settings")]
        public int triggerInterval = 5;
        public float rewardValueMultiplier = 0.01f;
    }
}
