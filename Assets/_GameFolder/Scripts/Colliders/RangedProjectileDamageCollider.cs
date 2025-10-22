using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class RangedProjectileDamageCollider : DamageCollider
    {
        [Header("Marksmen")]
        public CharacterManager characterShootingProjectile;

        [Header("Collision")]        
        private bool hasPenetratedSurface = false;
        public Rigidbody rigidbody;
        private CapsuleCollider capsuleCollider;
        protected override void Awake()
        {
            base.Awake();

            rigidbody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
        }

        private void FixedUpdate()
        {
            if(rigidbody.velocity != Vector3.zero)
            {
                rigidbody.rotation = Quaternion.LookRotation(rigidbody.velocity);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            CreatePenetrationIntoObject(collision);

            CharacterManager potentialTarget = collision.transform.gameObject.GetComponent<CharacterManager>(); 

            // TODO: Check For Shield Object and Perform Block

            // TODO: Instantiate impact particle and perform arrow

            if(characterShootingProjectile == null) { return; }

            Collider contactCollider = collision.gameObject.GetComponent<Collider>();
            if (contactCollider != null)
            {
                contactPoint = contactCollider.ClosestPointOnBounds(transform.position);
            }

            if (potentialTarget == null) { return; }

            if(WorldUtilityManager.Instance.CanIDamageThisTarget(characterShootingProjectile.characterGroup, potentialTarget.characterGroup))
            {
                CheckForBlock(potentialTarget);    
                DamageTarget(potentialTarget);
            }
            
        }

        protected override void CheckForBlock(CharacterManager damageTarget)
        {
            if(charactersDamaged.Contains(damageTarget)) { return; }

            float angle = Vector3.Angle(damageTarget.transform.forward, transform.forward);

            if (damageTarget.characterNetworkManager.isBlocking.Value && angle > 145)
            {
                charactersDamaged.Add(damageTarget);
                TakeBlockedDamageEffect blockedDamageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeBlockedDamageEffect);

                if(characterShootingProjectile != null)
                {
                    blockedDamageEffect.characterCausingDamage = characterShootingProjectile;
                }
                
                blockedDamageEffect.physicalDamage = physicalDamage;
                blockedDamageEffect.magicDamage = magicDamage;
                blockedDamageEffect.fireDamage = fireDamage;
                blockedDamageEffect.holyDamage = holyDamage;
                blockedDamageEffect.poiseDamage = poiseDamage;
                blockedDamageEffect.staminaDamage = poiseDamage;
                blockedDamageEffect.contactPoint = contactPoint;
            }
        }
        private void CreatePenetrationIntoObject(Collision hit)
        {
            if(!hasPenetratedSurface)
            {
                hasPenetratedSurface = true;

                // Get The Contact Point
                gameObject.transform.position = hit.GetContact(0).point;

                // Stops Our Arrow from "scaliing" in size with scaled up or down objects
                var emptyObject = new GameObject();
                emptyObject.transform.parent = hit.collider.transform;
                emptyObject.transform.SetParent(emptyObject.transform, true);

                // How far the arrow penetrates into the surface
                transform.position += transform.forward * (Random.Range(0.1f, 0.3f));

                // Disable Physics and Colliders
                rigidbody.isKinematic = true;
                capsuleCollider.enabled = false;

                // Destroy Damage Collider and Destroy arrow after a time
                Destroy(GetComponent<RangedProjectileDamageCollider>());
                Destroy(gameObject, 12);
            }
        }

    }
}
