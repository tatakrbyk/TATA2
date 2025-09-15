using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Stamine Regeneration")]
        [SerializeField] float staminaRegenerationAmount = 2;
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] float staminaRegenerationDelay = 2;

        [Header("Blocking Absorptions")]
        public float blockingPhysicalAbsorption;
        public float blockingFireAbsorption;
        public float blockingMagicAbsorption;
        public float blockingLightningAbsorption;
        public float blockingHolyAbsorption;
        public float blockingStability;

        [Header("Armor Absorptions")]

        public float armorPhysicalDamageAbsorption;
        public float armorMagicDamageAbsorption;
        public float armorFireDamageAbsorption;
        public float armorLightningDamageAbsorption;
        public float armorHolyDamageAbsorption;

        [Header("Armor Resistances")] 
        public float armorImmunity;          // Resistance To Rot and Poison
        public float armorRobustness;        // Resistance To Bleed and Frostbite
        public float armorFocus;             // Resistance To Sleep and Madness
        public float armorVitality;          // Resistance To Death Curse

        [Header("Poise")]
        public float totalPoisedDamage;             // How much poise damage we have taken
        public float offensivePoiseBonus;           // gaine from using items (weapons, etc)
        public float basePoiseDefense;              // gained from using items (armor, etc )
        public float defaultPoiseResetTime = 8;     // The time it takes for poise damage to reset( must not be hit in the time or it will reset)
        public float poiseResetTimer = 0;           // Timer to reset poise damage
        protected virtual void Awake()      
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            HandlePoiseResetTimer();    
        }
        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            int health = 0;

            health = vitality * 15;
            return Mathf.RoundToInt(health);
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            int stamina = 0;

            stamina = endurance * 10;
            return Mathf.RoundToInt(stamina);
        }

        public virtual void RegenerateStamina()
        {
            if (!character.IsOwner) { return; }
            if (character.characterNetworkManager.isSprinting.Value) { return; }
            if (character.isPerformingAction) { return; }

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if(character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;

                    if(staminaTickTimer >= 0.1)
                    {
                        staminaTickTimer = 0;
                        character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    }
                }
            }
        }

        public virtual void ResetStaminaRegeneraationTimer(float prevStaminaAmount, float currentStaminaAmount)
        {
            // Used stamina
            if(currentStaminaAmount < prevStaminaAmount)
            {
                staminaRegenerationTimer = 0;
            }
        }

        public virtual void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer -= Time.deltaTime;
            }
            else
            {
                totalPoisedDamage = 0;
            }
        }
    }

}
