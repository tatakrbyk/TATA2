using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace XD
{
    public class SiteOfGracesInteractable : Interactable
    {
        [Header("Site of Grace Info")]
        [SerializeField] private int siteOfGraceID;
        public NetworkVariable<bool> isActivated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("VFX")]
        [SerializeField] private GameObject activatedParticles;

        [Header("Interaction Text")]
        [SerializeField] string unactivatedInteractionText = "Restore Site of Grace";
        [SerializeField] string activatedInteractionText = "Rest";

        protected override void Start()
        {
            base.Start();

            if(IsOwner)
            {
                if (WorldSaveGameManager.Instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
                {
                    isActivated.Value = WorldSaveGameManager.Instance.currentCharacterData.sitesOfGrace[siteOfGraceID];
                }
                else
                {
                    isActivated.Value = false;
                }

            }
            if(isActivated.Value)
            {
                interactableText = activatedInteractionText;
            }
            else
            {
                interactableText = unactivatedInteractionText;
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if(!IsOwner)
            {
                OnIsActivatedChanged(false, isActivated.Value);
            }

            isActivated.OnValueChanged += OnIsActivatedChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            isActivated.OnValueChanged -= OnIsActivatedChanged;
        }
        private void RestoreSiteOfGrace(PlayerManager player)
        {
            isActivated.Value = true;
            // Adds Sýte Of Grace to activated sites in save files
            if(WorldSaveGameManager.Instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.sitesOfGrace.Remove(siteOfGraceID);
            }

            WorldSaveGameManager.Instance.currentCharacterData.sitesOfGrace.Add(siteOfGraceID, true);

            player.playerAnimatorManager.PlayActionAnimation("Activate_Site_Of_Grace_01", true);
            
            PlayerUIManager.Instance.playerUIPopUpManager.SendGraceRestoredPopUp("Site of Grace Restored");
            StartCoroutine(WaitForAnimationAndPopUpThenRestoreCollider());

        }

        private void RestAtSiteOfGrace(PlayerManager player)
        {
            interactableCollider.enabled = true; // Temp
            player.playerNetworkManager.currentHealth.Value = player.playerNetworkManager.maxHealth.Value;
            player.playerNetworkManager.currentStamina.Value = player.playerNetworkManager.maxStamina.Value;

            WorldSaveGameManager.Instance.SaveGame();
            
            WorldAIManager.Instance.ResetAllCharacters();
            WorldSaveGameManager.Instance.LoadGame();

        }

        private IEnumerator  WaitForAnimationAndPopUpThenRestoreCollider()
        {
            yield return new WaitForSeconds(2);
            interactableCollider.enabled = true;
        }
        private void OnIsActivatedChanged(bool oldStatus, bool newStatus)
        {
            if(isActivated.Value)
            {
                activatedParticles.SetActive(true);
                interactableText = activatedInteractionText;
            }
            else
            {
                interactableText = unactivatedInteractionText;
            }
        }
        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            if(!isActivated.Value)
            {
                RestoreSiteOfGrace(player);
            }
            else
            {
                RestAtSiteOfGrace(player);
            }
        }
    }

}
