using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VertigoCase.Runtime;
using VertigoCase.Systems.ZoneSystem;
using VertigoCase.Systems.PanelSystem;
using Cysharp.Threading.Tasks;
using Zenject;

namespace VertigoCase.Systems.WheelSystem
{
    public class WheelController : MonoBehaviour, IGameInitializer
    {
        [SerializeField] private SpinSettingsDataSO spinSettingsData;
        [Inject] GameManager gameManager;

        private Image wheelImage, indicatorImage;
        private RectTransform wheelRootRect;
        private Button spinButton;

        private ZoneType currentWheelVisualType;
        private WheelSpinnerService _wheelSpinnerService;
        private WheelInventoryService _wheelInventoryService;
        List<RewardedItemInfo> currentRewards;

        public void Initialize()
        {
            ChangeWheelVisual(ZoneType.Normal);
            currentRewards = new List<RewardedItemInfo>();
            _wheelSpinnerService = new WheelSpinnerService(spinSettingsData, wheelImage.transform);
            _wheelInventoryService = new WheelInventoryService(wheelImage.transform);
            for (int i = 0; i < wheelImage.transform.childCount; i++)
                wheelImage.transform.GetChild(i).localScale = Vector3.zero;
        }
        void Start()
        {
            currentRewards = ZoneManager.Instance.GetRewardsByZoneType();
            _wheelInventoryService.AddRewardableItems(currentRewards).Forget();
        }
        void OnEnable()
        {
            spinButton.onClick.AddListener(OnSpinButtonClick);
            EventBus.Subscribe<ChangedLevelEvent>(OnChangedLevelHandler);
            EventBus.Subscribe<PrepareNewLevelEvent>(OnPrepareNewLevel);
        }
        void OnDisable()
        {
            spinButton.onClick.RemoveListener(OnSpinButtonClick);
            EventBus.Unsubscribe<ChangedLevelEvent>(OnChangedLevelHandler);
            EventBus.Unsubscribe<PrepareNewLevelEvent>(OnPrepareNewLevel);
        }
        void OnChangedLevelHandler() => SetupWheelForNewLevel();
        async void OnPrepareNewLevel()
        {
            ZoneManager.Instance.IncreaseLevel();
            await _wheelInventoryService.DeleteRewardableItems();
            SetButtonsInteractable(true);
            EventBus.Fire<ChangedLevelEvent>();
        }

        async void OnSpinButtonClick()
        {
            EventBus.Fire<SpinStartedEvent>();
            SetButtonsInteractable(false);
            int index = await _wheelSpinnerService.SpinRandomAsync();
            if (index == -1) throw new System.Exception("Spin error.");
            transform.DOShakeScale(0.3f, 0.15f, 10, 90, false);
            var reward = currentRewards[index];
            if (reward.RewardType == RewardType.Deadly)
                PanelPopUpManager.Instance.OpenBombPanel();
            else
                EventBus.Fire(new SpinEndedEvent(reward));
        }
        public async void SetupWheelForNewLevel()
        {
            var zoneType = ZoneManager.Instance.CurrentZoneType;
            ChangeWheelVisual(zoneType);
            wheelImage.transform.rotation = Quaternion.identity;
            currentRewards = ZoneManager.Instance.GetRewardsByZoneType();
            await _wheelInventoryService.AddRewardableItems(currentRewards);
        }

        public void ChangeWheelVisual(ZoneType targetType)
        {
            if (currentWheelVisualType == targetType)
                return;

            var data = spinSettingsData.wheelVisualDataArray.Find(d => d.zoneType == targetType);
            if (data == null)
            {
                Debug.LogError($"WheelData not found for type: {targetType}");
                return;
            }

            currentWheelVisualType = targetType;
            wheelImage.sprite = data.wheelImage;
            indicatorImage.sprite = data.indicatorImage;

            wheelRootRect.DOShakeScale(0.5f, 0.15f, 10, 90, false);
            indicatorImage.transform.DOShakeScale(0.2f, 0.15f, 10, 90, false).SetDelay(0.5f);
        }
        void SetButtonsInteractable(bool value)
        {
            spinButton.interactable = value;
            gameManager.exitButton.interactable = value;
        }
        [ContextMenu("Test Setup Wheel")] private void TestSetupWheel() => SetupWheelForNewLevel();
        [ContextMenu("Test Delete Rewards")] private void TestDeleteRewards() => _wheelInventoryService.DeleteRewardableItems().Forget();

        /// <summary>
        /// Burada Validate kullanmak istedim. pek tercihim degil ama casede istediginiz icin kullanabildigimi gostermek maksadiyla :)
        /// Validateyi genelde editorde bir isim oldugunda kisa sureli kullaniyorum. mesela childlarin ismini sirali ve duzenli yapmak icin.
        /// </summary>
#if UNITY_EDITOR
        static string UI_WHEEL_BASE_PATH = "ui_spin_base_image";
        static string UI_INDICATOR_PATH = "ui_wheel_indicator";
        static string UI_WHEEL_ROOT_PATH = "ui_wheel_spin_root";

        private void OnValidate()
        {
            if (wheelImage == null)
                wheelImage = GameObject.Find(UI_WHEEL_BASE_PATH)?.GetComponent<Image>();

            if (indicatorImage == null)
                indicatorImage = GameObject.Find(UI_INDICATOR_PATH)?.GetComponent<Image>();

            if (wheelRootRect == null)
                wheelRootRect = GameObject.Find(UI_WHEEL_ROOT_PATH)?.GetComponent<RectTransform>();

            if (spinButton == null)
                spinButton = transform.GetComponentInChildren<Button>();

            if (spinSettingsData == null)
                Debug.LogWarning("SpinSettingsData not found");
        }
        [ContextMenu("Set Bronze Wheel")] private void SetBronze() => ChangeWheelVisual(ZoneType.Normal);
        [ContextMenu("Set Silver Wheel")] private void SetSilver() => ChangeWheelVisual(ZoneType.Safe);
        [ContextMenu("Set Golden Wheel")] private void SetGolden() => ChangeWheelVisual(ZoneType.Super);

#endif
    }
}
