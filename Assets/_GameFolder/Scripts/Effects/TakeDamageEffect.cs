using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace XD
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/ TakeDamage")]
    public class TakeDamageEffect : InstantCharacterEffect
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

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if(character.isDead.Value) { return; }

            // Check gor invulnerability

            CalculateDamage(character);

            // Check which directional damage came from
            // Play a damage animation 
            // Check for build ups (poison, etc)
            // Play damage sound fx
            // Play damage vfx 

        }

        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner) { return; }


            if(characterCausingDamage != null)
            {

            }

            // Check character defensive properties (armor, shield, etc) to reduce damage

            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if(finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            Debug.Log("Fýnal Damage Given: " + finalDamageDealt);
            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

            // calculate poise damage for stunned 
        }

    }
}
