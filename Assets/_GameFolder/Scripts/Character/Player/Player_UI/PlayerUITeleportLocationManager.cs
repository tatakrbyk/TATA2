using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerUITeleportLocationManager : PlayerUIMenu
    {
        [Header("Teleport Locations")]
        [SerializeField] private GameObject[] teleportLocations;

        public override void OpenMenu()
        {
            base.OpenMenu();
            CheckForUnlockedTeleports();

        }

        private void CheckForUnlockedTeleports()
        {
            bool hasFirstSelectedButton = false;
            for (int i = 0; i < teleportLocations.Length; i++)
            {
                for(int s = 0; s < WorldObjectManager.Instance.sitesOfGrace.Count; s++)
                {
                    if (WorldObjectManager.Instance.sitesOfGrace[s].siteOfGraceID == i)
                    {
                        if (WorldObjectManager.Instance.sitesOfGrace[s].isActivated.Value)
                        {
                            teleportLocations[i].SetActive(true);
                            
                            if(!hasFirstSelectedButton)
                            {
                                hasFirstSelectedButton = true;
                                teleportLocations[i].GetComponent<UnityEngine.UI.Button>().Select();

                            }
                        }
                        else
                        {
                            teleportLocations[i].SetActive(false);
                        }
                    }
                }
            }
        }
        public void TeleportToSiteOfGrace(int sideID)
        {
            for(int i = 0; i < WorldObjectManager.Instance.sitesOfGrace.Count; i++)
            {
                if (WorldObjectManager.Instance.sitesOfGrace[i].siteOfGraceID == sideID)
                {
                    WorldObjectManager.Instance.sitesOfGrace[i].TeleportToSiteOfGrace();
                    return;
                    
                }
            }
        }
    }
}