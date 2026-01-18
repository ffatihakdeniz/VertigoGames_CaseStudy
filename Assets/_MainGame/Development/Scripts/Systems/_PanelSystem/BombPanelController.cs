using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VertigoCase.Runtime;
using VertigoCase.Systems.InventorySystem;
using Zenject;

namespace VertigoCase.Systems.PanelSystem
{
    public class BombPanelController : MonoBehaviour
    {
        private static readonly int BombPanelHash = Animator.StringToHash("bombPanelAnimation");
        [Inject] InventoryController inventoryController;

        Button _buttonRevive;
        Button _buttonRevive2;
        Button buttonGive;

        void OnEnable()
        {
            inventoryController.SetSiblingIndex(SetSiblingType.TransformLastSibling);

            _buttonRevive.onClick.AddListener(ButtonReviveHandler);
            _buttonRevive2.onClick.AddListener(ButtonReviveHandler);
            buttonGive.onClick.AddListener(ButtonGiveHandler);
            transform.GetComponent<Animator>().Play(BombPanelHash, 0, 0f);
        }
        void OnDisable()
        {
            inventoryController.SetSiblingIndex(SetSiblingType.StartSiblingPosition);

            _buttonRevive.onClick.RemoveListener(ButtonReviveHandler);
            _buttonRevive2.onClick.RemoveListener(ButtonReviveHandler);
            buttonGive.onClick.RemoveListener(ButtonGiveHandler);
        }

        void ButtonReviveHandler()
        {
            EventBus.Fire<PrepareNewLevelEvent>();
            gameObject.SetActive(false);
            //TODO Kazanc - kayip sistyemi eklenecek
        }

        void ButtonGiveHandler()
        {
            Application.Quit();
        }

        void OnValidate()
        {
            if (_buttonRevive == null)
                _buttonRevive = transform.Find("ui_bombpanel_button_revive__1").GetComponent<Button>();

            if (_buttonRevive2 == null)
                _buttonRevive2 = transform.Find("ui_bombpanel_button_revive__2").GetComponent<Button>();

            if (buttonGive == null)
                buttonGive = transform.Find("ui_bombpanel_button_give").GetComponent<Button>();
        }
    }

}
