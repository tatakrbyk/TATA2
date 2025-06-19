using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class WorldUtilityManager : MonoBehaviour
    {
        private static WorldUtilityManager instance; public static WorldUtilityManager Instance { get { return instance; } }

        [SerializeField] private LayerMask characterLayer;
        [SerializeField] private LayerMask environmentLayers;
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

        public LayerMask GetCharacterLayer()
        {
            return characterLayer;
        }
        public LayerMask GetEnvironmentLayers()
        {
            return environmentLayers;
        }
    }

}
