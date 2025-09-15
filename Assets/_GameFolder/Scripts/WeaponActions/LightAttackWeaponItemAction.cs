using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace XD
{
    [CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Light Attack Action")]

    public class LightAttackWeaponItemAction : WeaponItemAction
    {   // Main Hand
        [Header("Light Attacks")]
        private string light_Attack_01 = "Main_Light_Attack_01";
        private string light_Attack_02 = "Main_Light_Attack_02";

        [Header("Attacs")]
        private string running_Attack_01 = "Main_Run_Attack_01";
        private string rolling_Attack_01 = "Main_Roll_Attack_01";
        private string backstep_Attack_01 = "Main_Backstep_Attack_01";

        // Two Hand
        [Header("Two Handed Light Attacks")]
        private string th_light_Attack_01 = "TH_Light_Attack_01";
        private string th_light_Attack_02 = "TH_Light_Attack_02";

        private string th_running_Attack_01 = "TH_Run_Attack_01";
        private string th_rolling_Attack_01 = "TH_Roll_Attack_01";
        private string th_backstep_Attack_01 = "TH_Backstep_Attack_01";

        public override void AttemptToPerformAction(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            base.AttemptToPerformAction(playerPerformAction, weaponPerformAction);

            if (!playerPerformAction.IsOwner) return;

            if (playerPerformAction.playerNetworkManager.currentStamina.Value <= 0) { return; }
            if(!playerPerformAction.playerLocomotionManager.isGrounded) { return; }
            
            if(playerPerformAction.IsOwner)
            {
                playerPerformAction.playerNetworkManager.isAttacking.Value = true;
            }
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
            
            playerPerformAction.characterCombatManager.AttemptCriticalAttack();
            PerformLightAttack(playerPerformAction, weaponPerformAction);
        }

        private void PerformLightAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            if(playerPerformAction.playerNetworkManager.IsTwoHandingWeapon.Value)
            {
                PerformTwoHandLightAttack(playerPerformAction, weaponPerformAction);
             
            }
            else
            {
                PerformMainHandLightAttack(playerPerformAction, weaponPerformAction);
            }
        }

        private void PerformMainHandLightAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            // Combo Attack
            if (playerPerformAction.playerCombatManager.canCommboWithMainHandWeapon && playerPerformAction.isPerformingAction)
            {
                playerPerformAction.playerCombatManager.canCommboWithMainHandWeapon = false;

                if (playerPerformAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01)
                {

                    playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.LightAttack02, light_Attack_02, true);
                }
                else
                {
                    playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.LightAttack01, light_Attack_01, true);

                }

            }
            // Normal Attack
            else if (!playerPerformAction.isPerformingAction)
            {
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.LightAttack01, light_Attack_01, true);

            }
        }

        private void PerformTwoHandLightAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            // Combo Attack
            if (playerPerformAction.playerCombatManager.canCommboWithMainHandWeapon && playerPerformAction.isPerformingAction)
            {
                playerPerformAction.playerCombatManager.canCommboWithMainHandWeapon = false;

                if (playerPerformAction.characterCombatManager.lastAttackAnimationPerformed == th_light_Attack_01)
                {

                    playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.LightAttack02, th_light_Attack_02, true);
                }
                else
                {
                    playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.LightAttack01, th_light_Attack_01, true);

                }

            }
            // Normal Attack
            else if (!playerPerformAction.isPerformingAction)
            {
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.LightAttack01, th_light_Attack_01, true);

            }
        }
        private void PerformRunningAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            if(playerPerformAction.playerNetworkManager.IsTwoHandingWeapon.Value)
            {
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.RunningAttack01, th_running_Attack_01, true);
            }
            else
            {
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.RunningAttack01, running_Attack_01, true);
            }

        }
        private void PerformRollingAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            playerPerformAction.playerCombatManager.canPerformRollingtAttack = false; // Reset the rolling attack flag
            if (playerPerformAction.playerNetworkManager.IsTwoHandingWeapon.Value)
            {
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.RollingAttack01, th_rolling_Attack_01, true);
            }
            else
            { 
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.RollingAttack01, rolling_Attack_01, true);
            }
        }
        private void PerformBackstepAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            playerPerformAction.playerCombatManager.canPerformBackstepAttack = false; // Reset the rolling attack flag

            if (playerPerformAction.playerNetworkManager.IsTwoHandingWeapon.Value)
            {
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.BackstepAttack01, th_backstep_Attack_01, true);
            }
            else
            {
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.BackstepAttack01, backstep_Attack_01, true);
            }
        }
    }


}
