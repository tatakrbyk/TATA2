using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class AICharacterAnimationManager : CharacterAnimatorManager
    {
        AICharacterManager aiCharacter;

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        private void OnAnimatorMove()
        {
            // HOST
            if(aiCharacter.IsOwner)
            {
                if(!aiCharacter.aICharacterLocomotionManager.isGrounded) { return; }

                Vector3 velocity = aiCharacter.animator.deltaPosition;

                aiCharacter.characterController.Move(velocity);
                aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
            }
            // CLIENT
            else
            {
                if (!aiCharacter.aICharacterLocomotionManager.isGrounded) { return; }

                Vector3 velocity = aiCharacter.animator.deltaPosition;

                aiCharacter.characterController.Move(velocity);
                aiCharacter.transform.position = Vector3.SmoothDamp(transform.position,
                    aiCharacter.characterNetworkManager.networkPosition.Value,
                    ref aiCharacter.characterNetworkManager.networkPositionVelocity,
                    aiCharacter.characterNetworkManager.networkPositionSmoothTime);
                aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
            }
        }
    }
}
