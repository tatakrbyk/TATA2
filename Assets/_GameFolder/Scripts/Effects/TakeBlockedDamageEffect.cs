using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/ Take Blocked Damage", fileName = "TakeBlockedDamageEffect")]

    public class TakeBlockedDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage;    // If the damage is caused by another character's attack it will be stored here

        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("Final Damage")]
        private int finalDamageDealt = 0;

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;                 // TODO(T): if is a true play "Stunned' animation

        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX;            // magic/fire/lightning/holy

        [Header("Direction Damage Taken From")]
        public float angleHitFrom;                          // Used to determine what damage animation to play (Move backward, to th left, to the right)
        public Vector3 contactPoint;

        [Header("Stamina")]
        public float staminaDamage = 0;
        public float finalStaminaDamage = 0;

        public override void ProcessEffect(CharacterManager character)
        {
            Debug.Log("Hi");
            if (character.characterNetworkManager.isInvulnerable.Value) { return; }

            base.ProcessEffect(character);

            if (character.isDead.Value) { return; }

            Debug.Log("Processing Blocked Damage Effect");
            CalculateDamage(character);
            CalculateStaminaDamage(character); 

            PlayDirectionalBasedBlockingDamageAnimation(character);
            // Check for build ups (poison, etc)
            PlayDamageSFX(character);
            PlayDamageVFX(character);

            CheckForGuardBreak(character);
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner) { return; }


            if (characterCausingDamage != null)
            {

            }

            // Check character defensive properties (armor, shield, etc) to reduce damage

            Debug.Log("Calculating oRÝGÝNAL Blocked Damage: "+ physicalDamage);
            physicalDamage -= (physicalDamage * (character.characterStatsManager.blockingPhysicalAbsorption / 100));
            magicDamage -= (magicDamage * (character.characterStatsManager.blockingMagicAbsorption / 100));
            fireDamage -= (fireDamage * (character.characterStatsManager.blockingFireAbsorption / 100));
            lightningDamage -= (lightningDamage * (character.characterStatsManager.blockingLightningAbsorption / 100));
            holyDamage -= (holyDamage * (character.characterStatsManager.blockingHolyAbsorption / 100)); 
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);


            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }
            Debug.Log("Calculating Blocked Damage: " + physicalDamage);

            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

            // calculate poise damage for stunned 
        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            if(!character.IsOwner) { return; }

            finalStaminaDamage = staminaDamage;

            float staminaDamageAbsorption =  finalStaminaDamage * (character.characterStatsManager.blockingStability / 100);
            float staminaDamageAfterAbsorption = finalStaminaDamage - staminaDamageAbsorption;

            character.characterNetworkManager.currentStamina.Value -= staminaDamageAfterAbsorption;
        }

        private void CheckForGuardBreak(CharacterManager character)
        {
            if(!character.IsOwner) { return; }

            if(character.characterNetworkManager.currentStamina.Value <= 0)
            {
                character.characterAnimatorManager.PlayActionAnimation("Guard_Break_01", true);
                character.characterNetworkManager.isBlocking.Value = false;
            }
        }
        private void PlayDamageVFX(CharacterManager character)
        {
            character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }

        private void PlayDamageSFX(CharacterManager character)
        {
            character.characterSoundFXManager.PlayBlockSoundFX();
        }

        // Hit React Animations
        private void PlayDirectionalBasedBlockingDamageAnimation(CharacterManager character)
        {
            if (!character.IsOwner) { return; }
            if (character.isDead.Value) { return; }

            DamageIntensity damageIntensity = WorldUtilityManager.Instance.GetDamageIntensityBasedOnPoiseDamage(poiseDamage);

            //TODo: cHECK FOR TWO HAND STATUS
            switch(damageIntensity)
            {
                case DamageIntensity.Ping:
                    damageAnimation = "Block_Ping_01";
                    break;
                case DamageIntensity.Light:
                    damageAnimation = "Block_Light_01";
                    break;
                case DamageIntensity.Medium:
                    damageAnimation = "Block_Medium_01";
                    break;
                case DamageIntensity.Heavy:
                    damageAnimation = "Block_Heavy_01";
                    break;
                case DamageIntensity.Colossal:
                    damageAnimation = "Block_Colossal_01";
                    break;
                default:
                    break;

            }
            
            character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
            character.characterAnimatorManager.PlayActionAnimation(damageAnimation, true);
        }
    }

}
