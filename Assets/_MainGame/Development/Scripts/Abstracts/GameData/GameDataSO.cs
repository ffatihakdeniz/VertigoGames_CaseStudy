using UnityEngine;

namespace VertigoCase.Runtime
{
    [CreateAssetMenu(fileName = "GameData", menuName = "VertigoCase/Game Data")]
    public class GameDataSO : ScriptableObject
    {
        [Header("Settings")]
        [Tooltip("-1 Sonsuz Seviye")]
        public int maxLevel = -1;
        public float zoneMultiplierIncreaseRate = .6f;
        public int startedLevel = 1;


        [Header("Progression")]
        public int currentLevel = 0;
        public int currentZoneIntervalCount = 0;


        [Header("Gameplay")]
        public bool isFirstLaunch;
    }
}
