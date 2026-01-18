using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static VertigoCase.Helpers.Extensions.MathExtensions;

namespace VertigoCase.Systems.ZoneSystem
{
    public class ZoneResolver
    {
        private readonly List<ZoneBaseSO> _zoneDataList;

        public ZoneResolver(List<ZoneBaseSO> zoneDataList)
        {
            _zoneDataList = zoneDataList;
        }

        public ZoneType GetZoneByLevel(int level)
        {
            if (level < 0)
                return ZoneType.None;

            var superZone = GetZoneSOByZoneType(ZoneType.Super);
            var safeZone = GetZoneSOByZoneType(ZoneType.Safe);

            if (superZone != null && superZone.triggerInterval > 0 && level % superZone.triggerInterval == 0)
                return ZoneType.Super;

            if (safeZone != null && safeZone.triggerInterval > 0 && level % safeZone.triggerInterval == 0)
                return ZoneType.Safe;

            return ZoneType.Normal;
        }

        public ZoneInfoSuperReward GetSuperZoneNextRewardByInterval(int currentLevel)
        {
            var superZone = GetZoneSOByZoneType(ZoneType.Super) as SuperZoneSO;
            if (superZone == null)
                throw new Exception("Super Zone SO eklenmemis");

            if (superZone.useIntervalProgression)
            {
                int intervalIndex = IntervalIndexByLevel(superZone.triggerInterval, currentLevel);
                if (intervalIndex < superZone.zoneInfoSuperRewards.Count)
                    return superZone.zoneInfoSuperRewards[Mathf.Max(0, intervalIndex - 1)];
            }
            else
            {
                var rndReward = superZone.zoneInfoSuperRewards[UnityEngine.Random.Range(0, superZone.zoneInfoSuperRewards.Count)].reward;
                var newInfoPanelReward = new ZoneInfoSuperReward(
                    rndReward,
                    IntervalIndexByLevel(superZone.triggerInterval, currentLevel) * superZone.triggerInterval
                );
                return newInfoPanelReward;
            }

            return null;
        }

        public int GetSafeZoneNextRewardByInterval(int currentLevel)
        {
            var safeZone = GetZoneSOByZoneType(ZoneType.Safe) as SafeZoneSO;
            var superZone = GetZoneSOByZoneType(ZoneType.Super) as SuperZoneSO;
            if (safeZone == null)
                throw new Exception("Safe Zone SO eklenmemis");

            int intervalIndex = IntervalIndexByLevel(safeZone.triggerInterval, currentLevel);
            int _retValue = safeZone.triggerInterval * intervalIndex; // Safezone'un superzone ile cakisma durumunun engellenmesi
            while (_retValue % superZone.triggerInterval == 0)
            {
                intervalIndex++;
                _retValue = safeZone.triggerInterval * intervalIndex;
            }
            return _retValue;
        }
        public ZoneBaseSO GetZoneSOByZoneType(ZoneType zoneType)
        {
            return _zoneDataList.Find(z => z.zoneType == zoneType);
        }
    }
}
