using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue /*,bool isSprinting*/)
        {
            character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
            character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
        }

        public virtual void PlayActionAnimation(
            string animationName,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false )
        { 
        
        }
    }

    

}
