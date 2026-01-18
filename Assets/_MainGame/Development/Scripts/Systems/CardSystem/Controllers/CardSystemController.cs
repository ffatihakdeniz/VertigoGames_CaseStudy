using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VertigoCase.Runtime;
using Zenject;
using DG.Tweening;
using static VertigoCase.Helpers.Extensions.ImageExtensions;
using VertigoCase.Systems.ZoneSystem;
using VertigoCase.Systems.WheelSystem;
using VertigoCase.Systems.InventorySystem;

namespace VertigoCase.Systems.CardSystem
{
    public class CardSystemController : MonoBehaviour, IGameInitializer, IAutoBindable
    {
        [Header("References")]
        [SerializeField] TextMeshProUGUI rewardCountText;
        [SerializeField] TextMeshProUGUI rewardNameText;
        [SerializeField] Image rewardIcon;
        [Inject] InventoryController inventoryController;

        [Header("Animation Settings")]
        [SerializeField] float animationDuration = 2f;
        [SerializeField] float cardDeactiveDuration = 4f;
        [SerializeField] Vector2 cardTargetPosition;
        Vector2 cardStartPosition;

        [Header("Test")]
        [SerializeField] RewardDataSO rewardSO;
        internal RewardedItemInfo rewardedItemInfo;
        Transform cardTransform;
        [Inject] RewardParticleController particleController;
        [Inject] WheelController wheelController;


        public void Initialize()
        {
            particleController = transform.GetComponentInChildren<RewardParticleController>(true);
            cardStartPosition = cardTargetPosition + Vector2.down * 1000f;
            cardTransform = transform.GetChild(0);
            cardTransform.localPosition = cardStartPosition;
            cardTransform.gameObject.SetActive(false);
        }
        void OnEnable()
        {
            EventBus.Subscribe<SpinEndedEvent>(StartCardAnimation);
        }
        void OnDisable()
        {
            EventBus.Unsubscribe<SpinEndedEvent>(StartCardAnimation);
        }

        [ContextMenu("Start Card Animation Test")]
        public void StartCardAnimation(SpinEndedEvent e)
        {
            cardTransform.localScale = Vector3.zero;
            //rewardedItemInfo = rewardItemInfoListTest[Random.Range(0, rewardItemInfoListTest.Count)];//Test
            rewardedItemInfo = e.RewardInfo;
            cardTransform.gameObject.SetActive(true);
            rewardCountText.text = "x " + rewardedItemInfo.RewardAmount.ToString();
            rewardNameText.text = rewardedItemInfo.RewardName;
            rewardIcon.sprite = rewardedItemInfo.RewardIcon;
            var sizeIcon = rewardedItemInfo.CalculateRectUIIconSize(rewardIcon.FitToFrame(rewardIcon.rectTransform.parent as RectTransform));
            rewardIcon.rectTransform.sizeDelta = sizeIcon;
            CardAnimationTask();
        }
        async void CardAnimationTask()
        {
            wheelController.GetComponent<CanvasGroup>().DOFade(0.15f, animationDuration / 2);

            cardTransform.DOLocalMove(cardTargetPosition, animationDuration / 2);
            cardTransform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutFlash);
            cardTransform.DOPunchRotation(Vector3.forward * 180, animationDuration / 2);

            await UniTask.Delay((int)(animationDuration * 1000));
            cardTransform.DOPunchScale(new Vector3(.2f, .2f, 1), 0.2f, 3, 1);
            await UniTask.Delay(1000);

            particleController.CreateParticle(rewardedItemInfo, inventoryController.ProcessItem(rewardedItemInfo));//test todo
            await UniTask.Delay((int)((cardDeactiveDuration - 2) * 1000));

            cardTransform.DOLocalMove(cardStartPosition, animationDuration / 2);
            cardTransform.DOScale(Vector3.zero, animationDuration / 2);


            wheelController.GetComponent<CanvasGroup>().DOFade(1, animationDuration);
            await UniTask.Delay(500);
            EventBus.Fire<PrepareNewLevelEvent>();
        }
        void CompletedAnimation()
        {
            cardTransform.localPosition = cardStartPosition;
            cardTransform.localScale = Vector3.zero;
        }

        //TESTT
        /*public List<RewardDataSO> rewardList;
        List<RewardedItemInfo> rewardItemInfoListTest = new();

        void Start()
        {
            for (int i = 0; i < rewardList.Count; i++)
            {
                rewardItemInfoListTest.Add(new RewardedItemInfo(rewardList[i], Random.Range(1, 100)));
            }
        }*/

    }


}