using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class AICharacterLocomotionManager : CharacterLocomotionManager
    {
        public void RotateTowardsAgent(AICharacterManager aICharacter)
        {
            if(aICharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                aICharacter.transform.rotation = aICharacter.navMeshAgent.transform.rotation;
            }
        }
    }

}
