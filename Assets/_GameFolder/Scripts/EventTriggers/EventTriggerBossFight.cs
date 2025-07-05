using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class EventTriggerBossFight : MonoBehaviour
    {
        [SerializeField] private int bossID;

        private void OnTriggerEnter(Collider other)
        {
            AIBossCharacterManager boss = WorldAIManager.Instance.GetBossCharacterByID(bossID);
            if (boss != null)
            {
                boss.WakeBoss();
            }
        }   
    }
}
