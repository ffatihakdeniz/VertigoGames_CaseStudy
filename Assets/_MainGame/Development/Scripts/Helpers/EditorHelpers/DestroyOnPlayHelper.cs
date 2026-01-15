using UnityEngine;

namespace VertigoCase.Helpers.OtherHelpers
{
    public class DestroyOnPlayHelper : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject);
        }
    }
}
