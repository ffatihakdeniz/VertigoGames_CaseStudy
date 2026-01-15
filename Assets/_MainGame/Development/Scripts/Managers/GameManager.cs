using UnityEngine;
using Patterns.Singleton;

namespace VertigoCase.Runtime
{
    public class GameManager : MonoSingleton<GameManager>
    {
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
            }
        }
    }

}
