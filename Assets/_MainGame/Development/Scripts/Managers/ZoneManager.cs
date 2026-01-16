using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Singleton;
using VertigoCase.Runtime;
using VertigoCase.Helpers.Extensions;
using System.Linq;

namespace VertigoCase.Systems.ZoneSystem
{
    public class ZoneManager : MonoSingleton<ZoneManager>, IGameDataConsumer
    {
        [SerializeField] private List<ZoneBaseSO> zoneDataList;
        GameDataSO gameData;
        private ZoneRewardResolver _rewardResolver;


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
                float multiplier = zoneDataList.Find(z => z.ZoneType == GetZoneByLevel()).rewardValueMultiplier;
                return Mathf.Pow(1f + gameData.zoneMultiplierIncreaseRate, CurrentZoneGeneralIntervalCount)*multiplier;
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

        public ZoneType GetZoneByLevel() // Mevcut seviye hangi zone grubun ait
        {
            return _rewardResolver.GetZoneByLevel(CurrentLevel);
        }
        public ZoneInfoSuperReward GetSuperZoneNextRewardByInterval()// Siradaki super bolge seviyesi ve odul gorseli
        {
            return _rewardResolver.GetSuperZoneNextRewardByInterval(CurrentLevel);
        }
        public int GetSafeZoneNextRewardByInterval() // Siradaki guvenli bolge seviyesi
        {
            return _rewardResolver.GetSafeZoneNextRewardByInterval(CurrentLevel);
        }










        
    }
}
/*
public ZoneInfoSuperReward GetSuperZoneRewardByLevel()
        {
            if(GetZoneByLevel() != ZoneType.Super)
                return null;
            var superZone = zoneDataList.Find(z => z.zoneType == ZoneType.Super) as SuperZoneSO;
            if (superZone == null)
                return null;

            if (superZone.useIntervalProgression)
            {
                int intervalIndex = CurrentLevel / superZone.triggerInterval;
                if (intervalIndex < superZone.zoneInfoSuperRewards.Count)
                    return superZone.zoneInfoSuperRewards[intervalIndex];
            }
            else
            {
                var rndReward = superZone.zoneInfoSuperRewards[Random.Range(0, superZone.zoneInfoSuperRewards.Count)].image;
                var newInfoPanelReward = new ZoneInfoSuperReward(rndReward, CurrentLevel);
                return superZone.zoneInfoSuperRewards.Find(r => r.CurrentLevel == CurrentLevel);
            }
        }
*/