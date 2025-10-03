using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu( fileName = "CastIncantationAction", menuName = "Character Actions/ Weapon Actions/Incantation Action")]

    public class CastIncantationAction : WeaponItemAction
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            base.AttemptToPerformAction(playerPerformAction, weaponPerformAction);

            if(!playerPerformAction.IsOwner) { return; }

            if(playerPerformAction.playerNetworkManager.currentStamina.Value <= 0) { return; }

            if(!playerPerformAction.characterLocomotionManager.isGrounded) { return; }

            if(playerPerformAction.playerInventoryManager.currentSpell == null) { return; }

            if(playerPerformAction.playerInventoryManager.currentSpell.spellClass != SpellClass.Incantation) { return; }

            if (playerPerformAction.IsOwner)
            {
                playerPerformAction.playerNetworkManager.isAttacking.Value = true;
            }
            
            CastIncantation(playerPerformAction, weaponPerformAction);
        }

        private void CastIncantation(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
           playerPerformAction.playerInventoryManager.currentSpell.AttemptToCastSpell(playerPerformAction);
        }
    }

}
