using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VertigoCase.Runtime;
using static VertigoCase.Helpers.Extensions.GeneralExtensions;
using static VertigoCase.Helpers.Extensions.ImageExtensions;
using DG.Tweening;

public class InventoryItemController : MonoBehaviour
{
    [SerializeField] Image imageItem;
    [SerializeField] TextMeshProUGUI textItemRewardCount;
    internal RewardedItemInfo rewardedItemInfo;
    void Awake()
    {
        transform.SetChildrenSetActive(false);
    }

    public void InitItem(RewardedItemInfo rewardInitInfo, float waitUntil = 0f)
    {
        rewardedItemInfo = rewardInitInfo;
        imageItem.sprite = rewardInitInfo.RewardIcon;
        textItemRewardCount.text = 0.ToString();
        // var sizeIcon = imageItem.FitToFrame(imageItem.transform.parent.GetComponent<RectTransform>() as RectTransform);
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
        imageItem.transform.DOPunchScale(new Vector3(.2f, .2f, 1), 0.2f, 3, 1);
    }
    public void SetItem(RewardedItemInfo rewardInfo, float waitUntil = 0f)
    {
        rewardedItemInfo.RewardAmount += rewardInfo.RewardAmount;
        DOVirtual.DelayedCall(waitUntil, () => SetValuesUseAnimation());//Test todo
        //SetValuesUseAnimation();
    }
}
