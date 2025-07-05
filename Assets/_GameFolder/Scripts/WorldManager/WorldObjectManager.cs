using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace XD
{ 
    public class WorldObjectManager : MonoBehaviour
    {
        private static WorldObjectManager instance; public static WorldObjectManager Instance { get { return instance; } }
        
        [Header("Network Objects")]
        [SerializeField] private List<NetworkObjectSpawner> networkObjectSpawners;
        [SerializeField] private List<GameObject> spawnedInObjects;

        [Header("Fog Walls")]
        public List<FogWallInteractable> fogWalls;

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
        
        public void SpawnObject(NetworkObjectSpawner networkObjectSpawner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                networkObjectSpawners.Add(networkObjectSpawner);
                networkObjectSpawner.AttemptToSpawnCharacter();

            }
        }

        public void AddFogWallToList(FogWallInteractable fogWall)
        {
            if (!fogWalls.Contains(fogWall))
            {
                fogWalls.Add(fogWall);
            }
        }

        public void RemoveFogWallToList(FogWallInteractable fogWall)
        {
            if(fogWalls.Contains(fogWall))
            {
                fogWalls.Remove(fogWall);
            }
        }
    }
}