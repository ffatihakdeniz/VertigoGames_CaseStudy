using UnityEngine;
using DG.Tweening;

namespace FatihAkdeniz.Helpers.DotweenHelpers
{
    public class UIDotweenShaker : MonoBehaviour
    {
        public enum ShakeType
        {
            Punch,
            Shake
        }

        [Header("Shake Settings")]
        [SerializeField] private ShakeType shakeType = ShakeType.Punch;

        [SerializeField] private float duration = 0.2f;
        [SerializeField] private float delay = 0f;

        [SerializeField] private Vector3 scale = new Vector3(0.2f, 0.2f, 0f);
        [SerializeField] private int vibrato = 10;
        [SerializeField] private float randomness = 90f;

        private Vector3 _initialScale;
        private Tween _activeTween;

        private void Awake()
        {
            _initialScale = transform.localScale;
        }

        public void Play()
        {
            KillActiveTween();
            transform.localScale = _initialScale;

            switch (shakeType)
            {
                case ShakeType.Punch:
                    _activeTween = transform.DOPunchScale(
                            scale,
                            duration,
                            vibrato,
                            randomness)
                        .SetDelay(delay)
                        .SetEase(Ease.OutQuad);
                    break;

                case ShakeType.Shake:
                    _activeTween = transform.DOShakeScale(
                            duration,
                            scale,
                            vibrato,
                            randomness)
                        .SetDelay(delay)
                        .SetEase(Ease.OutQuad);
                    break;
            }
        }

        private void KillActiveTween()
        {
            if (_activeTween != null && _activeTween.IsActive())
            {
                _activeTween.Kill();
            }
        }

        private void OnDisable()
        {
            KillActiveTween();
            transform.localScale = _initialScale;
        }
    }
}
