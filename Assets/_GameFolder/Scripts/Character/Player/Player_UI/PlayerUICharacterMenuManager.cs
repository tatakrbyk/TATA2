using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerUICharacterMenuManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] private GameObject menu;

        public void OpenCharacterMenu()
        {
            PlayerUIManager.Instance.menuWindowIsOpen = true;
            menu.SetActive(true);
        }
        public void CloseCharacterMenu()
        {
            PlayerUIManager.Instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

        public void CloseCharacterMenuAfterFixedFrame()
        {
            StartCoroutine(WaitThenCloseMenu());
        }
        private IEnumerator WaitThenCloseMenu()
        {
            yield return new WaitForFixedUpdate();

            PlayerUIManager.Instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }
    }

}
