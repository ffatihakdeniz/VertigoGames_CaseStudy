using System.Collections.Generic;
using UnityEngine;
using Patterns.Singleton;
using VertigoCase.Runtime;
using VertigoCase.Runtime.Data;

namespace VertigoCase.Systems.ZoneSystem
{
    public class ZoneManager : MonoSingleton<ZoneManager>, IGameDataConsumer
    {
        [Header("Zone Data")]
        [SerializeField] private List<ZoneBaseSO> zoneDataList;
        [Header("Reward Data")]
        [SerializeField] private List<RewardDataSO> rewardDataList;
        GameDataSO gameData;
        private ZoneResolver _zoneResolver;
        private RewardResolver _rewardResolver;

        public void Initialize(GameDataSO gameData)
        {
            this.gameData = gameData;
            _zoneResolver = new ZoneResolver(zoneDataList);
            _rewardResolver = new RewardResolver(rewardDataList);
        }
        void OnEnable()
        {
            EventBus.Subscribe<ChangedLevelEvent>(OnNewLevelStartHandler);
        }
        void OnDisable()
        {
            EventBus.Subscribe<ChangedLevelEvent>(OnNewLevelStartHandler);
        }
        void OnNewLevelStartHandler()
        {
            //TODO
        }

        public List<RewardedItemInfo> GetRewardsByZoneType()
        {
            //print("Zone Type: " + GetZoneTypeByLevel() + " Level: " + CurrentLevel + " Multiplier: " + CurrentLevelRewardMultiplier);
            return _rewardResolver.CreateRewardedItemList(CurrentLevelRewardMultiplier, GetZoneTypeByLevel());
        }

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
        public ZoneType CurrentZoneType => _zoneResolver.GetZoneByLevel(CurrentLevel);

        public int CurrentZoneGeneralIntervalCount
        {
            get => gameData.currentZoneIntervalCount;
            private set => gameData.currentZoneIntervalCount = Mathf.Max(1, value);
        }
        public void IncreaseZoneGeneralIntervalCount()
        {
            CurrentZoneGeneralIntervalCount++;
        }
        public void IncreaseLevel()
        {
            CurrentLevel++;
        }

        public float CurrentLevelRewardMultiplier //Her safe-super zonedan gecildiginde odul artis orani
        {
            get
            {
                float zoneMultiplier = zoneDataList.Find(z => z.zoneType == GetZoneTypeByLevel()).rewardValueMultiplier;
                return Mathf.Pow(1f + gameData.zoneMultiplierIncreaseRate, CurrentZoneGeneralIntervalCount) * zoneMultiplier;
            }
        }

        public ZoneType GetZoneTypeByLevel() // Mevcut seviye hangi zone grubun ait
        {
            return _zoneResolver.GetZoneByLevel(CurrentLevel);
        }
        public ZoneBaseSO GetZoneByLevel() // Mevcut seviye hangi zone grubun ait
        {
            return zoneDataList.Find(z => z.zoneType == GetZoneTypeByLevel());
        }
        public ZoneInfoSuperReward GetSuperZoneNextRewardByInterval()// Siradaki super bolge seviyesi ve odul gorseli
        {
            return _zoneResolver.GetSuperZoneNextRewardByInterval(CurrentLevel);
        }
        public int GetSafeZoneNextRewardByInterval() // Siradaki guvenli bolge seviyesi
        {
            return _zoneResolver.GetSafeZoneNextRewardByInterval(CurrentLevel);
        }
        public ZoneBaseSO GetZoneSOByZoneType(ZoneType zoneType)
        {
            return zoneDataList.Find(z => z.zoneType == zoneType);
        }










    }
}
