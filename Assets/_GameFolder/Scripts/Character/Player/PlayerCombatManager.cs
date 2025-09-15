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

        [Header("Flags")]
        public bool canCommboWithMainHandWeapon = false;
        public bool canCommboWithOffHandWeapon = false;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;

        }
        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformAction)
        {
            if (player.IsOwner)
            {
                weaponAction.AttemptToPerformAction(player, weaponPerformAction);

                player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformAction.itemID);

            }
        }

        public override void AttemptRiposte(RaycastHit hit)
        {
            CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();
            if(targetCharacter == null) {  return; }

            // If some how since the initial check the character can no longer be riposted, return
            if(!targetCharacter.characterNetworkManager.isRipostable.Value) { return; }
            
            // If Somebody else is already performin a critical strike on the character ( or we already are), return
            if(targetCharacter.characterNetworkManager.isBeginCriticallyDamaged.Value) { return; }

            MeleeWeaponItem riposteWeapon;
            MeleeWeaponDamageCollider riposteCollider;

            riposteWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
            riposteCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;

            Debug.Log("RiposteAnim");

            // NOTE: The Ripsote Animation will change depending on the weapon's animator controller, so the animation can be choosen there, the name will always be the same
            character.characterAnimatorManager.PlayActionAnimationInstantly("Riposte_01", true);

            if(character.IsOwner)
            {
                character.characterNetworkManager.isInvulnerable.Value = true;
            }

            // Create  a new damage effect for this type of damage
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeCriticalDamageEffect);

            damageEffect.physicalDamage = riposteWeapon.physicalDamage;
            damageEffect.holyDamage = riposteWeapon.holyDamage;
            damageEffect.fireDamage = riposteWeapon.fireDamage;
            damageEffect.lightningDamage = riposteWeapon.lightningDamage;
            damageEffect.magicDamage = riposteWeapon.magicDamage;
            damageEffect.poiseDamage = riposteCollider.poiseDamage;

            damageEffect.physicalDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.holyDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.fireDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.lightningDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.magicDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.poiseDamage *= riposteWeapon.riposte_Attack_01_Modifier;

            // Using a Server RPC send the riposte to the target, where they will play the proper animations ýn their end, take the damage
            targetCharacter.characterNetworkManager.NotifyTheServerOfRiposteServerRPC(
                targetCharacter.NetworkObjectId,
                character.NetworkObjectId,
                "Ripsoted_01",
                riposteWeapon.itemID,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.fireDamage,
                damageEffect.holyDamage,
                riposteCollider.poiseDamage);
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
                case AttackType.LightAttack02:
                    staminaDeduted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack01:
                    staminaDeduted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack02:
                    staminaDeduted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack01:
                    staminaDeduted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack02:
                    staminaDeduted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.RunningAttack01:
                    staminaDeduted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.runningAttackStaminaCostMultiplier;
                    break;
                case AttackType.RollingAttack01:
                    staminaDeduted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.rollingAttackStaminaCostMultiplier;
                    break;
                case AttackType.BackstepAttack01:
                    staminaDeduted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.backstepAttackStaminaCostMultiplier;
                    break;

                default:
                    staminaDeduted = 0;
                    break;
            }
            
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
