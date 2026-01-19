using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VertigoCase.Runtime;
using static VertigoCase.Helpers.Extensions.GeneralExtensions;
using static VertigoCase.Helpers.Extensions.ImageExtensions;
using DG.Tweening;

namespace VertigoCase.Systems.InventorySystem
{
    public class InventoryItemController : MonoBehaviour, IGameInitializer
    {
        [SerializeField] Image imageItem;
        [SerializeField] TextMeshProUGUI textItemRewardCount;
        internal RewardedItemInfo rewardedItemInfo;
        void IGameInitializer.Initialize() => transform.SetChildrenSetActive(false);

        public void InitItem(RewardedItemInfo rewardInitInfo, float waitUntil = 0f)
        {
            rewardedItemInfo = rewardInitInfo;
            imageItem.sprite = rewardInitInfo.RewardIcon;
            textItemRewardCount.text = 0.ToString();
            var sizeIcon = rewardedItemInfo.CalculateRectUIIconSize(imageItem.FitToFrame(imageItem.transform.parent.GetComponent<RectTransform>() as RectTransform));
            imageItem.rectTransform.sizeDelta = sizeIcon;
            if (rewardedItemInfo.RewardType == VertigoCase.Systems.ZoneSystem.RewardType.Chest)
                imageItem.transform.localRotation = Quaternion.Euler(0f, 35f, 0);
            DOVirtual.DelayedCall(waitUntil, () => SetValuesUseAnimation());//Test todo
            Canvas.ForceUpdateCanvases();
        }
        void SetValuesUseAnimation()
        {
            transform.SetChildrenSetActive(true);
            textItemRewardCount.DOTypeTextScalePulse(rewardedItemInfo.RewardAmount, 1.5f);
            imageItem.transform.DOPunchScale(new Vector3(.4f, .4f, 1), 0.3f, 3, 1);
        }
        public void SetItem(RewardedItemInfo rewardInfo, float waitUntil = 0f)
        {
            rewardedItemInfo.RewardAmount += rewardInfo.RewardAmount;
            DOVirtual.DelayedCall(waitUntil + .5f, () => SetValuesUseAnimation());//Test todo
                                                                                  //SetValuesUseAnimation();
        }

    }
}