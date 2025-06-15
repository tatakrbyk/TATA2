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

        [Header("VFX")]
        [SerializeField] private GameObject bloodSplatterVFX;

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
    }
}
    
