using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(fileName = "FireBallSpell", menuName = "Items/Spells/Fire Ball Spell")]

    public class FireBallSpell : SpellItem
    {
        [Header("Projectile Velocity")]
        [SerializeField] private float upwardVelocity = 8f;
        [SerializeField] private float forwardVelocity = 15;
        public override void AttemptToCastSpell(PlayerManager player)
        {
            base.AttemptToCastSpell(player);

            if (!CanICastThisSpell(player))
            {
                return;
            }

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerAnimatorManager.PlayActionAnimation(mainHandSpellAnimation, true);
            }
            else
            {
                player.playerAnimatorManager.PlayActionAnimation(offHandSpellAnimation, true);
            }
        }

        public override void InstantiateWarmUpSpellFX(PlayerManager player)
        {
            base.InstantiateWarmUpSpellFX(player);

            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellCastWarmUpFX);

            if(player.playerNetworkManager.isUsingRightHand.Value)
            {
                spellInstantiationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();               
            }
            else
            {
                spellInstantiationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }

            instantiatedWarmUpSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiatedWarmUpSpellFX.transform.localPosition = Vector3.zero;
            instantiatedWarmUpSpellFX.transform.localRotation = Quaternion.identity;


            player.playerEffectsManager.activeSpellWarmUpFX = instantiatedWarmUpSpellFX; 
        }
        public override void SuccessfullyChargeSpell(PlayerManager player)
        {
            base.SuccessfullyChargeSpell(player);

            // Destroy Any Warp Up FX Remaining From The Spell
            if (player.IsOwner)
            {
                player.playerCombatManager.DestroyAllCurrentActionFX();
            }
            // Instantiate The Projectile
            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiatedChargeSpellFX = Instantiate(spellChargeFX);

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                spellInstantiationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                spellInstantiationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }

            player.playerEffectsManager.activeSpellWarmUpFX = instantiatedChargeSpellFX;
            // Use the list of colliders from the caster and now apply the ignore physics with the colliders from the projectile
            instantiatedChargeSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiatedChargeSpellFX.transform.localPosition = Vector3.zero;
            instantiatedChargeSpellFX.transform.localRotation = Quaternion.identity;
            

        }
        public override void SuccessfullyCastSpell(PlayerManager player)
        {
            base.SuccessfullyCastSpell(player);

            // Destroy Any Warp Up FX Remaining From The Spell
            if(player.IsOwner)
            {
                player.playerCombatManager.DestroyAllCurrentActionFX();
            }
            
            // Instantiate The Projectile
            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiatedReleasedSpellFX = Instantiate(spellCastReleaseFX);

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                spellInstantiationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                spellInstantiationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }

            // Use the list of colliders from the caster and now apply the ignore physics with the colliders from the projectile
            instantiatedReleasedSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiatedReleasedSpellFX.transform.localPosition = Vector3.zero;
            instantiatedReleasedSpellFX.transform.localRotation = Quaternion.identity;
            instantiatedReleasedSpellFX.transform.parent = null;

            FireBallManager fireBallManager = instantiatedReleasedSpellFX.GetComponent<FireBallManager>();  
            fireBallManager.InitialzeFireBall(player);

            // Get Any Colliders From The Caster
            //Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
            //Collider characterCollisionCollider = player.GetComponent<Collider>();

            /*Physics.IgnoreCollision(characterCollisionCollider, fireBallManager.damageCollider.damageCollider, true);

            foreach (var collder in characterColliders)
            {
                Physics.IgnoreCollision(characterCollisionCollider, fireBallManager.damageCollider.damageCollider, true);
            }*/

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                instantiatedReleasedSpellFX.transform.LookAt(player.playerCombatManager.currentTarget.transform.position );
            }
            else
            {
                Vector3 forwardDirection = player.transform.forward;
                instantiatedReleasedSpellFX.transform.forward = forwardDirection;
            }

            Rigidbody spellRigidbody = instantiatedReleasedSpellFX.GetComponent<Rigidbody>();
            Vector3 upwVelocityVector = instantiatedReleasedSpellFX.transform.up * upwardVelocity;
            Vector3 forwardVelocityVector = instantiatedReleasedSpellFX.transform.forward * forwardVelocity;
            Vector3 totalVelocity = upwVelocityVector + forwardVelocityVector;
            spellRigidbody.velocity = totalVelocity;
        }

        public override void SuccessfullyCastSpellFullCharge(PlayerManager player)
        {
            base.SuccessfullyCastSpellFullCharge(player);

            // Destroy Any Warp Up FX Remaining From The Spell
            if (player.IsOwner)
            {
                player.playerCombatManager.DestroyAllCurrentActionFX();
            }

            // Instantiate The Projectile
            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiatedReleasedSpellFX = Instantiate(spellCastReleaseFXFullCharge);

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                spellInstantiationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            else
            {
                spellInstantiationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }

            // Use the list of colliders from the caster and now apply the ignore physics with the colliders from the projectile
            instantiatedReleasedSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiatedReleasedSpellFX.transform.localPosition = Vector3.zero;
            instantiatedReleasedSpellFX.transform.localRotation = Quaternion.identity;
            instantiatedReleasedSpellFX.transform.parent = null;

            FireBallManager fireBallManager = instantiatedReleasedSpellFX.GetComponent<FireBallManager>();
            fireBallManager.isFullyCharged = true;
            fireBallManager.InitialzeFireBall(player);

            // Get Any Colliders From The Caster
            //Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
            //Collider characterCollisionCollider = player.GetComponent<Collider>();

            /*Physics.IgnoreCollision(characterCollisionCollider, fireBallManager.damageCollider.damageCollider, true);

            foreach (var collder in characterColliders)
            {
                Physics.IgnoreCollision(characterCollisionCollider, fireBallManager.damageCollider.damageCollider, true);
            }*/

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                instantiatedReleasedSpellFX.transform.LookAt(player.playerCombatManager.currentTarget.transform.position);
            }
            else
            {
                Vector3 forwardDirection = player.transform.forward;
                instantiatedReleasedSpellFX.transform.forward = forwardDirection;
            }

            Rigidbody spellRigidbody = instantiatedReleasedSpellFX.GetComponent<Rigidbody>();
            Vector3 upwVelocityVector = instantiatedReleasedSpellFX.transform.up * upwardVelocity;
            Vector3 forwardVelocityVector = instantiatedReleasedSpellFX.transform.forward * forwardVelocity;
            Vector3 totalVelocity = upwVelocityVector + forwardVelocityVector;
            spellRigidbody.velocity = totalVelocity;
        }

        public override bool CanICastThisSpell(PlayerManager player)
        {
            if (player.isPerformingAction) { return false; }
            if (player.playerNetworkManager.isJumping.Value) { return false; }
            if (player.playerNetworkManager.currentStamina.Value <= 0) { return false; }

            return true;
        }
    }

}
