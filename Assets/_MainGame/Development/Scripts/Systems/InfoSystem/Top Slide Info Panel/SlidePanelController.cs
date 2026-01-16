using DG.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using VertigoCase.Systems.ZoneSystem;
using static VertigoCase.Helpers.Extensions.GeneralExtensions;
using UnityEngine.UI;

namespace VertigoCase.Systems.InfoSystem
{
    public class SlidePanelController : MonoBehaviour
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
        [SerializeField] Transform rootLevelTexts;
        [SerializeField] Transform CursorPanelRoot;
        private Transform cursorPanelLeft => CursorPanelRoot.GetChild(0);
        private Transform cursorPanelRight => CursorPanelRoot.GetChild(1);


        int startedLevel => ZoneManager.Instance.StartedLevel;
        //int currentLevel => ZoneManager.Instance.CurrentLevel;
        int currentLevel = 0;//TODO Test icin silinecek
        int nextLevel => currentLevel + 1;
        int lastLevel => Mathf.Max(currentLevel - 1, 0);

        int lastTextLevelIndex = -1;
        int currentMiddleChildIndex = 0;
        bool _isFirstSlide = true;


        public void Start()
        {
            SetChildLevelsText();
            SetColorByZoneTypeToLevel();
            GeneralSlideAnimation();
            cursorPanelLeft.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(currentLevel.ToString());
        }

        void SetChildLevelsText()
        {
            for (int i = 0; i < rootLevelTexts.childCount; i++)
            {
                lastTextLevelIndex = startedLevel + i;
                rootLevelTexts.GetChild(i).GetComponent<TextMeshProUGUI>().SetText(lastTextLevelIndex.ToString());
            }
        }

        void SetColorByZoneTypeToLevel()
        {
            if (!int.TryParse(rootLevelTexts.GetChild(0).GetComponent<TextMeshProUGUI>().text, out int firstLevel))
                throw new System.Exception("Level texti inte çevrilirken hata oluştur. Kontrol Et!!");

            int counterLevelIndex = firstLevel;

            for (int i = 0; i < rootLevelTexts.childCount; i++)
            {
                counterLevelIndex = firstLevel++;
                SetLevelTextColorByZoneType(i, counterLevelIndex);
            }
        }



        public void TweenAnimationSlideAllLevelPoint()
        {
            if (_isFirstSlide)
                _isFirstSlide = false;
            else
                currentMiddleChildIndex = Mathf.Min(currentMiddleChildIndex + 1, middleChildIndex);

            var _middleChild = rootLevelTexts.GetChild(currentMiddleChildIndex);
            _middleChild.DoMaxSize(textMaxSize);
            _middleChild.localPosition = Vector3.zero;

            int _counter = 0;
            for (int i = currentMiddleChildIndex; i >= 0; i--)
            {
                var child = rootLevelTexts.GetChild(i);
                child.DoMaxSize(textMaxSize - (_counter * 2));
                child.DOLocalMove(new Vector3(_middleChild.localPosition.x - childSpacing * _counter, child.localPosition.y, child.localPosition.z), tweenDuration).SetEase(tweenEase);
                _counter++;
            }
            _counter = 0;
            for (int i = currentMiddleChildIndex; i < rootLevelTexts.childCount; i++)
            {
                var child = rootLevelTexts.GetChild(i);
                child.DoMaxSize(textMaxSize - (_counter * 2));
                child.DOLocalMove(new Vector3(_middleChild.localPosition.x + childSpacing * _counter, child.localPosition.y, child.localPosition.z), tweenDuration).SetEase(tweenEase);
                _counter++;
            }
        }

        void SetLastLevelTextItem()
        {
            rootLevelTexts.GetChild(rootLevelTexts.childCount - 1).GetComponent<TextMeshProUGUI>().SetText(lastTextLevelIndex.ToString());
            SetLevelTextColorByZoneType(rootLevelTexts.childCount - 1, lastTextLevelIndex);
            lastTextLevelIndex++;
        }
        void SetLevelTextColorByZoneType(int siblingIndex, int counterLevelIndex)
        {
            rootLevelTexts.GetChild(siblingIndex).GetComponent<TextMeshProUGUI>().color = LevelColorByZoneType(counterLevelIndex);
        }
        Color LevelColorByZoneType(int level)
        {
            if (level % ZoneManager.Instance.IntervalLevelBySuperZone == 0)
                return ZoneManager.Instance.GetZoneSOByZoneType(ZoneType.Super).slidePanelLevelPointColor;
            else if (level % ZoneManager.Instance.IntervalLevelBySafeZone == 0)
                return ZoneManager.Instance.GetZoneSOByZoneType(ZoneType.Safe).slidePanelLevelPointColor;
            else
                return ZoneManager.Instance.GetZoneSOByZoneType(ZoneType.Normal).slidePanelLevelPointColor;
        }

        public async void GeneralSlideAnimation()
        {
            //print("_1: MiddleIndex: " + currentMiddleChildIndex + "--" + rootLevelTexts.GetChild(currentMiddleChildIndex).GetComponent<TextMeshProUGUI>().text);
            if (currentMiddleChildIndex >= middleChildIndex)
            {
                SetLastLevelTextItem();
                rootLevelTexts.GetChild(0).localPosition = rootLevelTexts.GetChild(rootLevelTexts.childCount - 1).localPosition;
                rootLevelTexts.GetChild(0).SetSiblingIndex(rootLevelTexts.childCount - 1);
                await UniTask.Delay(100);

            }
            TweenAnimationSlideAllLevelPoint();
            if (!_isFirstSlide)
                TweenAnimationCursorPanel();
            currentLevel++;
        }
        private async void TweenAnimationCursorPanel()
        {
            cursorPanelLeft.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(currentLevel.ToString());
            cursorPanelRight.GetChild(0).GetComponent<TextMeshProUGUI>().SetText((currentLevel + 1).ToString());
            cursorPanelRight.GetComponent<Image>().color = LevelColorByZoneType(currentLevel + 1);

            cursorPanelLeft.DOLocalMoveX(distanceXCursor, 0f).SetDelay(tweenDurationCursor + .1f);


            cursorPanelLeft.DOLocalMoveX(-distanceXCursor, tweenDurationCursor).SetEase(tweenEaseCursor);
            cursorPanelRight.DOLocalMoveX(0, tweenDurationCursor).SetEase(tweenEaseCursor);

            CursorPanelRoot.GetChild(0).SetSiblingIndex(1);

            await UniTask.Delay((int)(tweenDurationCursor * 1000));
            cursorPanelLeft.GetChild(0).DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.3f, 10, 1);

        }

    }
}
