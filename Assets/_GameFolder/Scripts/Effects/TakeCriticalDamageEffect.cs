using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Critical Damage Effect")]
    public class TakeCriticalDamageEffect : TakeDamageEffect
    {
        public override void ProcessEffect(CharacterManager character)
        {
            if (character.characterNetworkManager.isInvulnerable.Value) { return; }

            if (character.isDead.Value) { return; }


            CalculateDamage(character);

            character.characterCombatManager.pendingCriticalDamage = finalDamageDealt;
        }

        protected override void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner) { return; }

            if (characterCausingDamage != null)
            {
            }

            // Check character defensive properties (armor, shield, etc) to reduce damage
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            // calculate poise damage for stunned 
            character.characterStatsManager.totalPoisedDamage -= poiseDamage;

            character.characterCombatManager.previousPoiseDamageTaken = poiseDamage;

            float remainingPoise = character.characterStatsManager.basePoiseDefense +
                character.characterStatsManager.offensivePoiseBonus +
                character.characterStatsManager.totalPoisedDamage;

            if (remainingPoise <= 0)
            {
                poiseIsBroken = true;
            }

            // Since the character has been hit, we reset the poise reset timer
            character.characterStatsManager.poiseResetTimer = character.characterStatsManager.defaultPoiseResetTime;
        }
    }

}
