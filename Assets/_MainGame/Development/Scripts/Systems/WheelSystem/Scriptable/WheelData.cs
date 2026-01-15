using UnityEngine;
namespace VertigoCase.Data.Enums
{
    [CreateAssetMenu(fileName = "wheel_data",menuName = "Vertigo/WheelGame/WheelSystem/Wheel Data")]
    public class WheelData : ScriptableObject
    {
        public WheelType wheelType;

        public Sprite wheelImage;
        public Sprite indicatorImage;
    }
}
