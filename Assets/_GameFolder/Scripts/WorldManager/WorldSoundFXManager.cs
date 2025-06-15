using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        private static WorldSoundFXManager instance; public static WorldSoundFXManager Instance { get { return instance; } }

        [Header("Damage Sounds")]
        public AudioClip[] physicalDamageSFX;

        [Header("Actions Sounds")]
        public AudioClip rollSFX;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            int index = Random.Range(0, array.Length);
            return array[index];
        }
    }
}
