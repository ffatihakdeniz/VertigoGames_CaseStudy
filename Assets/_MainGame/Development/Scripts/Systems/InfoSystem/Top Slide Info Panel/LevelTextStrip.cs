using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using static VertigoCase.Helpers.Extensions.GeneralExtensions;

namespace VertigoCase.Systems.InfoSystem
{
    public sealed class LevelTextStrip
    {
        private readonly Transform _root;
        private readonly int _middleChildIndex;
        private readonly int _childSpacing;
        private readonly int _textMaxSize;
        private readonly Ease _tweenEase;
        private readonly float _tweenDuration;
        private readonly ZoneColorResolver _colorResolver;

        private int _lastTextLevelIndex = -1;
        private int _currentMiddleChildIndex = 0;
        private bool _isFirstSlide = true;

        public int CurrentMiddleChildIndex => _currentMiddleChildIndex;
        public bool IsFirstSlide => _isFirstSlide;

        public LevelTextStrip(
            Transform rootLevelTexts,
            int middleChildIndex,
            int childSpacing,
            int textMaxSize,
            Ease tweenEase,
            float tweenDuration,
            ZoneColorResolver colorResolver)
        {
            _root = rootLevelTexts;
            _middleChildIndex = middleChildIndex;
            _childSpacing = childSpacing;
            _textMaxSize = textMaxSize;
            _tweenEase = tweenEase;
            _tweenDuration = tweenDuration;
            _colorResolver = colorResolver;
        }

        public void Initialize(int startedLevel)
        {
            SetChildLevelsText(startedLevel);
            SetColorByZoneTypeToLevel();
        }

        public async UniTask SlideNextAsync()
        {
            if (_currentMiddleChildIndex >= _middleChildIndex)
            {
                SetLastLevelTextItem();
                _root.GetChild(0).localPosition = _root.GetChild(_root.childCount - 1).localPosition;
                _root.GetChild(0).SetSiblingIndex(_root.childCount - 1);
                await UniTask.Delay(100);
            }

            TweenSlideAllLevelPoints();
        }

        private void SetChildLevelsText(int startedLevel)
        {
            for (int i = 0; i < _root.childCount; i++)
            {
                _lastTextLevelIndex = startedLevel + i;
                _root.GetChild(i).GetComponent<TextMeshProUGUI>().SetText(_lastTextLevelIndex.ToString());
            }
        }

        private void SetColorByZoneTypeToLevel()
        {
            if (!int.TryParse(_root.GetChild(0).GetComponent<TextMeshProUGUI>().text, out int firstLevel))
                throw new System.Exception("Level texti inte çevrilirken hata oluştur. Kontrol Et!!");

            for (int i = 0; i < _root.childCount; i++)
            {
                int level = firstLevel++;
                SetLevelTextColorByZoneType(i, level);
            }
        }

        private void SetLastLevelTextItem()
        {
            _root.GetChild(_root.childCount - 1).GetComponent<TextMeshProUGUI>()
                .SetText(_lastTextLevelIndex.ToString());

            SetLevelTextColorByZoneType(_root.childCount - 1, _lastTextLevelIndex);
            _lastTextLevelIndex++;
        }

        private void SetLevelTextColorByZoneType(int siblingIndex, int level)
        {
            _root.GetChild(siblingIndex).GetComponent<TextMeshProUGUI>().color =
                _colorResolver.GetLevelColor(level);
        }

        private void TweenSlideAllLevelPoints()
        {
            if (_isFirstSlide)
                _isFirstSlide = false;
            else
                _currentMiddleChildIndex = Mathf.Min(_currentMiddleChildIndex + 1, _middleChildIndex);

            var middleChild = _root.GetChild(_currentMiddleChildIndex);
            middleChild.DoMaxSize(_textMaxSize);
            middleChild.localPosition = Vector3.zero;

            int counter = 0;
            for (int i = _currentMiddleChildIndex; i >= 0; i--)
            {
                var child = _root.GetChild(i);
                child.DoMaxSize(_textMaxSize - (counter * 2));
                child.DOLocalMove(
                        new Vector3(middleChild.localPosition.x - _childSpacing * counter, child.localPosition.y, child.localPosition.z),
                        _tweenDuration)
                    .SetEase(_tweenEase);
                counter++;
            }

            counter = 0;
            for (int i = _currentMiddleChildIndex; i < _root.childCount; i++)
            {
                var child = _root.GetChild(i);
                child.DoMaxSize(_textMaxSize - (counter * 2));
                child.DOLocalMove(
                        new Vector3(middleChild.localPosition.x + _childSpacing * counter, child.localPosition.y, child.localPosition.z),
                        _tweenDuration)
                    .SetEase(_tweenEase);
                counter++;
            }
        }
    }
}
