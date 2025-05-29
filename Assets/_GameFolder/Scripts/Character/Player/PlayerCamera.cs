using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerCamera : MonoBehaviour
    {
        private static PlayerCamera instance; public static PlayerCamera Instance { get { return instance; } }

        public Camera cameraObject;
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
    }

}
