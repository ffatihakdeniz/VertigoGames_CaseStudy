using UnityEngine;
using VertigoCase.Systems.ZoneSystem;
namespace VertigoCase.Data.Enums
{
    [CreateAssetMenu(fileName = "wheel_data", menuName = "Vertigo/WheelGame/WheelSystem/Wheel Data")]
    public class WheelData : ScriptableObject
    {
        public WheelType wheelType;
        public ZoneType zoneType;

        public Sprite wheelImage;
        public Sprite indicatorImage;
    }
}
