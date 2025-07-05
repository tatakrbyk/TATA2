using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XD
{
    public class WorldAIManager : MonoBehaviour
    {
        private static WorldAIManager instance; public static WorldAIManager Instance { get { return instance; } }

        [Header("Characters")]
        [SerializeField] private List<AICharacterSpawner> aiCharacterSpawners;
        [SerializeField] private List<AICharacterManager> spawnedInCharacters;

        [Header("Bosses")]
        [SerializeField] private List<AIBossCharacterManager> spawnedInBosses;

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
        public void DeSpawnCharacter()
        {
            foreach (var character in spawnedInCharacters)
            {
                character.GetComponent<NetworkObject>().Despawn();
                spawnedInCharacters.Remove(character);
            }
        }

        private void DisableAllCharacters()
        {  }
    }
}
