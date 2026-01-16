using DG.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;
using static VertigoCase.Helpers.Extensions.GeneralExtensions;

namespace VertigoCase.Helpers.EditorHelpers
{
    public class LayoutSortingHelper : MonoBehaviour
    {
        public int middleChildIndex = 6;
        public int childSpaceng = 85;
        public int textMaxSize = 45;

        public Ease tweenEase = Ease.OutExpo;
        public float tweenDuration = 0.5f;

        [ContextMenu("Set Child Layout Sorting")]
        void SetChildLayoutSorting()
        {
            var _middleChild = transform.GetChild(middleChildIndex);
            _middleChild.localPosition = Vector3.zero;
            _middleChild.DoMaxSize(textMaxSize);

            int _counter = 0;
            for (int i = middleChildIndex; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                child.DoMaxSize(textMaxSize - (_counter * 2));
                child.localPosition = new Vector3(_middleChild.localPosition.x - childSpaceng * _counter, child.localPosition.y, child.localPosition.z);
                _counter++;
            }

            _counter = 0;
            for (int i = middleChildIndex; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.DoMaxSize(textMaxSize - (_counter * 2));
                child.localPosition = new Vector3(_middleChild.localPosition.x + childSpaceng * _counter, child.localPosition.y, child.localPosition.z);
                _counter++;
            }
        }
        [ContextMenu("Set Child Layout Sorting and Animation use Dotween")]
        public void TestTweenAnimation()
        {
            var _middleChild = transform.GetChild(middleChildIndex);
            _middleChild.DoMaxSize(textMaxSize);
            _middleChild.localPosition = Vector3.zero;


            int _counter = 0;
            for (int i = middleChildIndex; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                child.DoMaxSize(textMaxSize - (_counter * 2));
                child.DOLocalMove(new Vector3(_middleChild.localPosition.x - childSpaceng * _counter, child.localPosition.y, child.localPosition.z), tweenDuration).SetEase(tweenEase);
                _counter++;
            }
            _counter = 0;
            for (int i = middleChildIndex; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.DoMaxSize(textMaxSize - (_counter * 2));
                child.DOLocalMove(new Vector3(_middleChild.localPosition.x + childSpaceng * _counter, child.localPosition.y, child.localPosition.z), tweenDuration).SetEase(tweenEase);
                _counter++;
            }
        }
        [ContextMenu("Test General Slide Animation")]
        public async void TestGeneralSlideAnimation()
        {
            transform.GetChild(0).localPosition = transform.GetChild(transform.childCount - 1).localPosition;
            transform.GetChild(0).SetSiblingIndex(transform.childCount - 1);
            await UniTask.Delay(100);
            TestTweenAnimation();
            print("Slide Animation Test Completed");
        }


    }
}
