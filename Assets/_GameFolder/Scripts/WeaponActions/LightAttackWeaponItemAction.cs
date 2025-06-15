using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Light Attack Action")]

    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        private string light_Attack_01 = "Main_Light_Attack_01";
        public override void AttemptToPerformAction(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            base.AttemptToPerformAction(playerPerformAction, weaponPerformAction);

            if (!playerPerformAction.IsOwner) return;

            if (playerPerformAction.playerNetworkManager.currentStamina.Value <= 0) { return; }
            if(!playerPerformAction.isGrounded) { return; }

            PerformLightAttack(playerPerformAction, weaponPerformAction);
        }

        private void PerformLightAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {

            if (playerPerformAction.playerNetworkManager.isUsingRightHand.Value)
            {
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation( AttackType.LightAttack01, light_Attack_01, true);
            }
            if(playerPerformAction.playerNetworkManager.isUsingLeftHand.Value)
            {
                //playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation();
            }
        }
    }


}
