using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XD
{
    public class WorldAIManager : MonoBehaviour
    {
        private static WorldAIManager instance; public static WorldAIManager Instance { get { return instance; } }

        [Header("Loading")]
        public bool isPerformingLoadingOperation = false;

        [Header("Characters")]
        [SerializeField] private List<AICharacterSpawner> aiCharacterSpawners;
        [SerializeField] private List<AICharacterManager> spawnedInCharacters;

        private Coroutine spawnAllCharactersCoroutine;
        private Coroutine deSpawnAllCharactersCoroutine;
        private Coroutine resetAllCharactersCoroutine;

        [Header("Beacon Prefab")]
        public GameObject beaconGameobejct;

        [Header("Bosses")]
        [SerializeField] private List<AIBossCharacterManager> spawnedInBosses;

        [Header("Patrol Paths")]
        [SerializeField] List<AIPatrolPath> aiPatrolPaths = new List<AIPatrolPath>();
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
        }

        public void SpawnCharacter(AICharacterSpawner aiCharacterSpawner)
        {
            if(NetworkManager.Singleton.IsServer)
            {
                aiCharacterSpawners.Add(aiCharacterSpawner);
                aiCharacterSpawner.AttemptToSpawnCharacter();

            }
        }

        public void AddCharacterToSpawnedCharactersList(AICharacterManager character)
        {
            if(spawnedInCharacters.Contains(character)) {  return; }
            spawnedInCharacters.Add(character);

            AIBossCharacterManager bossCharacter = character as AIBossCharacterManager;

            if(bossCharacter != null)
            {
                if(spawnedInBosses.Contains(bossCharacter)) { return; }
                spawnedInBosses.Add(bossCharacter);
            }
        }

        public AIBossCharacterManager GetBossCharacterByID(int ID)
        {
            return spawnedInBosses.FirstOrDefault(boss => boss.bossID == ID);
        }

        public void SpawnallCharacters()
        {
            isPerformingLoadingOperation = true;
            
            if (spawnAllCharactersCoroutine != null)
            {
                StopCoroutine(spawnAllCharactersCoroutine);
            }
            spawnAllCharactersCoroutine = StartCoroutine(SpawnAllCharactersCoroutine());
        }

        private IEnumerator SpawnAllCharactersCoroutine()
        {
            for(int i = 0; i < aiCharacterSpawners.Count; i++)
            {
                yield return new WaitForFixedUpdate();
                aiCharacterSpawners[i].AttemptToSpawnCharacter();
                yield return null;
            }
           
            isPerformingLoadingOperation = false;
            yield return null;
        }

        public void ResetAllCharacterS()
        {
            isPerformingLoadingOperation = true;
            if (resetAllCharactersCoroutine != null)
            {
                StopCoroutine(resetAllCharactersCoroutine);
            }
            resetAllCharactersCoroutine = StartCoroutine(ResetAllCharactersCoroutine());

        }
        private IEnumerator ResetAllCharactersCoroutine()
        {
            for (int i = 0; i < aiCharacterSpawners.Count; i++)
            {
                yield return new WaitForFixedUpdate();
                aiCharacterSpawners[i].ResetCharacter();
                yield return null;
            }
            isPerformingLoadingOperation = false;
            yield return null;
        }
        public void DeSpawnAllCharacter()
        {
            isPerformingLoadingOperation = true;

            if (deSpawnAllCharactersCoroutine != null)
            {
                StopCoroutine(deSpawnAllCharactersCoroutine);
            }
            deSpawnAllCharactersCoroutine = StartCoroutine(DeSpawnAllCharactersCoroutine());
        }
        private IEnumerator DeSpawnAllCharactersCoroutine()
        {
            for (int i = 0; i < spawnedInCharacters.Count; i++)
            {
                yield return new WaitForFixedUpdate();
                spawnedInCharacters[i].GetComponent<NetworkObject>().Despawn();
                yield return null;
            }
            spawnedInCharacters.Clear();
            isPerformingLoadingOperation = false;
            yield return null;
        }

        private void DisableAllCharacters()
        { 
        }

        // Patrol Paths

        public void AddPatrolPathToList(AIPatrolPath patrolPath)
        {
            if (aiPatrolPaths.Contains(patrolPath)) { return; }
            aiPatrolPaths.Add(patrolPath);
        }

        public AIPatrolPath GetAIPatrolPathByID(int patrolPathID)
        {
            AIPatrolPath patrolPath = null;

            for (int i = 0; i < aiPatrolPaths.Count; i++)
            {
                if (aiPatrolPaths[i].patrolPathID == patrolPathID)
                {
                    patrolPath =  aiPatrolPaths[i];
                }
            }

            return patrolPath;
        }
    }
}
