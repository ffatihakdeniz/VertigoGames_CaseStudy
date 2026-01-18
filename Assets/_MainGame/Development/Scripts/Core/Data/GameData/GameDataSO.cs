using UnityEngine;

namespace VertigoCase.Runtime.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Vertigo/WheelGame/GameData")]
    public class GameDataSO : ScriptableObject
    {
        [Header("Settings")]
        [Tooltip("-1 Sonsuz Seviye")]
        public int maxLevel = -1;
        public float zoneMultiplierIncreaseRate = .2f;
        public int startedLevel = 1;


        [Header("Progression")]
        public int currentLevel = 1;
        public int currentZoneIntervalCount = 0;


        [Header("Gameplay")]
        public bool isFirstLaunch;
    }
}
