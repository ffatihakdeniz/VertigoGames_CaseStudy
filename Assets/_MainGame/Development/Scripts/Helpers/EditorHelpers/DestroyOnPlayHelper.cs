using UnityEngine;

namespace VertigoCase.Helpers.EditorHelpers
{
    public class DestroyOnPlayHelper : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject);
        }
    }
}
