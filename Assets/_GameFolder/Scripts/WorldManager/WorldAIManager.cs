using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private List<GameObject> spawnedInCharacters;
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
