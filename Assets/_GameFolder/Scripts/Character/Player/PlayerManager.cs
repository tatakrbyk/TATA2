using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XD
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager  = GetComponent<PlayerStatsManager>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // If this is the player object owned by this client
            if(IsOwner)
            {
                PlayerCamera.Instance.player = this;
                PlayerInputManager.Instance.player = this;
                WorldSaveGameManager.Instance.player = this;
                
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.Instance.playerUIHUDManager.SetNewStaminaValue; 
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegeneraationTimer;

                playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                PlayerUIManager.Instance.playerUIHUDManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);

            }
        }
        protected override void Update()
        {
            base.Update();

            // If we do not own this gameobject, we do not control or edit it 
            if(!IsOwner) return;

            playerLocomotionManager.HandleAllMovement();
            playerStatsManager.RegenerateStamina();
        }

        protected override void LateUpdate()
        {
            if(!IsOwner) return;
            base.LateUpdate();

            PlayerCamera.Instance.HandleAllCameraActions();

        }
        
        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.xCoord = transform.position.x;
            currentCharacterData.yCoord = transform.position.y;
            currentCharacterData.zCoord = transform.position.z;
        }
        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 playerPosition = new Vector3(currentCharacterData.xCoord, currentCharacterData.yCoord, currentCharacterData.zCoord);
            transform.position= playerPosition;
        }
    }
}
  