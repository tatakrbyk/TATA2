using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        // Instant Effects (Damage, Healing, ...)
        // Timed Effects (Poison, Burn, ...)
        // static Effects (Buff, ...)

        CharacterManager character;

        [Header("Current Active FX")]
        public GameObject activeSpellWarmUpFX;
        public GameObject activeDrawnProjectileFX;

        [Header("VFX")]
        [SerializeField] private GameObject bloodSplatterVFX;
        [SerializeField] private GameObject criticalBloodSplatterVFX;

        [Header("Static Effects")]
        public List<StaticCharacterEffect> staticEffects = new List<StaticCharacterEffect>();
        private void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }

        public void PlayBloodSplatterVFX(Vector2 contactPoint)
        {
            if ((bloodSplatterVFX != null))
            {
                GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.Instance.bloodSplatterVFX, contactPoint, Quaternion.identity);

            }
        }

        public void PlayCriticalBloodSplatterVFX(Vector2 contactPoint)
        {
            if ((bloodSplatterVFX != null))
            {
                GameObject bloodSplatter = Instantiate(criticalBloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.Instance.criticalBloodSplatterVFX, contactPoint, Quaternion.identity);

            }
        }
        public void AddStaticEffect(StaticCharacterEffect effect)
        {
            staticEffects.Add(effect);
            effect.ProcessStaticEffect(character);

            // Remove null effects from the list
            for (int i = staticEffects.Count - 1; i > -1; i--)
            {
                if (staticEffects[i] == null)
                {
                    staticEffects.RemoveAt(i);
                }
            }
        }

        public void RemoveStaticEffect(int effectID)
        {
            StaticCharacterEffect effect;

            for(int i = 0; i < staticEffects.Count; i++)
            {
                if (staticEffects[i] != null)
                {
                    if(staticEffects[i].staticEffectID == effectID)
                    {
                        effect = staticEffects[i];
                        // Remove static effect from character
                        effect.RemoveStaticEffect(character);
                        // Remove effect from the list
                        staticEffects.Remove(effect);
                    }
                }
            }

            // Remove null effects from the list
            for (int i = staticEffects.Count - 1; i > -1; i--)
            {
                if (staticEffects[i] == null)
                {
                    staticEffects.RemoveAt(i);
                }
            }
        }
    }
}
    
