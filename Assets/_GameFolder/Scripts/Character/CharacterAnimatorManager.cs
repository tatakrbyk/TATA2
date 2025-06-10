using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

namespace XD
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int vertical;
        int horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting)
        {
            float localHorizontal = horizontalValue;
            float localVertical = verticalValue;
            if(isSprinting)
            {
                localVertical = 2;
            }
          
            character.animator.SetFloat(vertical, localVertical, 0.1f, Time.deltaTime);
            character.animator.SetFloat(horizontal, localHorizontal, 0.1f, Time.deltaTime);
        }

        public virtual void PlayActionAnimation(
            string animationName,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false )
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationName, 0.2f);
            // Can be used to stop character from attempting new actions 
            // This flag will turn true if you are stunned
            character.isPerformingAction = isPerformingAction; 
            character.canRotate = canRotate;
            character.canMove = canMove;

            // Animation Replicated
            character.characterNetworkManager.NotifyServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, animationName, applyRootMotion);
        }
    }

    

}
