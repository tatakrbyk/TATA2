using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;

        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("Movement Settings")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        private float walkingSpeed = 1.5f;
        private float runningSpeed = 3.5f;
        private float sprintingSpeed = 7f;
        private float rotationSpeed = 15;
        private int sprintingStaminaCost = 1;

        [Header("Dodge")]     
        private Vector3 rollDirection;
        private float dodgeStaminaCost = 20;

        [Header("Jump")]
        private float jumpStaminaCost = 10;
        private float jumpHeight = 4;
        private float jumpForwardSpeed = 5;
        private float freeFallSpeed = 2;
        private Vector3 jumpDirection;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            if(player.IsOwner)
            {
                // Replicated
                player.characterNetworkManager.verticalMovement.Value = verticalMovement;
                player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                player.characterNetworkManager.moveAmount.Value = moveAmount;
            }
            else
            {
                verticalMovement = player.characterNetworkManager.verticalMovement.Value;
                horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
                moveAmount = player.characterNetworkManager.moveAmount.Value;

                // If not locked on,    
                if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
                {
                    player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
                }
                else
                {
                    player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalMovement, verticalMovement, player.playerNetworkManager.isSprinting.Value);
                }
            }
        }

        // Call PlayerManager
        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMovement();
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.Instance.verticalInput;
            horizontalMovement = PlayerInputManager.Instance.horizontalInput;  
            moveAmount = PlayerInputManager.Instance.moveAmount;
            
        }
        private void HandleGroundedMovement()
        {
            if(player.playerLocomotionManager.canMove || player.playerLocomotionManager.canRotate) 
            {
                GetMovementValues();
            }

            if(!player.characterLocomotionManager.canMove) { return; }


            moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement; 
            moveDirection = moveDirection + PlayerCamera.Instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if(player.playerNetworkManager.isSprinting.Value)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);

            }
            else
            {
                if (PlayerInputManager.Instance.moveAmount > 0.5f)
                {
                    // Move at a running speed
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
                {
                    // Move at a walking speed
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);

                }
            }

        }

        private void HandleJumpingMovement()
        {
            if(player.playerNetworkManager.isJumping.Value)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }
        private void HandleFreeFallMovement()
        {
            if(!player.playerLocomotionManager.isGrounded)
            {
                Vector3 freefallDirection;

                freefallDirection = PlayerCamera.Instance.transform.forward * PlayerInputManager.Instance.verticalInput;
                freefallDirection += PlayerCamera.Instance.transform.right * PlayerInputManager.Instance.horizontalInput;
                freefallDirection.y  = 0;

                player.characterController.Move(freefallDirection * freeFallSpeed * Time.deltaTime);    
            }
        }

        private void HandleRotation()
        {
            if(player.isDead.Value) { return; }
            if(!player.playerLocomotionManager.canRotate) { return; }

            if(player.playerNetworkManager.isLockedOn.Value)
            {
                if(player.playerNetworkManager.isSprinting.Value || player.playerLocomotionManager.isRolling)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
                    targetDirection += PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if(targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
                else
                {
                    if (player.playerCombatManager.currentTarget == null) { return; }

                    Vector3 targetDirection;    
                    targetDirection = player.playerCombatManager.currentTarget.transform.position - transform.position;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
            }
            else
            {
                targetRotationDirection = Vector3.zero;
                targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
                targetRotationDirection = targetRotationDirection + PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
                targetRotationDirection.Normalize();
                targetRotationDirection.y = 0;

                if(targetRotationDirection == Vector3.zero )
                {
                    targetRotationDirection = transform.forward;
                }

                Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
                Quaternion targetRotation = Quaternion.Slerp( transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = targetRotation;

            }
        }

        public void HandleSprinting()
        {
            if(player.isPerformingAction)
            {
                player.playerNetworkManager.isSprinting.Value = false;          
            }

            if (player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            if (moveAmount >= 0.5)
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }
            // If we are stationary/Moving Slowly sprinting is false
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
            
            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }
        public void AttemptToPerformDodge()
        {
            if(player.isPerformingAction) {  return; }  
            if(player.playerNetworkManager.currentStamina.Value <= 0) { return; }

            // if we are moving when we attempt to dodge, we perform a roll
            if (PlayerInputManager.Instance.moveAmount > 0 )
            {
                rollDirection = PlayerCamera.Instance.cameraObject.transform.forward * PlayerInputManager.Instance.verticalInput;
                rollDirection += PlayerCamera.Instance.cameraObject.transform.right * PlayerInputManager.Instance.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                // Roll Animation
                player.playerAnimatorManager.PlayActionAnimation("Roll_Forward_01", true, true);
                player.playerLocomotionManager.isRolling = true;
            }
            // If we are stationary, we perform a backstep
            else
            {
                // Backstep Animation
                player.playerAnimatorManager.PlayActionAnimation("Back_Step_01", true, true);

            }

            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;

        }

        public void AttemptToPerformJump()
        {
            if (player.isPerformingAction) { return; }
            if (player.playerNetworkManager.currentStamina.Value <= 0) { return; }
            if (player.playerNetworkManager.isJumping.Value) { return; }
            if (!player.playerLocomotionManager.isGrounded) { return; }

            // TODO: Two Hand or One Hand Animation
            player.playerAnimatorManager.PlayActionAnimation("Main_Jump_Start_01", false);
            player.playerNetworkManager.isJumping.Value = true;
            
            player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

            jumpDirection = PlayerCamera.Instance.cameraObject.transform.forward * PlayerInputManager.Instance.verticalInput;
            jumpDirection += PlayerCamera.Instance.cameraObject.transform.right * PlayerInputManager.Instance.horizontalInput;
            jumpDirection.y = 0;

            if(jumpDirection != Vector3.zero)
            {
                // Sprinting = Full, running = half, walking = quarter
                if(player.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 1;
                }
                else if(PlayerInputManager.Instance.moveAmount >= 0.5) // Walk
                {
                    jumpDirection *= 0.5f; 
                }
                else if (PlayerInputManager.Instance.moveAmount < 0.5) // Walk
                {
                    jumpDirection *= 0.25f;
                }
            }
        }

        // Call Main_Jump_Lift Events
        public void ApplyJumpVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
    }

}

