using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WheelGame.Data.Enums;

namespace WheelGame.Controllers
{
    public class WheelController : MonoBehaviour,IAutoBindable
    {
        [SerializeField] private List<WheelData> wheelDataArray=new();
        private Image wheelImage;
        private Image indicatorImage;
        private RectTransform wheelRootRect;

        internal WheelType _currentType;


        private void Awake()
        {
            SetWheelType(WheelType.Bronze);
        }

        public void SetWheelType(WheelType targetType)
        {
            if (_currentType == targetType)
                return;

            var data = wheelDataArray.Find(d => d.wheelType == targetType);
            if (data == null)
            {
                Debug.LogError($"WheelData not found for type: {targetType}");
                return;
            }

            _currentType = targetType;
            wheelImage.sprite = data.wheelImage;
            indicatorImage.sprite = data.indicatorImage;

            wheelRootRect.DOShakeScale(0.5f, 0.15f, 10, 90, false);
            indicatorImage.transform.DOShakeScale(0.5f, 0.15f, 10, 90, false);
        }

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

            if (wheelDataArray == null || wheelDataArray.Count == 0)
                Debug.LogWarning($"{nameof(WheelController)} has no WheelData assigned.", this);
        }
        [ContextMenu("Set Bronze Wheel")] private void SetBronze() => SetWheelType(WheelType.Bronze);
        [ContextMenu("Set Silver Wheel")] private void SetSilver() => SetWheelType(WheelType.Silver);
        [ContextMenu("Set Golden Wheel")] private void SetGolden() => SetWheelType(WheelType.Golden);
    #endif
    }
}
