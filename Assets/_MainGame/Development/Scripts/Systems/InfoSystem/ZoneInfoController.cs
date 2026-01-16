using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VertigoCase.Systems.ZoneSystem;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using static VertigoCase.Helpers.Extensions.GeneralExtensions;


namespace VertigoCase.Runtime
{
    public class ZoneInfoController : MonoBehaviour, IGameInitializer
    {
        [SerializeField] Image superZoneInfoSprite;
        [SerializeField] TextMeshProUGUI superZoneInfoText_value;
        [SerializeField] TextMeshProUGUI safeZoneInfoText_value;
        [SerializeField] RectTransform superZoneInfoPanelRect;
        [SerializeField] RectTransform safeZoneInfoPanelRect;
        int requiredSuperZoneLevel => ZoneManager.Instance.GetSuperZoneNextRewardByInterval()?.level ?? -1;
        int requiredSafeZoneLevel => ZoneManager.Instance.GetSafeZoneNextRewardByInterval();
        int currentSuperZoneLevel = 0;
        int currentSafeZoneLevel = 0;

        public void Initialize()
        {
        }
        void Start()//Test
        {
            OnChangedLevelHandler();

        }
        void OnChangedLevelHandler()
        {
            if (requiredSuperZoneLevel != currentSuperZoneLevel)
            {
                var reward = ZoneManager.Instance.GetSuperZoneNextRewardByInterval();
                if (reward == null)
                    throw new System.Exception("Super Zone SO eklenmemis");
                currentSuperZoneLevel = requiredSuperZoneLevel;
                SuperZoneInfoPanelAnimation(reward);
            }
            if (requiredSafeZoneLevel != currentSafeZoneLevel)
            {
                print("Safe Zone Level Changed");
                currentSafeZoneLevel = requiredSafeZoneLevel;
                SafeZoneInfoPanelAnimation(currentSafeZoneLevel);
            }
        }
        async void SuperZoneInfoPanelAnimation(ZoneInfoSuperReward infoReward)
        {
            superZoneInfoSprite.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
            await UniTask.Delay(500);
            superZoneInfoSprite.sprite = infoReward.reward.icon;
            superZoneInfoSprite.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InExpo);
            await UniTask.Delay(500);
            superZoneInfoText_value.DOTypeText(infoReward.level, .5f);
            await UniTask.Delay(600);
            superZoneInfoPanelRect.DOShakeScale(0.25f, 0.15f, 10, 90, false);
        }
        async void SafeZoneInfoPanelAnimation(int level)
        {
            safeZoneInfoText_value.DOTypeText(level, 0.5f);
            await UniTask.Delay(600);
            safeZoneInfoPanelRect.DOShakeScale(0.25f, 0.15f, 10, 90, false);
        }

    }
}
