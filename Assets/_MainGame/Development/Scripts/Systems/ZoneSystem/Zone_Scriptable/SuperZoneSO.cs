using UnityEngine;
using System.Collections.Generic;

namespace VertigoCase.Systems.ZoneSystem
{
    [CreateAssetMenu(fileName = "SuperZone", menuName = "Vertigo/WheelGame/ZoneSystem/SuperZone")]
    public class SuperZoneSO : ZoneBaseSO
    {
        public override ZoneType ZoneType => ZoneType.Super;

        [Header("Super Zone Settings")][Tooltip("Manuel aşağıda eklendiği şekilde mi seviyeler belirlensin? yoksa step bazlı mı?")]
        public bool useIntervalProgression = true;
        public List<ZoneInfoSuperReward> zoneInfoSuperRewards;
    }
}
