using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class AIDurkSoundFXManager : CharacterSoundFXManager
    {
        [Header("Club Whooses")]
        public AudioClip[] clubWhooshes;

        [Header("Club Impacts")]
        public AudioClip[] clubImpacts;

        [Header("Stomp Impacts")]
        public AudioClip[] stompImpacts;

        // Call Durk_Attack_01
        public virtual void PlayClubImpactSoundFX()
        {
            if(clubImpacts.Length > 0)
            {
                PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(clubImpacts));
            }
        }

        public virtual void PlayStompImpactSoundFX()
        {
            if (stompImpacts.Length > 0)
            {
                PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(stompImpacts));
            }
        }
    }

}
