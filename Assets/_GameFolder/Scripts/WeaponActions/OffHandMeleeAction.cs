using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Off Hand Melee Action", fileName = "OffHandMeleeAction")]

    public class OffHandMeleeAction : WeaponItemAction
    {
        override public void AttemptToPerformAction(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            base.AttemptToPerformAction(playerPerformAction, weaponPerformAction);

            if (!playerPerformAction.playerCombatManager.canBlock) return;
            
            if (playerPerformAction.playerNetworkManager.isAttacking.Value) 
            {
                // Disable Blocking (When using a short/medium spear block attacking is allowed light attacks. Handled on another action class)
                if(playerPerformAction.IsOwner)
                {
                    playerPerformAction.playerNetworkManager.isBlocking.Value = false;
                }
            }

            if (playerPerformAction.playerNetworkManager.isBlocking.Value) { return; }

            if (playerPerformAction.IsOwner)
            {
                playerPerformAction.playerNetworkManager.isBlocking.Value = true;
            }
            
        }
    }

}
