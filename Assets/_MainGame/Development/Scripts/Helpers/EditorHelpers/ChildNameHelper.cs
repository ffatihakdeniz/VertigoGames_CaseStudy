using UnityEngine;
namespace VertigoCase.Helpers.EditorHelpers
{
    public class ChildNameHelper : MonoBehaviour
    {
        public int StartIndexName = 0;
        public string ChildNamePrefix = "Child_";
        [ContextMenu("Set Child Names")]
        void SetChildNames()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.name = ChildNamePrefix + (StartIndexName + i).ToString();
            }
        }
    }
}
