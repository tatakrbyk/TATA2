using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class Utility_DestroyAfterTime : MonoBehaviour
    {
        private float timeUntilDestroy = 5f;

        private void Awake()
        {
            Destroy(gameObject, timeUntilDestroy);
        }
    }

}

