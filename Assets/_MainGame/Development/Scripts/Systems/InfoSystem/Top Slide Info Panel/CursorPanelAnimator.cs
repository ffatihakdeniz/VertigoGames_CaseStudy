using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VertigoCase.Systems.InfoSystem
{
    public sealed class CursorPanelAnimator
    {
        private readonly Transform _cursorRoot;
        private Transform _left;
        private Transform _right;

        private readonly Ease _ease;
        private readonly float _duration;
        private readonly float _distanceX;
        private readonly ZoneColorResolver _colorResolver;

        public CursorPanelAnimator(Transform cursorPanelRoot, Ease tweenEaseCursor, float tweenDurationCursor, float distanceXCursor, ZoneColorResolver colorResolver)
        {
            _cursorRoot = cursorPanelRoot;
            _left = _cursorRoot.GetChild(0);
            _right = _cursorRoot.GetChild(1);

            _ease = tweenEaseCursor;
            _duration = tweenDurationCursor;
            _distanceX = distanceXCursor;
            _colorResolver = colorResolver;
        }

        public void SetInitialLeftText(int currentLevel)
        {
            _left.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(currentLevel.ToString());
        }

        public async UniTask PlayAsync(int currentLevel)
        {
            _left = _cursorRoot.GetChild(0);
            _right = _cursorRoot.GetChild(1);
            var _leftTMP = _left.GetChild(0).GetComponent<TextMeshProUGUI>();
            var _rightTMP = _right.GetChild(0).GetComponent<TextMeshProUGUI>();

            _leftTMP.SetText(currentLevel.ToString());
            _rightTMP.SetText((currentLevel + 1).ToString());
            _right.GetComponent<Image>().color = _colorResolver.GetLevelColor(currentLevel + 1);
            _rightTMP.color = _colorResolver.GetLevelTextColor(currentLevel + 1);

            _left.DOLocalMoveX(_distanceX, 0f).SetDelay(_duration + .1f);

            _left.DOLocalMoveX(-_distanceX, _duration).SetEase(_ease);
            _right.DOLocalMoveX(0, _duration).SetEase(_ease);

            _cursorRoot.GetChild(0).SetSiblingIndex(1);

            await UniTask.Delay((int)(_duration * 1000));
            _leftTMP.transform.DOPunchScale(new Vector3(0.35f, 0.35f, 0), 0.15f, 3, 1);
        }
    }
}
