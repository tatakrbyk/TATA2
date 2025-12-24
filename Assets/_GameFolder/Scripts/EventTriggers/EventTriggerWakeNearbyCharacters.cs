using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace XD
{
    // create Gameobject Sphere and assign this script
    // wake inside spheere Trigger is by player 

    // IsTrigger = true;
    // Layer = EventTrigger
    public class EventTriggerWakeNearbyCharacters : MonoBehaviour
    {
        [SerializeField] float awakenRadius = 8f;
        private void OnTriggerEnter(Collider other)
        {
            if(!NetworkManager.Singleton.IsServer) return;
            
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null) return;

            Collider[] creaturesInRadius = Physics.OverlapSphere(transform.position, awakenRadius, WorldUtilityManager.Instance.GetCharacterLayer());
            List<AICharacterManager> creaturesToWake = new List<AICharacterManager>();

            for(int i = 0; i < creaturesInRadius.Length; i++)
            {
                AICharacterManager aiCharacter = creaturesInRadius[i].GetComponent<AICharacterManager>();

                if (aiCharacter == null) continue;
                if(aiCharacter.isDead.Value) continue;
                if(aiCharacter.aiCharacterNetworkManager.isAwake.Value) continue;
                if(!creaturesToWake.Contains(aiCharacter))
                {
                    creaturesToWake.Add(aiCharacter);
                }
            }

            for (int i = 0; i < creaturesToWake.Count; i++)
            {
                
                creaturesToWake[i].aiCharacterCombatManager.SetTarget(player);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, awakenRadius);
        }
    }
}

