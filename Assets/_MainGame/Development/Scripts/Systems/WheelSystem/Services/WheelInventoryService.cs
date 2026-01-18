using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using static VertigoCase.Helpers.Extensions.ImageExtensions;
using System.Collections.Generic;
using VertigoCase.Runtime;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace VertigoCase.Systems.WheelSystem
{
    /// <summary>
    /// Non-Mono: wheel icerisine eklenen rewardableItemlerdan sorumlu servis.
    /// </summary>
    public sealed class WheelInventoryService
    {
        private readonly Transform wheelParentPivot;
        public WheelInventoryService(Transform pivot)
        {
            this.wheelParentPivot = pivot;
        }
        public async UniTask AddRewardableItems(List<RewardedItemInfo> rewardableItems)
        {
            for (int i = 0; i < wheelParentPivot.childCount; i++)
            {
                wheelParentPivot.GetChild(i).DOScale(Vector3.one, .5f).SetEase(Ease.OutBack);
                var imageItem = wheelParentPivot.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>();
                imageItem.enabled = true;
                imageItem.sprite = rewardableItems[i].RewardIcon;
                var sizeDelta = rewardableItems[i].CalculateRectUIIconSize(imageItem.FitToFrame(imageItem.transform.parent.GetComponent<RectTransform>() as RectTransform));
                imageItem.rectTransform.sizeDelta = sizeDelta;
                wheelParentPivot.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().SetText("x " + rewardableItems[i].RewardAmount.ToString());
                if (rewardableItems[i].RewardType == VertigoCase.Systems.ZoneSystem.RewardType.Deadly)
                    wheelParentPivot.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(string.Empty);
                await UniTask.Delay(200);
            }
        }
        public async UniTask DeleteRewardableItems()
        {
            for (int i = 0; i < wheelParentPivot.childCount; i++)
            {
                wheelParentPivot.GetChild(i).DOScale(Vector3.zero, .3f);
                wheelParentPivot.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
                await UniTask.Delay(100);
            }
            for (int i = 0; i < wheelParentPivot.childCount; i++)
            {
                wheelParentPivot.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
                wheelParentPivot.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().SetText("0");
            }
        }



    }
}
