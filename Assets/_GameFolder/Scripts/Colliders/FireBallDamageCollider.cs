using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class FireBallDamageCollider : SpellProjectileDamageCollider
    {
        private FireBallManager fireBallManager;
        protected override void Awake()
        {
            base.Awake();
            fireBallManager = GetComponentInParent<FireBallManager>();
        }
        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                if (damageTarget == spellCaster) { return; } // Prevent self damage

                if (!WorldUtilityManager.Instance.CanIDamageThisTarget(spellCaster.characterGroup, damageTarget.characterGroup)) { return; }

                CheckForParry(damageTarget);
                CheckForBlock(damageTarget);

                if (!damageTarget.characterNetworkManager.isInvulnerable.Value)
                {
                    DamageTarget(damageTarget);
                }

                fireBallManager.WaitThenInstatiateSpellDestructionFX(0.4f);
            }
        }
        protected override void CheckForParry(CharacterManager damageTarget)
        {
         
        }
        protected override void GetBlockingDotValues(CharacterManager damageTarget)
        {
            directionFromAttackToDamageTarget = spellCaster.transform.position - damageTarget.transform.position;
            dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward); // Facing In the correct Direction
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget)) { return; }

            charactersDamaged.Add(damageTarget);
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.contactPoint = contactPoint;
            damageEffect.angleHitFrom = Vector3.SignedAngle(spellCaster.transform.forward, damageTarget.transform.forward, Vector3.up);

  

            if (spellCaster.IsOwner)
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId, spellCaster.NetworkObjectId,
                    damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage,
                    damageEffect.holyDamage, damageEffect.poiseDamage,
                    damageEffect.angleHitFrom,
                    damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z);
            }
        }
    }
}
