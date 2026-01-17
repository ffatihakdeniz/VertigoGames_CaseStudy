using System.Collections.Generic;
using UnityEngine;

namespace VertigoCase.Systems.ZoneSystem
{
    public abstract class ZoneBaseSO : ScriptableObject
    {
        public abstract ZoneType zoneType { get; }

        [Header("Slide Info Panel Settings")]
        public Color slidePanelLevelPointColor = Color.white;
        public Color slidePanelLevelTextCursorColor = Color.black;
        //public Sprite slidePanelCursorImage;
        //public List<RewardSO> rewardedData = new List<RewardSO>();

        [Header("Zone Level Settings")]
        public int triggerInterval = 5;
        public float rewardValueMultiplier = 0.01f;
    }
}
