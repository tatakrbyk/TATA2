using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        
        private AudioSource audioSource;
        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Assign Roll Animation Events
        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.Instance.rollSFX);
        }
    }

}
