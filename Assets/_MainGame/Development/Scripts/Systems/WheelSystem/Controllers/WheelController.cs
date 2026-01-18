using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VertigoCase.Data.Enums;
using VertigoCase.Runtime;
using VertigoCase.Systems.ZoneSystem;
using Cysharp.Threading.Tasks;

namespace VertigoCase.Systems.WheelSystem
{
    public class WheelController : MonoBehaviour, IGameInitializer
    {
        [SerializeField] private SpinSettingsDataSO spinSettingsData;

        private Image wheelImage, indicatorImage;
        private RectTransform wheelRootRect;
        private Button spinButton;

        private ZoneType currentWheelVisualType;
        private WheelSpinnerService _wheelSpinnerService;

        public void Initialize()
        {
            SetWheelType(ZoneType.Normal);
            _wheelSpinnerService = new WheelSpinnerService(spinSettingsData, wheelImage.transform);
        }
        void OnEnable()
        {
            spinButton.onClick.AddListener(OnSpinButtonClick);
        }
        void OnDisable()
        {
            spinButton.onClick.RemoveListener(OnSpinButtonClick);
        }

        async void OnSpinButtonClick()
        {
            int index = await _wheelSpinnerService.SpinRandomAsync();
            if (index == -1) throw new System.Exception("Spin error.");
            print("Index: " + index);
        }
        void InitializeWheelNewLevel()
        {
            var zoneType = ZoneManager.Instance.CurrentZoneType;
            SetWheelType(zoneType);
        }





        public void SetWheelType(ZoneType targetType)
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
        [ContextMenu("Set Bronze Wheel")] private void SetBronze() => SetWheelType(ZoneType.Normal);
        [ContextMenu("Set Silver Wheel")] private void SetSilver() => SetWheelType(ZoneType.Safe);
        [ContextMenu("Set Golden Wheel")] private void SetGolden() => SetWheelType(ZoneType.Super);

#endif
    }
}
