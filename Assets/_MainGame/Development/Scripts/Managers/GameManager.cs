using UnityEngine;
using Patterns.Singleton;

namespace VertigoCase.Runtime
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private GameDataSO gameData;
        protected override void Awake()
        {
            Initializer();
        }
        void Initializer()
        {
            var initializers = FindObjectsOfType<MonoBehaviour>(true);
            foreach (var obj in initializers)
            {
                if (obj is IGameInitializer initializer)
                    initializer.Initialize();
                else if (obj is IGameDataConsumer dataConsumer)
                    dataConsumer.Initialize(gameData);
            }
        }
    }

}
