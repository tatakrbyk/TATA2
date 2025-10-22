using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Fire Projectile Action")]

    public class FireProjectileAction : WeaponItemAction
    {
        [SerializeField] ProjectileSlot projectileSlot;

        public override void AttemptToPerformAction(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            base.AttemptToPerformAction(playerPerformAction, weaponPerformAction);

            if (!playerPerformAction.IsOwner) return;
            if (playerPerformAction.playerNetworkManager.currentStamina.Value <= 0) { return; }

            RangedProjectileItem projectileItem = null;

            switch (projectileSlot)
            {
                case ProjectileSlot.Main:
                    projectileItem = playerPerformAction.playerInventoryManager.mainProjectile;
                    break;
                case ProjectileSlot.Secondary:
                    projectileItem = playerPerformAction.playerInventoryManager.secondaryProjectile;
                    break;
                default:
                    break;
            }

            if(projectileItem == null) { return; }

            if(!playerPerformAction.IsOwner) { return; }
            
            // If the player is not two handing the weapon , make them two hand it now (Weapon must be two handed to fire projectile)
            if(!playerPerformAction.playerNetworkManager.IsTwoHandingWeapon.Value)
            {
                if(playerPerformAction.playerNetworkManager.isUsingRightHand.Value)
                {
                    playerPerformAction.playerNetworkManager.IsTwoHandingRightWeapon.Value = true;
                }
                else if(playerPerformAction.playerNetworkManager.isUsingLeftHand.Value)
                {
                    playerPerformAction.playerNetworkManager.IsTwoHandingLeftWeapon.Value = true;
                }
            }
            

            // If the player does not have an arrow notched, do so now
            if(!playerPerformAction.playerNetworkManager.hasArrowNotched.Value)
            {
                playerPerformAction.playerNetworkManager.hasArrowNotched.Value = true;

                bool canIDrawAProjectile = CanIFireThisProjectile(weaponPerformAction, projectileItem);

                if(!canIDrawAProjectile) { return; }
                if (projectileItem.currentAmmoAmount <= 0)
                {
                    // Play a silly animation here that indicates they are out of ammo
                    playerPerformAction.playerAnimatorManager.PlayActionAnimation("Out_Of_Ammo_01", true);
                    return;
                }

                playerPerformAction.playerCombatManager.currentProjectileBeingUsed = projectileSlot;
                playerPerformAction.playerAnimatorManager.PlayActionAnimation("Bow_Draw_01", true);
                playerPerformAction.playerNetworkManager.NotifyServerOfDrawnProjectileServerRpc(projectileItem.itemID);
            }
        }

        private bool CanIFireThisProjectile(WeaponItem weaponPerformingAction, RangedProjectileItem projectileItem)
        {
            return true;
        }

    }
}
