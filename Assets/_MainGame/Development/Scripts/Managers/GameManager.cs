using UnityEngine;
using Patterns.Singleton;
using VertigoCase.Runtime.Data;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace VertigoCase.Runtime
{
    public class GameManager : MonoBehaviour, IAutoBindable
    {
        [SerializeField] private GameDataSO gameData;
        public Button exitButton;

        void Awake()
        {
            gameData.currentLevel = 1;
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
        async void Start()
        {
            exitButton.onClick.AddListener(() => Application.Quit());//zaman kalırsa reset sistemi icin ugrasicam//islevsiz kalmasin diye direkt quit attiriyorum :(
            await UniTask.Yield();
            EventBus.Fire<ChangedLevelEvent>();
        }
    }

}
