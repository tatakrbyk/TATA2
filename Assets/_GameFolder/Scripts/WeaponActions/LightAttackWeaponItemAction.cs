using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Light Attack Action")]

    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [Header("Light Attacks")]
        private string light_Attack_01 = "Main_Light_Attack_01";
        private string light_Attack_02 = "Main_Light_Attack_02";

        [Header("Attacs")]
        private string running_Attack_01 = "Main_Run_Attack_01";
        private string rolling_Attack_01 = "Main_Roll_Attack_01";
        private string backstep_Attack_01 = "Main_Backstep_Attack_01";

        public override void AttemptToPerformAction(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            base.AttemptToPerformAction(playerPerformAction, weaponPerformAction);

            if (!playerPerformAction.IsOwner) return;

            if (playerPerformAction.playerNetworkManager.currentStamina.Value <= 0) { return; }
            if(!playerPerformAction.playerLocomotionManager.isGrounded) { return; }

            // If we are sprinting, we perform a running attack
            if (playerPerformAction.characterNetworkManager.isSprinting.Value)
            {
                PerformRunningAttack(playerPerformAction, weaponPerformAction);
                return;
            }
            // If we are rolling, we perform a rolling attack
            if (playerPerformAction.characterCombatManager.canPerformRollingtAttack)
            {
                PerformRollingAttack(playerPerformAction, weaponPerformAction);
                return;
            }
            // If we are backstep, we perform a backstep attack
            if (playerPerformAction.characterCombatManager.canPerformBackstepAttack)
            {
                PerformBackstepAttack(playerPerformAction, weaponPerformAction);
                return;
            }
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

        private void PerformRunningAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            // TODO: One/Two Handed Running Attackq
            playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(AttackType.RunningAttack01, running_Attack_01, true);

        }
        private void PerformRollingAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            // TODO: One/Two Handed Running Attackq
            playerPerformAction.playerCombatManager.canPerformRollingtAttack = false; // Reset the rolling attack flag
            playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(AttackType.RollingAttack01, rolling_Attack_01, true);

        }
        private void PerformBackstepAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            // TODO: One/Two Handed Running Attackq
            playerPerformAction.playerCombatManager.canPerformBackstepAttack = false; // Reset the rolling attack flag
            playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(AttackType.BackstepAttack01, backstep_Attack_01, true);

        }
    }


}
