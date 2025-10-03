using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    /* What does this script do?
     * 
     * - This script server as a central hub to manipulate and adjust the fireball spell once its active, doing things such as 
     * -> Making the spell slightly "curve" or "follow" its lock targets as they are moving
     * -> Assigning damage neatly with a function from this script
     * -> Enabling/Disabling VFX and SFX, such as "contact" explosions, trailers,
    */
    public class FireBallManager : SpellManager
    {
        [Header("Colliders")]
        public FireBallDamageCollider damageCollider;

        [Header("Instantiated FX")]
        private GameObject instantiatedDestructionFX;
        
        private bool hasCollided = false;
        public bool isFullyCharged = false;
        private Rigidbody fireBallRigidbody;
        private Coroutine destructionFXCoroutine;

        protected override void Awake()
        {
            base.Awake();
            fireBallRigidbody = GetComponent<Rigidbody>();
        }

        protected override void Update()
        {
            base.Update();

            if(spellTarget != null )
            {
                transform.LookAt(spellTarget.transform);
            }

            if(fireBallRigidbody != null )
            {
                Vector3 currentVelocity = fireBallRigidbody.velocity;
                fireBallRigidbody.velocity = transform.forward + currentVelocity;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Ignore Character
            if (collision.gameObject.layer  == 6) { return; }

            if(!hasCollided)
            {
                hasCollided = true;
                InstantiateSpellDestructionFX();
                
            }
        }

        public void InitialzeFireBall(CharacterManager spellCaster)
        {
            damageCollider.spellCaster = spellCaster;

            // TODO: Set Up Damage Formula
            damageCollider.fireDamage = 150;
            
            if(isFullyCharged)
            {
                damageCollider.fireDamage *= 1.4f;
            }
        }

        public void InstantiateSpellDestructionFX()
        {
            if(isFullyCharged)
            {
                instantiatedDestructionFX = Instantiate(impactParticleFullCharge, transform.position, Quaternion.identity);
            }
            else
            {
                instantiatedDestructionFX = Instantiate(impactParticle, transform.position, Quaternion.identity);               
            }
            Destroy(gameObject);
        }

        public void WaitThenInstatiateSpellDestructionFX(float timeToWait)
        {
            if(destructionFXCoroutine != null)
            {
                StopCoroutine(destructionFXCoroutine);
            }

            destructionFXCoroutine = StartCoroutine(WaitThenInstantiateFX(timeToWait));
            StartCoroutine(WaitThenInstantiateFX(timeToWait));
        }

        private IEnumerator WaitThenInstantiateFX(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            InstantiateSpellDestructionFX();
        }
    }
}
