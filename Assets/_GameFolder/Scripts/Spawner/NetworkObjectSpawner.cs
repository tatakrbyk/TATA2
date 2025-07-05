using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace XD
{
    public class NetworkObjectSpawner : MonoBehaviour
    {
        [Header("Object")]
        [SerializeField] private GameObject networkGameObject;
        [SerializeField] private GameObject instantiatedGameObject;

        private void Start()
        {
            WorldObjectManager.Instance.SpawnObject(this);
            gameObject.SetActive(false);
        }

        public void AttemptToSpawnCharacter()
        {
            if (networkGameObject != null)
            {
                instantiatedGameObject = Instantiate(networkGameObject);
                instantiatedGameObject.transform.position = transform.position;
                instantiatedGameObject.transform.rotation = transform.rotation;
                instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
            }
        }     
    }

}
