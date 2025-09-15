using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerUIToggleHUD : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerUIManager.Instance.playerUIHUDManager.ToggleHUD(false);
        }
        private void OnDisable()
        {
            PlayerUIManager.Instance.playerUIHUDManager.ToggleHUD(true);
        }
    }

}
