using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace XD
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformAction)
        {
            if (player.IsOwner)
            {
                weaponAction.AttemptToPerformAction(player, weaponPerformAction);

                player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformAction.itemID);

            }
        }

        // Call: Main_Light_Attack_01 Events
        public virtual void DrainStaminaBasedOnAttack()
        {
            if(!player.IsOwner){ return; }
            if (currentWeaponBeingUsed == null) return;

            float staminaDeduted = 0;

            switch(currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDeduted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                default:
                    break;


            }
            Debug.Log($"Stamina Deduted: {staminaDeduted}");
            player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeduted); 
        }

        public override void SetTarget(CharacterManager newTarget)
        {
            base.SetTarget(newTarget);

            if(player.IsOwner)
            {
                PlayerCamera.Instance.SetLockCameraHeight();
            }
        }
    }

}
