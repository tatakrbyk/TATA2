using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XD
{
    public class PlayerManager : CharacterManager
    {
        [Header("Debug Menu")]
        [SerializeField] private bool respawnCharacter = false;

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
        protected override void Update()
        {
            base.Update();

            // If we do not own this gameobject, we do not control or edit it 
            if (!IsOwner) return;

            playerLocomotionManager.HandleAllMovement();
            playerStatsManager.RegenerateStamina();

            DebugMenu();
        }

        protected override void LateUpdate()
        {
            if (!IsOwner) return;
            base.LateUpdate();

            PlayerCamera.Instance.HandleAllCameraActions();

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
                
                // Update (if increase statü value )
                playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;

                // Update UI Stat Bars
                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.Instance.playerUIHUDManager.SetNewHealthValue;

                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.Instance.playerUIHUDManager.SetNewStaminaValue; 
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegeneraationTimer;

            }

            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP; 
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if(IsOwner)
            {
                PlayerUIManager.Instance.playerUIPopUpManager.SendYouDiedPopUp();
            }
            return base.ProcessDeathEvent(manuallySelectDeathAnimation);    
        }

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();

            if(IsOwner)
            {
                playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
                playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;

                playerAnimatorManager.PlayActionAnimation("Empty", false);
            }
        }
     
        
        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.xCoord = transform.position.x;
            currentCharacterData.yCoord = transform.position.y;
            currentCharacterData.zCoord = transform.position.z;

            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;

            currentCharacterData.vitality = playerNetworkManager.vitality.Value;
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;
        }
        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 playerPosition = new Vector3(currentCharacterData.xCoord, currentCharacterData.yCoord, currentCharacterData.zCoord);
            transform.position = playerPosition;

            playerNetworkManager.vitality.Value = currentCharacterData.vitality;
            playerNetworkManager.endurance.Value = currentCharacterData.endurance;

            // Health
            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            PlayerUIManager.Instance.playerUIHUDManager.SetMaxHealthValue(playerNetworkManager.maxHealth.Value);
            // Stamina
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            PlayerUIManager.Instance.playerUIHUDManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
        }

        // TODO: DEBUG
        private void DebugMenu()
        {
            if(respawnCharacter)
            {
                respawnCharacter = false;
                ReviveCharacter();

            }
        }
    }
}
  