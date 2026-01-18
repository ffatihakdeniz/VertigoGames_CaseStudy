using UnityEngine;
using DG.Tweening;
using VertigoCase.Data.Enums;
using System.Collections.Generic;

namespace VertigoCase.Systems.WheelSystem
{
    [CreateAssetMenu(
        fileName = "SpinSettingsData",
        menuName = "Vertigo/WheelGame/Spin Settings")]
    public class SpinSettingsDataSO : ScriptableObject
    {
        [Header("DATA")]
        [Space]
        [Header("Wheel Visual Data")]
        public List<WheelData> wheelVisualDataArray;
        [Space(15)]

        [Header("SETTINGS")]
        [Space]
        [Header("Timing")]
        public float spinDuration = 2.5f;

        [Header("Rotation")]
        public int minRotations = 4;
        public int maxRotations = 8;
        public RotateMode rotateMode = RotateMode.FastBeyond360;

        [Header("Easing")]
        public Ease ease = Ease.OutQuart;

        [Header("Indicator")]
        public float indicatorAngle = 90f;
        public float indicatorOffset = 22.5f;

        [Header("Slice Count")]
        public int sliceCount = 8;

        public int GetRandomRotations() => Random.Range(minRotations, maxRotations + 1);


    }
}
