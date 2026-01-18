using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using static VertigoCase.Helpers.Extensions.MathExtensions;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal.Internal;

namespace VertigoCase.Systems.WheelSystem
{
    /// <summary>
    /// Non-Mono: wheel transformunu dondurr, UniTask ile sonucu doner.
    /// </summary>
    public sealed class WheelSpinnerService
    {
        private readonly SpinSettingsDataSO _settings;
        private Tween _tween;
        public bool IsSpinning => _tween != null && _tween.IsActive();
        private int sliceCount = 0;
        private Transform wheel;


        public WheelSpinnerService(SpinSettingsDataSO settings, Transform wheel)//ctor
        {
            _settings = settings;
            sliceCount = settings.sliceCount;
            this.wheel = wheel;
        }

        public void Kill()
        {
            if (_tween != null && _tween.IsActive())
                _tween.Kill();
            _tween = null;
        }

        /// <summary>
        /// Rastgele Index secer ve oraya dondurup kazanan indexi dondurur
        /// </summary>
        public UniTask<int> SpinRandomAsync()
        {
            int target = UnityEngine.Random.Range(0, sliceCount);
            return SpinToIndexAsync(target);
        }

        /// <summary>
        /// Belilerlenen indexe spin atar. Ozel durumlarda kullanabiliriz. mesela her 6 seviyede 1 bomba gelsin gibi..
        /// indicatorAngle = 90 => wheel.z = 90 iken index 0 pointerda kabul edilir.
        /// </summary>
        public UniTask<int> SpinToIndexAsync(int targetIndex)
        {
            if (wheel == null) return UniTask.FromResult(-1);
            if (_settings == null) return UniTask.FromResult(-1);
            if (sliceCount <= 0) return UniTask.FromResult(-1);

            targetIndex = Mathf.Clamp(targetIndex, 0, sliceCount - 1);

            Kill();

            float sliceAngle = 360f / sliceCount;

            float targetCenter = (targetIndex * sliceAngle) + (sliceAngle * 0.5f);

            float desiredZ = _settings.indicatorAngle - targetCenter + _settings.indicatorOffset;

            float currentZ = NormalizeAngle(wheel.eulerAngles.z);
            float desiredNorm = NormalizeAngle(desiredZ);

            float deltaToTargetCW = NormalizeAngle(currentZ - desiredNorm);

            int rotations = _settings.GetRandomRotations();
            float totalDelta = rotations * 360f + deltaToTargetCW;

            float finalZ = currentZ - totalDelta;

            float dur = Mathf.Max(0.01f, _settings.spinDuration);

            var tcs = new UniTaskCompletionSource<int>();
            // finalZ = (targetIndex * sliceAngle) * _settings.minRotations;

            _tween = wheel
                .DORotate(new Vector3(0f, 0f, finalZ), _settings.spinDuration, _settings.rotateMode)
                .SetEase(_settings.ease)
                .OnComplete(() =>
                {
                    _tween = null;
                    float axisZ = Mathf.RoundToInt(wheel.eulerAngles.z);
                    if (axisZ < 0) axisZ += 360f;
                    int retValue = Mathf.RoundToInt(axisZ / sliceAngle);
                    Debug.Log($"retValue: {retValue} | axisZ: {axisZ} | sliceAngle: {sliceAngle} | targetIndex: {targetIndex}");

                    tcs.TrySetResult(retValue);
                })
                .OnKill(() =>
                {
                    if (!tcs.Task.Status.IsCompleted())
                        tcs.TrySetResult(-1);
                });

            return tcs.Task;
        }


    }
}
