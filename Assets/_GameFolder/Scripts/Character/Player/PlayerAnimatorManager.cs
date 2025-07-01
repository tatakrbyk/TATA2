using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        // Called per frame
        private void OnAnimatorMove()
        {
            if(player.playerAnimatorManager.applyRootMotion)
            {
                Vector3 velocity = player.animator.deltaPosition;
                player.characterController.Move(velocity);
                player.transform.rotation *= player.animator.deltaRotation;
            } 
        }

        // Call Animation = "straight_sword_light_attack_01/02"
        public override void EnableCanDoCombo()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerCombatManager.canCommboWithMainHandWeapon = true;

            }
            else
            {

            }
        }

        public override void DisableCanDoCombo()
        {
            player.playerCombatManager.canCommboWithMainHandWeapon = false;
            player.playerCombatManager.canCommboWithOffHandWeapon = false;
        }
    }

}
