using UnityEngine;

namespace VertigoCase.Systems.ZoneSystem
{
    [CreateAssetMenu(fileName = "NormalZone", menuName = "Vertigo/WheelGame/ZoneSystem/NormalZone")]
    public class NormalZoneSO : ZoneBaseSO
    {
        public override ZoneType zoneType => ZoneType.Normal;
    }

}
