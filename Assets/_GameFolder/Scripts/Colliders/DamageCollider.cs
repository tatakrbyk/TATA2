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

        protected Vector3 contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

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


                DamageTarget(damageTarget);
            }
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