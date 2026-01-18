using VertigoCase.Runtime.Data;

namespace VertigoCase.Runtime
{
    public readonly struct SpinStartedEvent { }//tmm

    public readonly struct SpinEndedEvent//tmm
    {
        public readonly RewardedItemInfo RewardInfo;
        public SpinEndedEvent(RewardedItemInfo rewardInfo) => RewardInfo = rewardInfo;
    }

    //public readonly struct CardParticleEffectStartedEvent { }

    public readonly struct PrepareNewLevelEvent { }//

    public readonly struct ChangedLevelEvent { }//tmm

    public readonly struct ExitButtonClickedEvent { }//

    public readonly struct NewGameStartedEvent { }//
}
