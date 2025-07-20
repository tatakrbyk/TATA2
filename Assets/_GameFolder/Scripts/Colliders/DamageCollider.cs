using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace XD
{

    public class DamageCollider : MonoBehaviour
    {
        [SerializeField] protected Collider damageCollider;

        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("Poise")]
        public float poiseDamage = 0;

        protected Vector3 contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        [Header("Blocks")]
        protected Vector3 directionFromAttackToDamageTarget;
        protected float dotValueFromAttackToDamageTarget; // Facing In the correct Direction
        protected virtual void Awake()
        {
           
        }
        // NOTE(T): 
        // Damage Collider layer only trigger Character layer
        protected virtual void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                CheckForBlock(damageTarget);
                DamageTarget(damageTarget);
            }
        }

        protected virtual void CheckForBlock(CharacterManager damageTarget)
        {
            // If this character has already been damaged, do not proceed  
            if(charactersDamaged.Contains(damageTarget)) { return; }

            GetBlockingDotValues(damageTarget);

            if (damageTarget.characterNetworkManager.isBlocking.Value && dotValueFromAttackToDamageTarget > 0.3f)
            {
                
                charactersDamaged.Add(damageTarget);
                TakeBlockedDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeBlockedDamageEffect);

                damageEffect.physicalDamage = physicalDamage;
                damageEffect.magicDamage = magicDamage;
                damageEffect.fireDamage = fireDamage;
                damageEffect.lightningDamage = lightningDamage;
                damageEffect.holyDamage = holyDamage;
                damageEffect.poiseDamage = poiseDamage;
                damageEffect.staminaDamage = poiseDamage;
                damageEffect.contactPoint = contactPoint;
                
                damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
            }
        }
        protected virtual void GetBlockingDotValues(CharacterManager damageTarget)
        {
            directionFromAttackToDamageTarget = transform.position - damageTarget.transform.position;
            dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward); // Facing In the correct Direction
        }
        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            if(charactersDamaged.Contains(damageTarget)){ return; }

            charactersDamaged.Add(damageTarget);
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

        }

        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            charactersDamaged.Clear();
        }

    }

}