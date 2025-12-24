using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class BeaconDetector : MonoBehaviour
    {
        public  PlayerManager player;

        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void OnTriggerExit(Collider other)
        {
            AICharacterManager aICharacter = other.GetComponent<AICharacterManager>();

            if(aICharacter != null)
            {
                aICharacter.DeactivateCharacter(player);
            }
        }
    }
}
