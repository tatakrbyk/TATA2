using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        
        private AudioSource audioSource;

        [Header("Damage Grunts")]
        [SerializeField] protected AudioClip[] damageGrunts;

        [Header("Attack Grunts")]
        [SerializeField] protected AudioClip[] attackGrunts;
        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            audioSource.PlayOneShot(soundFX);
            audioSource.pitch = 1; 

            if(randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom,  pitchRandom);
            }
        }

        // Assign Roll Animation Events
        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.Instance.rollSFX);
        }

        public virtual void PlayDamageGrunt()
        {
            PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(damageGrunts));
        }

        public virtual void PlayAttackGrunt()
        {
            PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(attackGrunts));

        }
    }

}
