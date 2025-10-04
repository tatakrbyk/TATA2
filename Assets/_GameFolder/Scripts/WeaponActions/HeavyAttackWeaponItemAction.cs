using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Heavy Attack Action")]
    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {

        [Header("Main Heavy Attacks")]
        private string heavy_Attack_01 = "Main_Heavy_Attack_01";
        private string heavy_Attack_02 = "Main_Heavy_Attack_02";
        private string heavy_Jumping_Attack_01 = "Main_Heavy_Jump_Attack_01";

        [Header("Two Hand Heavy Attacks")]
        private string th_heavy_Attack_01 = "TH_Heavy_Attack_01";
        private string th_heavy_Attack_02 = "TH_Heavy_Attack_02";
        private string th_heavy_Jumping_Attack_01 = "TH_Heavy_Jump_Attack_01";

        public override void AttemptToPerformAction(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            base.AttemptToPerformAction(playerPerformAction, weaponPerformAction);

            if (!playerPerformAction.IsOwner) return;

            if (playerPerformAction.playerNetworkManager.currentStamina.Value <= 0) { return; }
            if (!playerPerformAction.playerLocomotionManager.isGrounded) 
            {
                PerformJumpingHeavyAttack(playerPerformAction, weaponPerformAction);
                return;
            }

            if(playerPerformAction.playerNetworkManager.isJumping.Value) { return; }

            PerformHeavyAttack(playerPerformAction, weaponPerformAction);
        }

        private void PerformHeavyAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            if(playerPerformAction.playerNetworkManager.IsTwoHandingWeapon.Value)
            {
                PerformTwoHandHeavyAttack(playerPerformAction, weaponPerformAction);
            }
            else
            {
                PerformMainHandHeavyAttack(playerPerformAction, weaponPerformAction);
            }
        }

        private void PerformMainHandHeavyAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            // Combo Attack
            if (playerPerformAction.playerCombatManager.canCommboWithMainHandWeapon && playerPerformAction.isPerformingAction)
            {
                playerPerformAction.playerCombatManager.canCommboWithMainHandWeapon = false;

                if (playerPerformAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_01)
                {
                    playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.HeavyAttack02, heavy_Attack_02, true);

                }
                else
                {
                    playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.HeavyAttack01, heavy_Attack_01, true);


                }

            }
            // Normal Attack
            else if (!playerPerformAction.isPerformingAction)
            {
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.HeavyAttack01, heavy_Attack_01, true);


            }
        }

        private void PerformTwoHandHeavyAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            // Combo Attack
            if (playerPerformAction.playerCombatManager.canCommboWithMainHandWeapon && playerPerformAction.isPerformingAction)
            {
                playerPerformAction.playerCombatManager.canCommboWithMainHandWeapon = false;

                if (playerPerformAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_01)
                {
                    playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.HeavyAttack02, th_heavy_Attack_02, true);

                }
                else
                {
                    playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.HeavyAttack01, th_heavy_Attack_01, true);


                }

            }
            // Normal Attack
            else if (!playerPerformAction.isPerformingAction)
            {
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.HeavyAttack01, th_heavy_Attack_01, true);


            }
        }

        private void PerformJumpingHeavyAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            if (playerPerformAction.playerNetworkManager.IsTwoHandingWeapon.Value)
            {
                PerformTwoHandJumpingHeavyAttack(playerPerformAction, weaponPerformAction);
            }
            else
            {
                PerformMainHandJumpingHeavyAttack(playerPerformAction, weaponPerformAction);

            }
        }

        private void PerformMainHandJumpingHeavyAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            if (playerPerformAction.isPerformingAction)
            {
                return;                
            }

            playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.HeavyJumpingAttack01, heavy_Jumping_Attack_01, true);
        }
        private void PerformTwoHandJumpingHeavyAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            if (playerPerformAction.isPerformingAction)
            {
                return;
            }

            playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(weaponPerformAction, AttackType.HeavyJumpingAttack01, th_heavy_Jumping_Attack_01, true);
        }
    }

}
