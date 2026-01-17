using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VertigoCase.Runtime;
using VertigoCase.Systems.ZoneSystem;

public class RewardedListboxController : MonoBehaviour
{
    [SerializeField] GameObject rewardedItemPrefab;
    [SerializeField] Transform rewardedItemRoot;
    private List<RewardedItemListboxController> rewardedItem = new List<RewardedItemListboxController>();

    public List<RewardSO> rewardList;
    List<RewardedItemInfo> rewardItemInfoListTest = new();

    void Start()
    {
        for (int i = 0; i < rewardList.Count; i++)
        {
            rewardItemInfoListTest.Add(new RewardedItemInfo(rewardList[i], Random.Range(1, 100)));
        }
    }
    [ContextMenu("Test")]
    public void Test()
    {
        var reward = rewardItemInfoListTest[Random.Range(0, rewardItemInfoListTest.Count)];
        ProcessItem(reward);
    }
    void ProcessItem(RewardedItemInfo reward)
    {
        foreach (var item in rewardedItem)
        {
            if (item.rewardedItemInfo.RewardName == reward.RewardName)
            {
                item.SetItem(reward);
                return;
            }
        }

        var newItem = Instantiate(rewardedItemPrefab, rewardedItemRoot)
            .GetComponent<RewardedItemListboxController>();

        rewardedItem.Add(newItem);
        newItem.InitItem(reward);
    }
    public Transform TestProcessItem(RewardedItemInfo reward)//test todo
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
            .GetComponent<RewardedItemListboxController>();

        rewardedItem.Add(newItem);
        newItem.InitItem(reward, 2f);
        return newItem.transform.GetChild(0);
    }


}
