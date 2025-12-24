using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
namespace XD
{
    public class AICharacterSpawner : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] private GameObject characterGameObject;
        [SerializeField] private GameObject instantiatedGameObject;
        private AICharacterManager aiCharacter;

        [Header("Patrol")]
        [SerializeField] private bool hasPatrolPath = false;
        [SerializeField] private int patrolPathID= 0;

        [Header("Patrol")]
        [SerializeField] bool isSleeping = false;
        private void Awake()
        {
        }

        private void Start()
        {
            WorldAIManager.Instance.SpawnCharacter(this);
            gameObject.SetActive(false);
        }

        public void AttemptToSpawnCharacter()
        {
            if (characterGameObject != null)
            {
                instantiatedGameObject = Instantiate(characterGameObject);
                instantiatedGameObject.transform.position = transform.position;
                instantiatedGameObject.transform.rotation = transform.rotation;
                instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
                aiCharacter = instantiatedGameObject.GetComponent<AICharacterManager>();

                if (aiCharacter == null) return;

                WorldAIManager.Instance.AddCharacterToSpawnedCharactersList(aiCharacter);

                if (hasPatrolPath)
                {
                    aiCharacter.idle.patrolPath = WorldAIManager.Instance.GetAIPatrolPathByID(patrolPathID);
                }

                if (isSleeping)
                {
                    aiCharacter.aiCharacterNetworkManager.isAwake.Value = false;
                }

                aiCharacter.aiCharacterNetworkManager.isActive.Value = false;
            }
        }

        public void ResetCharacter()
        {
            if (instantiatedGameObject == null) return;
            if (aiCharacter == null) return;

            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            aiCharacter.aiCharacterNetworkManager.currentHealth.Value = aiCharacter.aiCharacterNetworkManager.maxHealth.Value;
            aiCharacter.aiCharacterCombatManager.SetTarget(null);
            if (aiCharacter.isDead.Value)
            {
                aiCharacter.isDead.Value = false;
                aiCharacter.characterAnimatorManager.PlayActionAnimation("Empty", false, false, true, true, true, true);
                aiCharacter.currentState.SwitchState(aiCharacter, aiCharacter.idle);
            }

            aiCharacter.characterUIManager.ResetCharacterHPBar();
        }
    }

}
