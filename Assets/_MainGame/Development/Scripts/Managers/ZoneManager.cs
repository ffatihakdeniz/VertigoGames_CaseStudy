using System.Collections.Generic;
using UnityEngine;
using Patterns.Singleton;
using VertigoCase.Runtime;

namespace VertigoCase.Systems.ZoneSystem
{
    public class ZoneManager : MonoSingleton<ZoneManager>, IGameDataConsumer
    {
        [SerializeField] private List<ZoneBaseSO> zoneDataList;
        GameDataSO gameData;
        private ZoneRewardResolver _rewardResolver;

        public int StartedLevel => gameData.startedLevel;
        public int IntervalLevelBySuperZone
        {
            get
            {
                var superZone = GetZoneSOByZoneType(ZoneType.Super);
                if (superZone != null)
                    return superZone.triggerInterval;
                throw new System.Exception("Super Zone SO eklenmemis");
            }
        }
        public int IntervalLevelBySafeZone
        {
            get
            {
                var safeZone = GetZoneSOByZoneType(ZoneType.Safe);
                if (safeZone != null)
                    return safeZone.triggerInterval;
                throw new System.Exception("Safe Zone SO eklenmemis");
            }
        }

        public int CurrentLevel
        {
            get => gameData.currentLevel;
            private set => gameData.currentLevel = Mathf.Max(0, value);
        }

        public int CurrentZoneGeneralIntervalCount
        {
            get => gameData.currentZoneIntervalCount;
            private set => gameData.currentZoneIntervalCount = Mathf.Max(0, value);
        }
        public void IncreaseZoneGeneralIntervalCount()
        {
            CurrentZoneGeneralIntervalCount++;
        }
        public void IncreaseLevel()
        {
            CurrentLevel++;
        }

        public float CurrentLevelRewardMultiplier
        {
            get
            {
                float multiplier = zoneDataList.Find(z => z.zoneType == GetZoneTypeByLevel()).rewardValueMultiplier;
                return Mathf.Pow(1f + gameData.zoneMultiplierIncreaseRate, CurrentZoneGeneralIntervalCount) * multiplier;
            }
        }



        public void Initialize(GameDataSO gameData)
        {
            this.gameData = gameData;
            _rewardResolver = new ZoneRewardResolver(zoneDataList);

        }
        void OnChangedZoneLevelHandler()
        {
            //TODO
        }

        public ZoneType GetZoneTypeByLevel() // Mevcut seviye hangi zone grubun ait
        {
            return _rewardResolver.GetZoneByLevel(CurrentLevel);
        }
        public ZoneBaseSO GetZoneByLevel() // Mevcut seviye hangi zone grubun ait
        {
            return zoneDataList.Find(z => z.zoneType == GetZoneTypeByLevel());
        }
        public ZoneInfoSuperReward GetSuperZoneNextRewardByInterval()// Siradaki super bolge seviyesi ve odul gorseli
        {
            return _rewardResolver.GetSuperZoneNextRewardByInterval(CurrentLevel);
        }
        public int GetSafeZoneNextRewardByInterval() // Siradaki guvenli bolge seviyesi
        {
            return _rewardResolver.GetSafeZoneNextRewardByInterval(CurrentLevel);
        }
        public ZoneBaseSO GetZoneSOByZoneType(ZoneType zoneType)
        {
            return zoneDataList.Find(z => z.zoneType == zoneType);
        }










    }
}
