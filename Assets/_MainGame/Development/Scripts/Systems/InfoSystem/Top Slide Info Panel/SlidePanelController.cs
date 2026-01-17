using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VertigoCase.Systems.ZoneSystem;

namespace VertigoCase.Systems.InfoSystem
{
    public class SlidePanelController : MonoBehaviour, IAutoBindable
    {
        [Header("Layout Settings")]
        public int middleChildIndex = 8;
        public int childSpacing = 85;
        public int textMaxSize = 45;

        [Header("Slide Tween Settings")]
        public Ease tweenEase = Ease.OutExpo;
        public float tweenDuration = 0.5f;

        [Header("Cursor Tween Settings")]
        public Ease tweenEaseCursor = Ease.OutExpo;
        public float tweenDurationCursor = 0.5f;
        public float distanceXCursor = 130f;

        [Header("Referances")]
        [SerializeField] private Transform rootLevelTexts;
        [SerializeField] private Transform CursorPanelRoot;

        private int startedLevel => ZoneManager.Instance.StartedLevel;
        private int currentLevel = 0; // TODO test icin silinecek

        private ZoneColorResolver _colorResolver;
        private LevelTextStrip _levelStrip;
        private CursorPanelAnimator _cursorAnimator;

        public void Start()
        {
            _colorResolver = new ZoneColorResolver();
            _levelStrip = new LevelTextStrip(rootLevelTexts, middleChildIndex, childSpacing, textMaxSize, tweenEase, tweenDuration, _colorResolver);
            _cursorAnimator = new CursorPanelAnimator(CursorPanelRoot, tweenEaseCursor, tweenDurationCursor, distanceXCursor, _colorResolver);

            _levelStrip.Initialize(startedLevel);
            _cursorAnimator.SetInitialLeftText(currentLevel);

            GeneralSlideAnimation();
        }

        public async void GeneralSlideAnimation()
        {
            await _levelStrip.SlideNextAsync();

            if (!_levelStrip.IsFirstSlide)
                await _cursorAnimator.PlayAsync(currentLevel);

            currentLevel++;
        }
    }
}
