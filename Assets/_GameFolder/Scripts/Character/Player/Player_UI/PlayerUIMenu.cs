using System.Collections;
using UnityEngine;

namespace XD
{
    public class PlayerUIMenu : MonoBehaviour
    {

        [Header("Menu")]
        [SerializeField] private GameObject menu;

        public virtual void OpenMenu()
        {
            PlayerUIManager.Instance.menuWindowIsOpen = true;
            menu.SetActive(true);
        }
        public virtual void CloseMenu()
        {
            PlayerUIManager.Instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

        public virtual void CloseMenuAfterFixedFrame()
        {
            if(!menu.activeInHierarchy) return;
            StartCoroutine(WaitThenCloseMenu());
        }
        protected virtual IEnumerator WaitThenCloseMenu()
        {
            yield return new WaitForFixedUpdate();

            PlayerUIManager.Instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }
    }
}
