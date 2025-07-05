using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class DurkStompCollider : DamageCollider
    {
        AIDurkCharacterManager durkCharacterManager;

        protected override void Awake()
        {
            base.Awake();
            durkCharacterManager = GetComponentInParent<AIDurkCharacterManager>();
            
        }
        public void StompAttack()
        {
            GameObject stompVFX = Instantiate(durkCharacterManager.durkCombatManager.durkImpactVFX, transform);

            Collider[] colliders = Physics.OverlapSphere(transform.position, durkCharacterManager.durkCombatManager.stompAttackAreaOfEffectRadius, WorldUtilityManager.Instance.GetCharacterLayer());
            List<CharacterManager> charactersDamaged = new List<CharacterManager>();

            foreach (Collider collider in colliders)
            {
                CharacterManager character = collider.GetComponentInParent<CharacterManager>();

                if (character != null)
                {
                    if (charactersDamaged.Contains(character)) { continue; }
                    if(character == durkCharacterManager) { continue; } // Don't damage myself
                    charactersDamaged.Add(character);

                    if (character.IsOwner)
                    {
                        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeDamageEffect);
                        damageEffect.physicalDamage = durkCharacterManager.durkCombatManager.stompDamage;
                        damageEffect.poiseDamage = durkCharacterManager.durkCombatManager.stompDamage;


                        character.characterEffectsManager.ProcessInstantEffect(damageEffect);
                    }
                }
            }
        }
    
    }

}
