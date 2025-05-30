using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // If this is the player object owned by this client
            if(IsOwner)
            {
                PlayerCamera.Instance.player = this;
                PlayerInputManager.Instance.player = this;
            }
        }
        protected override void Update()
        {
            base.Update();

            // If we do not own this gameobject, we do not control or edit it 
            if(!IsOwner) return;

            playerLocomotionManager.HandleAllMovement();
        }

        protected override void LateUpdate()
        {
            if(!IsOwner) return;
            base.LateUpdate();

            PlayerCamera.Instance.HandleAllCameraActions();

        }
    }
}
  