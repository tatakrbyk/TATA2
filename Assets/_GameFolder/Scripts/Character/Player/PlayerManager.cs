using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace XD
{
    public class PlayerManager : CharacterManager
    {

        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        [HideInInspector] public PlayerInteractionManager playerInteractionManager;
        [HideInInspector] public PlayerEffectsManager playerEffectsManager;
        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager  = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInteractionManager = GetComponent<PlayerInteractionManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }
        protected override void Update()
        {
            base.Update();

            // If we do not own this gameobject, we do not control or edit it 
            if (!IsOwner) return;

            playerLocomotionManager.HandleAllMovement();
            playerStatsManager.RegenerateStamina();

            
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
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            // If this is the player object owned by this client
            if (IsOwner)
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

            // Only Update hp bar if this character is not the local character
            if (!IsOwner)
            {
                characterNetworkManager.currentHealth.OnValueChanged += characterUIManager.OnHPChanged;
                
            }
            

            // Stats
            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;

            // Lock On
            playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChanged;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged += playerNetworkManager.OnLockOnTargetIDChange;
            // Equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange; 
            playerNetworkManager.isBlocking.OnValueChanged += playerNetworkManager.OnIsBlockingChanged;

            // Two Hand
            playerNetworkManager.IsTwoHandingWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingWeaponChanged;
            playerNetworkManager.IsTwoHandingRightWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingRightWeaponChanged;
            playerNetworkManager.IsTwoHandingLeftWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingLeftWeaponChanged;

            // FLAGS
            playerNetworkManager.isChargingAttack.OnValueChanged += playerNetworkManager.OnIsChargingAttackChanged;

            if (IsOwner && !IsServer)
            {
                LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.Instance.currentCharacterData);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
            // If this is the player object owned by this client
            if (IsOwner)
            {


                // Update (if increase statü value )
                playerNetworkManager.vitality.OnValueChanged -= playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewMaxStaminaValue;

                // Update UI Stat Bars
                playerNetworkManager.currentHealth.OnValueChanged -= PlayerUIManager.Instance.playerUIHUDManager.SetNewHealthValue;

                playerNetworkManager.currentStamina.OnValueChanged -= PlayerUIManager.Instance.playerUIHUDManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged -= playerStatsManager.ResetStaminaRegeneraationTimer;

            }
            if (!IsOwner)
            {
                characterNetworkManager.currentHealth.OnValueChanged -= characterUIManager.OnHPChanged;
            }
            // Stats
            playerNetworkManager.currentHealth.OnValueChanged -= playerNetworkManager.CheckHP;

            // Lock On
            playerNetworkManager.isLockedOn.OnValueChanged -= playerNetworkManager.OnIsLockedOnChanged;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged -= playerNetworkManager.OnLockOnTargetIDChange;
            // Equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged -= playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            //playerNetworkManager.isBlocking.OnValueChanged -= playerNetworkManager.OnIsBlockingChanged;

            // Two Hand
            playerNetworkManager.IsTwoHandingWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingWeaponChanged;
            playerNetworkManager.IsTwoHandingRightWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingRightWeaponChanged;
            playerNetworkManager.IsTwoHandingLeftWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingLeftWeaponChanged;

            // FLAGS
            playerNetworkManager.isChargingAttack.OnValueChanged -= playerNetworkManager.OnIsChargingAttackChanged;
        }

        private void OnClientConnectedCallback(ulong clientId)
        {

            WorldGameSessionManager.Instance.AddPlayerToActivePlayersList(this);

            // If we are the server, we are the host, so we don't need to Load Players to sync data
            if (!IsServer && IsOwner)
            {
                foreach (var player in WorldGameSessionManager.Instance.activePlayers)
                {
                    if (player != this)
                    {
                        player.LoadOtherPlayerCharacterWhenJoiningServer();
                    }
                }
            }
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
                isDead.Value = false;
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

        // When a player late joins the server
        public void LoadOtherPlayerCharacterWhenJoiningServer( )
        {
            // Sync Weapons
            playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, playerNetworkManager.currentRightHandWeaponID.Value);
            playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, playerNetworkManager.currentLeftHandWeaponID.Value);

            // Sync Two Hand Status
            playerNetworkManager.OnIsTwoHandingRightWeaponChanged(false, playerNetworkManager.IsTwoHandingRightWeapon.Value);
            playerNetworkManager.OnIsTwoHandingLeftWeaponChanged(false, playerNetworkManager.IsTwoHandingLeftWeapon.Value);

            // SYNC BLOCK STATUS 
            playerNetworkManager.OnIsBlockingChanged(false, playerNetworkManager.isBlocking.Value);

            // Lock On
            if (playerNetworkManager.isLockedOn.Value)
            {
                playerNetworkManager.OnLockOnTargetIDChange(0, playerNetworkManager.currentTargetNetworkObjectID.Value);
            }
        }

  
    }
}
  