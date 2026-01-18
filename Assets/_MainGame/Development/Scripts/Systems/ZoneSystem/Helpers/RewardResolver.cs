using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static VertigoCase.Helpers.Extensions.ListExtensions;
using VertigoCase.Runtime;
using VertigoCase.Helpers.Extensions;

namespace VertigoCase.Systems.ZoneSystem
{
    public class RewardResolver
    {
        private readonly List<RewardDataSO> _rewardDataList;
        List<RewardDataSO> _processDataList = new();

        public RewardResolver(List<RewardDataSO> rewardDataList)
        {
            _rewardDataList = rewardDataList;
        }
        List<RewardDataSO> CreateRewards(ZoneType zoneType, int count = 8)
        {
            count = Mathf.Max(3, count);
            _processDataList = new();
            for (int i = 0; i < count - 1; i++)
                _processDataList.Add(CreateReward(ZoneType.Normal));

            _processDataList.Add(CreateSpecialReward(zoneType));

            if (_processDataList.Count != count)//Ne olur ne olmaz kontrolu
                while (_processDataList.Count < count)
                    _processDataList.Add(CreateReward(ZoneType.Normal));

            return _processDataList.Shuffled();
        }
        public List<RewardedItemInfo> CreateRewardedItemList(float multiplier, ZoneType zoneType)
        {
            var rewards = CreateRewards(zoneType);
            List<RewardedItemInfo> result = new();

            for (int i = 0; i < rewards.Count; i++)
            {
                if (rewards[i].zoneType == ZoneType.Normal)
                    result.Add(new RewardedItemInfo(rewards[i], rewards[i].CalculateRewardAmount(multiplier)));
                else
                    result.Add(new RewardedItemInfo(rewards[i], 1));
            }

            return result;
        }

        RewardDataSO CreateReward(ZoneType zoneType)
        {
            RewardDataSO selectedReward = null;

            int maxTry = _rewardDataList.Count;

            for (int i = 0; i < maxTry; i++)
            {
                RewardDataSO candidate = _rewardDataList[Random.Range(0, _rewardDataList.Count)];

                if ((candidate.zoneType == zoneType && !_processDataList.Contains(candidate)) && candidate.rewardType != RewardType.Deadly)
                {
                    selectedReward = candidate;
                    break;
                }
            }

            if (selectedReward == null)
            {
                for (int i = 0; i < maxTry; i++)
                {
                    RewardDataSO candidate = _rewardDataList[Random.Range(0, _rewardDataList.Count)];

                    if (candidate.zoneType == ZoneType.Normal && !_processDataList.Contains(candidate) && candidate.rewardType != RewardType.Deadly)
                    {
                        selectedReward = candidate;
                        break;
                    }
                }
            }

            return selectedReward;
        }
        RewardDataSO CreateBombReward()
        {
            return _rewardDataList.Find(x => x.rewardType == RewardType.Deadly);
        }
        RewardDataSO CreateSpecialReward(ZoneType zoneType)
        {
            switch (zoneType)
            {
                case ZoneType.Normal:
                    return CreateBombReward();
                case ZoneType.Super:
                    return CreateReward(ZoneType.Super);
                default:
                    return CreateReward(ZoneType.Normal);
            }
        }


    }
}
