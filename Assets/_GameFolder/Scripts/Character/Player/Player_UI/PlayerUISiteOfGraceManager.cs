using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XD
{
    public class PlayerUISiteOfGraceManager : PlayerUIMenu
    {
        public void OpenTeleportLocationMenu()
        {
            CloseMenu();
            PlayerUIManager.Instance.playerUITeleportLocationManager.OpenMenu();
        }

        public void OpenLevelUpMenu()
        {
            CloseMenu();
            PlayerUIManager.Instance.playerUILevelUpManager.OpenMenu();
        }
    }
}
