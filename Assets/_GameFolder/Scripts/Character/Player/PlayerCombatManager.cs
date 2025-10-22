using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;
        public ProjectileSlot currentProjectileBeingUsed;

        [Header("Projectile")]
        private Vector3 projectileAimDirection;

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

                // Que 60 call, 
                //player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformAction.itemID);

            }
        }

        public override void CloseAllDamageColliders()
        {
            base.CloseAllDamageColliders();

            player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
            player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
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

            if (player.playerNetworkManager.IsTwoHandingLeftWeapon.Value)
            {
                riposteWeapon = player.playerInventoryManager.currentLeftHandWeapon as MeleeWeaponItem;
                riposteCollider = player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider;
            }
            else
            {
                riposteWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
                riposteCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;
            }

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

        public override void AttemptedBackStab(RaycastHit hit)
        {
            CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();
            if (targetCharacter == null) { return; }

            // If some how since the initial check the character can no longer be riposted, return
            if (!targetCharacter.characterCombatManager.canBeBackstabbed) { return; }

            // If Somebody else is already performin a critical strike on the character ( or we already are), return
            if (targetCharacter.characterNetworkManager.isBeginCriticallyDamaged.Value) { return; }

            MeleeWeaponItem backstabWeapon;
            MeleeWeaponDamageCollider backstabCollider;

            if(player.playerNetworkManager.IsTwoHandingLeftWeapon.Value)
            {
                backstabWeapon = player.playerInventoryManager.currentLeftHandWeapon as MeleeWeaponItem;
                backstabCollider = player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider;
            }
            else
            {
                backstabWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
                backstabCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;
            }

            // NOTE: The Ripsote Animation will change depending on the weapon's animator controller, so the animation can be choosen there, the name will always be the same
            character.characterAnimatorManager.PlayActionAnimationInstantly("Backstab_01", true);

            if (character.IsOwner)
            {
                character.characterNetworkManager.isInvulnerable.Value = true;
            }

            // Create  a new damage effect for this type of damage
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeCriticalDamageEffect);

            damageEffect.physicalDamage = backstabWeapon.physicalDamage;
            damageEffect.holyDamage = backstabWeapon.holyDamage;
            damageEffect.fireDamage = backstabWeapon.fireDamage;
            damageEffect.lightningDamage = backstabWeapon.lightningDamage;
            damageEffect.magicDamage = backstabWeapon.magicDamage;
            damageEffect.poiseDamage = backstabCollider.poiseDamage;

            damageEffect.physicalDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.holyDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.fireDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.lightningDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.magicDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.poiseDamage *= backstabWeapon.backstab_Attack_01_Modifier;

            // Using a Server RPC send the riposte to the target, where they will play the proper animations ýn their end, take the damage
            targetCharacter.characterNetworkManager.NotifyTheServerOfBackstabServerRPC(
                targetCharacter.NetworkObjectId,
                character.NetworkObjectId,
                "Backstabbed_01",
                backstabWeapon.itemID,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.fireDamage,
                damageEffect.holyDamage,
                backstabCollider.poiseDamage);
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
                case AttackType.LightJumpingAttack01:
                    staminaDeduted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack01:
                    staminaDeduted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack02:
                    staminaDeduted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyJumpingAttack01:
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

        // Call Animation = "Bow_th_fire_01"
        public void ReleaseArrow()
        {
            if(player.IsOwner)
            {
                player.playerNetworkManager.hasArrowNotched.Value = false;
            }
            if (player.playerEffectsManager.activeDrawnProjectileFX != null)
            {
                Destroy(player.playerEffectsManager.activeDrawnProjectileFX);
            }
            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(WorldSoundFXManager.Instance.releaseArrowSFX));

            // Animate The Bow
            Animator bowAnimator;

            if (player.playerNetworkManager.IsTwoHandingLeftWeapon.Value)
            {
                bowAnimator = player.playerEquipmentManager.leftHandWeaponModel.GetComponentInChildren<Animator>();
            }
            else
            {
                bowAnimator = player.playerEquipmentManager.rightHandWeaponModel.GetComponentInChildren<Animator>();
            }

            bowAnimator.SetBool("IsDrawn", false);
            bowAnimator.Play("Bow_Fire_01");

            if(!player.IsOwner) { return; }

            // The Projectile We Are Firing
            RangedProjectileItem projectileItem = null;

            switch(currentProjectileBeingUsed)
            {
                case ProjectileSlot.Main:
                    projectileItem = player.playerInventoryManager.mainProjectile;
                    break;
                case ProjectileSlot.Secondary:
                    projectileItem = player.playerInventoryManager.secondaryProjectile;
                    break;
                default:
                    break;
            }

            if(projectileItem == null) { return; }
            if(projectileItem.currentAmmoAmount <= 0) { return; }

            Transform projectileInstantiationLocation;
            GameObject projectileGameObject;
            Rigidbody projectileRigidbody;
            RangedProjectileDamageCollider projectileDamageCollider;

            
            // Subtract Ammo
            projectileItem.currentAmmoAmount -= 1;
            // TODO: Make And Update Aroow Count UI

            projectileInstantiationLocation = player.playerCombatManager.lockOnTransform;
            projectileGameObject = Instantiate(projectileItem.releaseProjectileModel, projectileInstantiationLocation);
            projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
            projectileRigidbody = projectileGameObject.GetComponent<Rigidbody>();

            // TODO: Make Formula To Set Range Projectile Damage
            projectileDamageCollider.physicalDamage = 100;
            projectileDamageCollider.characterShootingProjectile = player;

            // Fire an arrow based on 1 of 3 varitions

            float yRotationDuringFire = player.transform.localEulerAngles.y;
            // Aiming
            if(player.playerNetworkManager.isAiming.Value)
            {   
                Ray newRay = new Ray(lockOnTransform.position, PlayerCamera.Instance.aimDirection);
                projectileAimDirection = newRay.GetPoint(5000);
                projectileGameObject.transform.LookAt(projectileAimDirection);
            }
            else
            {
                // Locked And Not Aiming
                if (player.playerCombatManager.currentTarget != null)
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position
                        - projectileGameObject.transform.position);
                    projectileGameObject.transform.rotation = arrowRotation;
                }               
                else
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(player.transform.forward);
                    projectileGameObject.transform.rotation = arrowRotation;

                }

            }

            // Get all character colliders and ignore self
            Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
            List<Collider> collidersArrowWillIgonore = new List<Collider>();

            foreach(var item in characterColliders) {  collidersArrowWillIgonore.Add(item); }
            foreach(Collider hitBox in collidersArrowWillIgonore)
            {
                Physics.IgnoreCollision(projectileDamageCollider.damageCollider, hitBox, true);
            }

            projectileRigidbody.AddForce(projectileGameObject.transform.forward * projectileItem.forwardVelocity);
            projectileGameObject.transform.parent = null;

            // TODO: Sync Arrow fire wih server RPC
            player.playerNetworkManager.NotifyServerOfReleasedProjectileServerRpc(player.OwnerClientId, projectileItem.itemID, 
                projectileAimDirection.x, projectileAimDirection.y, projectileAimDirection.z, yRotationDuringFire);

        }

        // CALL ANIMATION = "sphand_main_projectile_01_charge"
        public void InstantiateSpellWarmUpFX()
        {
            if(player.playerInventoryManager.currentSpell == null) { return; }

            player.playerInventoryManager.currentSpell.InstantiateWarmUpSpellFX(player);
        }

        // CALL ANIMATION = "sphand_main_projectile_01_Release"
        public void SuccessfullyCastSpell()
        {
            if(player.playerInventoryManager.currentSpell == null) { return; }
            
            player.playerInventoryManager.currentSpell.SuccessfullyCastSpell(player);
        }

        // CALL ANIMATION = "sphand_main_projectile_01_Release_full"
        public void SuccessfullyCastSpellFullCharge()
        {
            if (player.playerInventoryManager.currentSpell == null) { return; }

            player.playerInventoryManager.currentSpell.SuccessfullyCastSpellFullCharge(player);
        }

        public void SuccessfullyChargeSpell()
        {
            if (player.playerInventoryManager.currentSpell == null) { return; }

            player.playerInventoryManager.currentSpell.SuccessfullyChargeSpell(player);
        }
        public WeaponItem SelectWeaponToPerformAshOfWar()
        {
            // TODO: Select weapon depending on setuo
            WeaponItem selectedWeapon = player.playerInventoryManager.currentLeftHandWeapon;
            player.playerNetworkManager.SetCharacterActionHand(false);
            player.playerNetworkManager.currentWeaponBeingUsed.Value = selectedWeapon.itemID;

            return selectedWeapon;

        }
    }

}
