using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName ="Items/Consumeables/Flask")]
    public class FlaskItem : QuickSlotItem
    {
        [Header("Flask Type")]
        public bool healthFlask = true;

        [Header("Restoration Value")]
        [SerializeField] private int flaskRestoration = 50;
        
        [Header("Empty Item")]
        public GameObject empytFlaskItem;
        public string emptyFlaskAnimation;


        public override bool CanIUseThisItem(PlayerManager player)
        {   
            if(!player.playerCombatManager.isUsingItem && player.isPerformingAction) { return false; }
            if (player.playerNetworkManager.isAttacking.Value) { return false; }
            return false;
            return true;
        }
        public override void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player)) { return; }   

            // Health Flask Check
            if (healthFlask && player.playerNetworkManager.remainingHealthFlasks.Value <= 0) 
            {
                if (player.playerCombatManager.isUsingItem) { return; }
                player.playerCombatManager.isUsingItem = true;

                if (player.IsOwner)
                {
                    player.playerAnimatorManager.PlayActionAnimation(emptyFlaskAnimation, false, false, true, true, false);
                    player.playerNetworkManager.HideWeaponsServerRpc();
                }

                Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                GameObject emptyFlask = Instantiate(empytFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
                player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
                return;
            }
            // Focus Flask Check
            if (!healthFlask && player.playerNetworkManager.remainingFocusPointsFlasks.Value <= 0) 
            {
                if (player.playerCombatManager.isUsingItem) { return; }
                player.playerCombatManager.isUsingItem = true;

                if (player.IsOwner)
                {
                    player.playerAnimatorManager.PlayActionAnimation(emptyFlaskAnimation, false, false, true, true, false);
                    player.playerNetworkManager.HideWeaponsServerRpc();

                }
                Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                GameObject emptyFlask = Instantiate(empytFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
                player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
                return;
            }

            // Check For Chugging
            if(player.playerCombatManager.isUsingItem)
            {
                if(player.IsOwner)
                {
                    player.playerNetworkManager.isChugging.Value = true;
                }
                return;
            }

            player.playerCombatManager.isUsingItem = true;
            player.playerEffectsManager.activeQuickSlotItemFX = Instantiate(ItemModel, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            if(player.IsOwner)
            {
                player.playerAnimatorManager.PlayActionAnimation(useItemAnimation, false, false, true, true, false);
                player.playerNetworkManager.HideWeaponsServerRpc();

            }
        }

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            if(player.IsOwner)
            {
                if(healthFlask)
                {
                    player.playerNetworkManager.currentHealth.Value += flaskRestoration;
                    player.playerNetworkManager.remainingHealthFlasks.Value -= 1;
                }
                else
                {
                    player.playerNetworkManager.currentFocusPoints.Value += flaskRestoration;
                    player.playerNetworkManager.remainingFocusPointsFlasks.Value -= 1;

                }

                PlayerUIManager.Instance.playerUIHUDManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);

            }

            if (healthFlask && player.playerNetworkManager.remainingHealthFlasks.Value <= 0)
            {
                Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                GameObject emptyFlask = Instantiate(empytFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
                player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
            }
            if(!healthFlask && player.playerNetworkManager.remainingFocusPointsFlasks.Value <= 0)
            {
                Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                GameObject emptyFlask = Instantiate(empytFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
                player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
            }

            PlayHealingFX(player);
        }

        public override int GetCurrentAmount(PlayerManager player)
        {
            int currentAmount = 0;

            if(healthFlask)
            {
                currentAmount = player.playerNetworkManager.remainingHealthFlasks.Value;
            }
            else
            {
                currentAmount = player.playerNetworkManager.remainingFocusPointsFlasks.Value;
            }

            return currentAmount;   
        }

        private void PlayHealingFX(PlayerManager player)
        {
            Instantiate(WorldCharacterEffectsManager.Instance.healingFlaskVFX, player.transform);
            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Instance.healingFlaskSFX);   
        }
        }
}
