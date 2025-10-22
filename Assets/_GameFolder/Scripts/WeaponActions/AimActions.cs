using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Aim Action")]
    public class AimActions : WeaponItemAction
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            base.AttemptToPerformAction(playerPerformAction, weaponPerformAction);

            if(!playerPerformAction.playerLocomotionManager.isGrounded) { return; }
            if(playerPerformAction.playerNetworkManager.isJumping.Value) { return; }
            if(playerPerformAction.playerLocomotionManager.isRolling) { return; }
            if (playerPerformAction.playerNetworkManager.isLockedOn.Value) { return; }

            if(playerPerformAction.IsOwner)
            {
                // Two Hand thw weapon ( bow ) before we aim
                if (!playerPerformAction.playerNetworkManager.IsTwoHandingWeapon.Value)
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

                playerPerformAction.playerNetworkManager.isAiming.Value = true;
            }            
        }
    }
}
