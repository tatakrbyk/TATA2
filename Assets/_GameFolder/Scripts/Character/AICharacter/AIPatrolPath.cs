using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class AIPatrolPath : MonoBehaviour
    {
        public int patrolPathID = 0;
        public List<Vector3> patrolWaypoints = new List<Vector3>();

        private void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                patrolWaypoints.Add(transform.GetChild(i).position);
            }

            WorldAIManager.Instance.AddPatrolPathToList(this);
        }
    }
}
