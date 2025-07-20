using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {   
        private static WorldCharacterEffectsManager instance; public static WorldCharacterEffectsManager Instance { get { return instance; } }

        [Header("VFX")]
        public GameObject bloodSplatterVFX;

        [Header("Damage")]
        public TakeDamageEffect takeDamageEffect;
        public TakeBlockedDamageEffect takeBlockedDamageEffect;

        [Header("Two Hand")]
        public TwoHandingEffect twoHandingEffect;
        [Header("Instant Effects")]
        [SerializeField] List<InstantCharacterEffect> instantEffects;

        [Header("Static Effects")]
        [SerializeField] List<StaticCharacterEffect> staticEffects;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject); 
            }

            GenerateEffectIDs();
        }

        private void GenerateEffectIDs()
        {
            for(int i = 0; i < instantEffects.Count; i++)
            {
                instantEffects[i].instantEffectID = i;
            }
            for (int i = 0; i < staticEffects.Count; i++)
            {
                staticEffects[i].staticEffectID = i;
            }
        }
    }
}
