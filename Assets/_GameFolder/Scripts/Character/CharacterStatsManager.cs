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
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {

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
    }

}
