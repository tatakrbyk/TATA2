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

        private void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }
    }
}
    
