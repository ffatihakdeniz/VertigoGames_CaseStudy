using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VertigoCase.Runtime;
using VertigoCase.Systems.ZoneSystem;

namespace VertigoCase.Systems.InventorySystem
{
    public class InventoryController : MonoBehaviour, IGameInitializer
    {
        [SerializeField] GameObject rewardedItemPrefab;
        [SerializeField] Transform rewardedItemRoot;
        private List<InventoryItemController> rewardedItem = new List<InventoryItemController>();
        public List<RewardDataSO> rewardList;
        List<RewardedItemInfo> rewardItemInfoListTest = new();
        int startSiblingIndex = 0;
        public void Initialize() => startSiblingIndex = transform.GetSiblingIndex();


        public Transform ProcessItem(RewardedItemInfo reward) //TODOfix
        {
            foreach (var item in rewardedItem)
            {
                if (item.rewardedItemInfo.RewardName == reward.RewardName)
                {
                    item.SetItem(reward, 2f);
                    return item.transform.GetChild(0);
                }
            }

            var newItem = Instantiate(rewardedItemPrefab, rewardedItemRoot)
                .GetComponent<InventoryItemController>();

            rewardedItem.Add(newItem);
            newItem.InitItem(reward, 2f);
            return newItem.transform.GetChild(0);
        }


        public void SetSiblingIndex(SetSiblingType type)
        {
            if (type == SetSiblingType.StartSiblingPosition)
                transform.SetSiblingIndex(startSiblingIndex);
            else
                transform.SetSiblingIndex(transform.parent.childCount - 1);
        }

    }





















    public enum SetSiblingType
    {
        StartSiblingPosition,
        TransformLastSibling
    }
}





/*
void Start()
        {
            for (int i = 0; i < rewardList.Count; i++)
            {
                rewardItemInfoListTest.Add(new RewardedItemInfo(rewardList[i], Random.Range(1, 100)));
            }
        }
*/






































/*
[ContextMenu("Test")]
    public void Test()
    {
        var reward = rewardItemInfoListTest[Random.Range(0, rewardItemInfoListTest.Count)];
        //ProcessItem(reward);Test
    }
public Transform ProcessItem(RewardedItemInfo reward)//test todo
{
    foreach (var item in rewardedItem)
    {
        if (item.rewardedItemInfo.RewardID == reward.RewardID)
        {
            item.SetItem(reward);
        }
    }

    var newItem = Instantiate(rewardedItemPrefab, rewardedItemRoot)
        .GetComponent<InventoryItemController>();

    rewardedItem.Add(newItem);
    newItem.InitItem(reward);
}*/