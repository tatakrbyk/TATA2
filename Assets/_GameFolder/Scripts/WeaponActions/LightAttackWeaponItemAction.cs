using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Light Attack Action")]

    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        private string light_Attack_01 = "Main_Light_Attack_01";
        private string light_Attack_02 = "Main_Light_Attack_02";
        public override void AttemptToPerformAction(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            base.AttemptToPerformAction(playerPerformAction, weaponPerformAction);

            if (!playerPerformAction.IsOwner) return;

            if (playerPerformAction.playerNetworkManager.currentStamina.Value <= 0) { return; }
            if(!playerPerformAction.playerLocomotionManager.isGrounded) { return; }

            PerformLightAttack(playerPerformAction, weaponPerformAction);
        }

        private void PerformLightAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            // Combo Attack
            if (playerPerformAction.playerCombatManager.canCommboWithMainHandWeapon && playerPerformAction.isPerformingAction)
            {
                playerPerformAction.playerCombatManager.canCommboWithMainHandWeapon = false;

                if(playerPerformAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01)
                {

                    playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(AttackType.LightAttack02, light_Attack_02, true);
                }
                else
                {
                    playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);

                }

            }
            // Normal Attack
            else if(!playerPerformAction.isPerformingAction)
            {
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);

            }

        }
    }


}
