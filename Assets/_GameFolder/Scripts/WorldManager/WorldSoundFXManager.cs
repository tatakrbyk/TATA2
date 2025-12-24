using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace XD
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        private static WorldSoundFXManager instance; public static WorldSoundFXManager Instance { get { return instance; } }

        [Header("Boss Track")]
        [SerializeField] private AudioSource bossIntroPlayer;
        [SerializeField] private AudioSource bossLoopPlayer;

        [Header("Damage Sounds")]
        public AudioClip[] physicalDamageSFX;

        [Header("Actions Sounds")]
        public AudioClip pickUpItemSFX;
        public AudioClip rollSFX;
        public AudioClip stanceBreakSFX;
        public AudioClip criticalStrikeSFX;
        public AudioClip[] releaseArrowSFX;
        public AudioClip[] notchArrowSFX;
        public AudioClip healingFlaskSFX;

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

        public void PlayBossTrack(AudioClip introTrack, AudioClip loopTrack)
        {
            bossIntroPlayer.volume = 1;
            bossIntroPlayer.clip = introTrack;
            bossIntroPlayer.loop = false;
            bossIntroPlayer.Play();

            bossLoopPlayer.volume = 1;
            bossLoopPlayer.clip = loopTrack;
            bossLoopPlayer.loop = true;
            bossLoopPlayer.PlayDelayed(bossIntroPlayer.clip.length);
        }
        /*public AudioClip ChooseRandomFootStepSoundBasedOmround(GameObject steppedOnObject, CharacterManager character)
        {
            if(steppedOnObject.tag == "Untagged")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footSteps);
            }
            else if((steppedOnObject.tag == "Snow")
            {
                return ChooseRandomSFXFromArray(character.characterSoundFXManager.footStepsSnow);
            }
            return null;
        }*/

        public void StopBossMusic()
        {
            StartCoroutine(FadeOutBossMusicThenStop());
        }

        private IEnumerator FadeOutBossMusicThenStop()
        {
            while(bossIntroPlayer.volume > 0)
            {
                bossIntroPlayer.volume -= Time.deltaTime;
                bossIntroPlayer.volume -= Time.deltaTime;
                yield return null;
            }

            bossIntroPlayer.Stop();
            bossLoopPlayer.Stop();
        }


        // AI Listen Sound using FUNC and switch state
        public void ALertNearbyCharactersToSound(Vector3 positionOfSound, float rangeOfSound)
        {
            if(!NetworkManager.Singleton.IsServer) { return; }

            Collider[] characterColliders = Physics.OverlapSphere(positionOfSound, rangeOfSound);

            List<AICharacterManager> charactersToAlert = new List<AICharacterManager>();

            for(int i = 0; i < characterColliders.Length; i++)
            {
                AICharacterManager aICharacter = characterColliders[i].GetComponent<AICharacterManager>();

                if (aICharacter == null) continue;
                if (charactersToAlert.Contains(aICharacter)) continue;

                charactersToAlert.Add(aICharacter);
            }
            for (int i = 0; i < charactersToAlert.Count; i++)
            {
                charactersToAlert[i].aiCharacterCombatManager.AlertCharacterToSound(positionOfSound);
            }
        }
    }
}
