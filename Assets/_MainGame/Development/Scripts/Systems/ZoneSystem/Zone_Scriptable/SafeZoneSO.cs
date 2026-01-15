using UnityEngine;

namespace VertigoCase.Systems.ZoneSystem
{
    [CreateAssetMenu(fileName = "SafeZone", menuName = "Vertigo/WheelGame/ZoneSystem/SafeZone")]
    public class SafeZoneSO : ZoneBaseSO
    {
        public override ZoneType ZoneType => ZoneType.Safe;
    }
}
