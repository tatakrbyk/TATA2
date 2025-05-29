using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;

        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;

        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] private float walkingSpeed = 2;
        [SerializeField] private float runningSpeed = 5;
        [SerializeField] private float rotationSpeed = 15;
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
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
            }
        }

        // Call PlayerManager
        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.Instance.verticalInput;
            horizontalMovement = PlayerInputManager.Instance.horizontalInput;  
            moveAmount = PlayerInputManager.Instance.moveAmount;
            
        }
        private void HandleGroundedMovement()
        {
            GetMovementValues();

            moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement; 
            moveDirection = moveDirection + PlayerCamera.Instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if(PlayerInputManager.Instance.moveAmount > 0.5f )
            {
                // Move at a running speed
                player.characterController.Move(moveDirection * runningSpeed *  Time.deltaTime);
            }
            else if(PlayerInputManager.Instance.moveAmount <= 0.5f)
            {
                // Move at a walking speed
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);

            }
        }

        private void HandleRotation()
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

}

