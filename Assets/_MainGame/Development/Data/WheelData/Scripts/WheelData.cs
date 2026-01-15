using UnityEngine;
namespace WheelGame.Data.Enums
{
    [CreateAssetMenu(fileName = "wheel_data",menuName = "Vertigo/WheelGame/Data/Wheel Data")]
    public class WheelData : ScriptableObject
    {
        public WheelType wheelType;

        public Sprite wheelImage;
        public Sprite indicatorImage;
    }
}
