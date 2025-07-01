using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Heavy Attack Action")]
    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {
        private string heavy_Attack_01 = "Main_Heavy_Attack_01";
        private string heavy_Attack_02 = "Main_Heavy_Attack_02";
        public override void AttemptToPerformAction(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            base.AttemptToPerformAction(playerPerformAction, weaponPerformAction);

            if (!playerPerformAction.IsOwner) return;

            if (playerPerformAction.playerNetworkManager.currentStamina.Value <= 0) { return; }
            if (!playerPerformAction.playerLocomotionManager.isGrounded) { return; }

            PerformHeavyAttack(playerPerformAction, weaponPerformAction);
        }

        private void PerformHeavyAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {

            // Combo Attack
            if (playerPerformAction.playerCombatManager.canCommboWithMainHandWeapon && playerPerformAction.isPerformingAction)
            {
                playerPerformAction.playerCombatManager.canCommboWithMainHandWeapon = false;

                if (playerPerformAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_01)
                {
                    playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(AttackType.HeavyAttack02, heavy_Attack_02, true);

                }
                else
                {
                    playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(AttackType.HeavyAttack01, heavy_Attack_01, true);


                }

            }
            // Normal Attack
            else if (!playerPerformAction.isPerformingAction)
            {
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(AttackType.HeavyAttack01, heavy_Attack_01, true);


            }

        }
    }

}
