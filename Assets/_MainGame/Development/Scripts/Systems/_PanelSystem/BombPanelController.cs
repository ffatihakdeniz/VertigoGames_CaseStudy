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
        [Inject] InventoryController inventoryController;

        Button buttonRevive;
        Button buttonGive;

        void OnEnable()
        {
            inventoryController.SetSiblingIndex(SetSiblingType.TransformLastSibling);

            buttonRevive.onClick.AddListener(ButtonReviveHandler);
            buttonGive.onClick.AddListener(ButtonGiveHandler);
        }
        void OnDisable()
        {
            inventoryController.SetSiblingIndex(SetSiblingType.StartSiblingPosition);

            buttonRevive.onClick.RemoveListener(ButtonReviveHandler);
            buttonGive.onClick.RemoveListener(ButtonGiveHandler);
        }

        void ButtonReviveHandler()
        {
            Application.Quit();
        }

        void ButtonGiveHandler()
        {
            EventBus.Fire<PrepareNewLevelEvent>();
            gameObject.SetActive(false);
            //TODO Kazanc - kayip sistyemi eklenecek
        }

        void OnValidate()
        {
            if (buttonRevive == null)
                buttonRevive = transform.Find("ui_bombpanel_button_revive").GetComponent<Button>();

            if (buttonGive == null)
                buttonGive = transform.Find("ui_bombpanel_button_give").GetComponent<Button>();
        }
    }

}
