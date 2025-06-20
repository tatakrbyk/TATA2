using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ Heavy Attack Action")]
    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {
        private string heavy_Attack_01 = "Main_Heavy_Attack_01";
        public override void AttemptToPerformAction(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            base.AttemptToPerformAction(playerPerformAction, weaponPerformAction);

            if (!playerPerformAction.IsOwner) return;

            if (playerPerformAction.playerNetworkManager.currentStamina.Value <= 0) { return; }
            if (!playerPerformAction.isGrounded) { return; }

            PerformHeavyAttack(playerPerformAction, weaponPerformAction);
        }

        private void PerformHeavyAttack(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {

            if (playerPerformAction.playerNetworkManager.isUsingRightHand.Value)
            {
                playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation(AttackType.HeavyAttack01, heavy_Attack_01, true);
            }
            if (playerPerformAction.playerNetworkManager.isUsingLeftHand.Value)
            {
                //playerPerformAction.playerAnimatorManager.PlayAttackActionAnimation();
            }
        }
    }

}
