using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace VertigoCase.Helpers
{
    [RequireComponent(typeof(Button))]
    public sealed class RestartGame : MonoBehaviour
    {
        private Button button;

        private void OnValidate()
        {
            if (button == null)
                button = GetComponent<Button>();
        }

        private void OnEnable() => button.onClick.AddListener(Restart);

        private void OnDisable() => button.onClick.RemoveListener(Restart);

        private void Restart()
        {
            var activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex);
        }
    }

}
