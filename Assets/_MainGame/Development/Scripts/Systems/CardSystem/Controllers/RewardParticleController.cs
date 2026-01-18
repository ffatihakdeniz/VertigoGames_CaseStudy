using UnityEngine;
using VertigoCase.Runtime;
using FatihAkdeniz.Helpers.DotweenHelpers;
using AssetKits.ParticleImage;

namespace VertigoCase.Systems.CardSystem
{
    public class RewardParticleController : MonoBehaviour, IAutoBindable
    {
        [Header("Reward Imnfo")]
        [SerializeField] RewardedItemInfo rewardedItemInfo;
        [SerializeField] GameObject rewardParticlePrefab;

        [Header("Particle Settings")]
        [SerializeField] Vector2Int MinMaxParticleCount = new Vector2Int(2, 20);

        Transform targetCanvasTransform;
        UIDotweenShaker itemShaker;
        ParticleImage particleReward;

        private void Awake()
        {
            targetCanvasTransform = transform.root;
            itemShaker = transform.parent.GetComponentInChildren<UIDotweenShaker>();
            print(targetCanvasTransform.name);
        }

        [ContextMenu("Test Particle")]
        public void CreateParticle(Transform attractorTransform = default)
        {
            if (rewardedItemInfo.RewardType == 0)
                return;
            int particleCount = Mathf.Clamp(rewardedItemInfo.RewardAmount, MinMaxParticleCount.x, MinMaxParticleCount.y);
            particleReward = Instantiate(rewardParticlePrefab, transform.position, Quaternion.identity, targetCanvasTransform).GetComponent<ParticleImage>();
            particleReward.gameObject.SetActive(true);
            particleReward.sprite = rewardedItemInfo.RewardIcon;
            particleReward.rateOverTime = particleCount;
            particleReward.attractorTarget = attractorTransform;
            particleReward.startSize = new SeparatedMinMaxCurve(particleReward.startSize.mainCurve.constant * rewardedItemInfo.ScaleMultiplierIcon, true, false);
            particleReward.Play();
            itemShaker.Play();
        }
        public void CreateParticle(RewardedItemInfo rewardInfo, Transform attractorTransform)
        {
            rewardedItemInfo = rewardInfo;
            CreateParticle(attractorTransform);
        }
    }
}
