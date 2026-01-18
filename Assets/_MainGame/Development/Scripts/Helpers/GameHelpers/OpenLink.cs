using UnityEngine;
using UnityEngine.UI;

namespace VertigoCase.Helpers
{
    [RequireComponent(typeof(Button))]
    public sealed class OpenLink : MonoBehaviour
    {
        private Button button;
        [SerializeField] private string url;

        private void OnValidate()
        {
            if (button == null)
                button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (button == null)
                button = GetComponent<Button>();

            button.onClick.AddListener(OpenUrl);
        }

        private void OnDisable()
        {
            if (button != null)
                button.onClick.RemoveListener(OpenUrl);
        }

        private void OpenUrl()
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogWarning($"{nameof(OpenLink)}: URL boş.");
                return;
            }

            Application.OpenURL(url);
        }
    }

}
