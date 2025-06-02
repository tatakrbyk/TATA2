using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Ground & Jumping Check")]

        protected float gravityForce = -5.55f;
        [SerializeField] LayerMask groundLayer;
        private float groundCheckSphereRadius = 0.3f;
        protected Vector3 yVelocity; // The force at which our character is pulled up or down (jumping & Faaling)
        protected float groundedYVelocity = -20; // The force at which our character is sticking to the ground whilst they are grounded
        protected float fallStartVelocity = -5; //  The force at which our character is begins to fall when they become ungrounded ( rises as they fall longer)
        protected bool failingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();

            if(character.isGrounded)
            {
                 // If we are not attempting to jump or move upward
                if(yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    failingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {   // If we are not jumping, and our falling velocity has not been set 
                if(!character.isJumping && !failingVelocityHasBeenSet)
                {
                    failingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartVelocity;
                }
                inAirTimer += Time.deltaTime;
                character.animator.SetFloat("InAirTimer", inAirTimer);
                yVelocity.y += gravityForce * Time.deltaTime;
            }
            
            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }

}
