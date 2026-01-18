using UnityEngine;
using Patterns.Singleton;

namespace VertigoCase.Systems.PanelSystem
{
    public class PanelPopUpManager : MonoSingleton<PanelPopUpManager>
    {
        [SerializeField] GameObject BombPanel;
        public void OpenBombPanel()
        {
            BombPanel.SetActive(true);
        }

        /*[SerializeField] GameObject ExitPanel;
        public void OpenExitPanel()
        {
            //BombPanel.SetActive(true);
        }*/


    }

}
