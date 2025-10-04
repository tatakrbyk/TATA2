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
        [HideInInspector] public PlayerBodyManager playerBodyManager;

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
            playerBodyManager = GetComponent<PlayerBodyManager>();
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
                playerNetworkManager.mind.OnValueChanged += playerNetworkManager.SetNewMaxFocusPointValue;

                // Update UI Stat Bars
                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.Instance.playerUIHUDManager.SetNewHealthValue;

                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.Instance.playerUIHUDManager.SetNewStaminaValue; 
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegeneraationTimer;

                playerNetworkManager.currentFocusPoints.OnValueChanged += PlayerUIManager.Instance.playerUIHUDManager.SetNewFocusPointValue;

            }

            // Only Update hp bar if this character is not the local character
            if (!IsOwner)
            {
                characterNetworkManager.currentHealth.OnValueChanged += characterUIManager.OnHPChanged;
                
            }
            
            // BodyType
            playerNetworkManager.isMale.OnValueChanged += playerNetworkManager.OnIsMaleChanged;
            // Stats
            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;

            // Lock On
            playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChanged;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged += playerNetworkManager.OnLockOnTargetIDChange;
            // Equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange; 
            playerNetworkManager.currentSpellID.OnValueChanged += playerNetworkManager.OnCurrentSpellIDChange;

            playerNetworkManager.isBlocking.OnValueChanged += playerNetworkManager.OnIsBlockingChanged;

            playerNetworkManager.headEquipmentID.OnValueChanged += playerNetworkManager.OnHeadEquipmentChanged;
            playerNetworkManager.bodyEquipmentID.OnValueChanged += playerNetworkManager.OnBodyEquipmentChanged;
            playerNetworkManager.legEquipmentID.OnValueChanged += playerNetworkManager.OnLegEquipmentChanged;
            playerNetworkManager.handEquipmentID.OnValueChanged += playerNetworkManager.OnHandEquipmentChanged;
            
            // Spells
            playerNetworkManager.isChargingRightSpell.OnValueChanged += playerNetworkManager.OnIsChargingRightSpellChanged;
            playerNetworkManager.isChargingLeftSpell.OnValueChanged += playerNetworkManager.OnIsChargingLeftSpellChanged;

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
                playerNetworkManager.mind.OnValueChanged -= playerNetworkManager.SetNewMaxFocusPointValue;

                // Update UI Stat Bars
                playerNetworkManager.currentHealth.OnValueChanged -= PlayerUIManager.Instance.playerUIHUDManager.SetNewHealthValue;

                playerNetworkManager.currentStamina.OnValueChanged -= PlayerUIManager.Instance.playerUIHUDManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged -= playerStatsManager.ResetStaminaRegeneraationTimer;

                playerNetworkManager.currentFocusPoints.OnValueChanged -= PlayerUIManager.Instance.playerUIHUDManager.SetNewFocusPointValue;

            }
            if (!IsOwner)
            {
                characterNetworkManager.currentHealth.OnValueChanged -= characterUIManager.OnHPChanged;
            }
            // BodyType
            playerNetworkManager.isMale.OnValueChanged -= playerNetworkManager.OnIsMaleChanged;
            // Stats
            playerNetworkManager.currentHealth.OnValueChanged -= playerNetworkManager.CheckHP;

            // Lock On
            playerNetworkManager.isLockedOn.OnValueChanged -= playerNetworkManager.OnIsLockedOnChanged;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged -= playerNetworkManager.OnLockOnTargetIDChange;
            
            // Equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged -= playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            playerNetworkManager.currentSpellID.OnValueChanged -= playerNetworkManager.OnCurrentSpellIDChange;
            
            playerNetworkManager.isBlocking.OnValueChanged -= playerNetworkManager.OnIsBlockingChanged;

            playerNetworkManager.headEquipmentID.OnValueChanged -= playerNetworkManager.OnHeadEquipmentChanged;
            playerNetworkManager.bodyEquipmentID.OnValueChanged -= playerNetworkManager.OnBodyEquipmentChanged;
            playerNetworkManager.legEquipmentID.OnValueChanged -= playerNetworkManager.OnLegEquipmentChanged;
            playerNetworkManager.handEquipmentID.OnValueChanged -= playerNetworkManager.OnHandEquipmentChanged;

            // Spells
            playerNetworkManager.isChargingRightSpell.OnValueChanged -= playerNetworkManager.OnIsChargingRightSpellChanged;
            playerNetworkManager.isChargingLeftSpell.OnValueChanged -= playerNetworkManager.OnIsChargingLeftSpellChanged;

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
            currentCharacterData.IsMale = playerNetworkManager.isMale.Value;
            currentCharacterData.xCoord = transform.position.x;
            currentCharacterData.yCoord = transform.position.y;
            currentCharacterData.zCoord = transform.position.z;

            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;
            currentCharacterData.currentFocusPoints = playerNetworkManager.currentFocusPoints.Value;


            currentCharacterData.vitality = playerNetworkManager.vitality.Value;
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;
            currentCharacterData.mind = playerNetworkManager.mind.Value;

            // EQuipment
            currentCharacterData.headEquipment = playerNetworkManager.headEquipmentID.Value;
            currentCharacterData.bodyEquipment = playerNetworkManager.bodyEquipmentID.Value;
            currentCharacterData.legEquipment = playerNetworkManager.legEquipmentID.Value;
            currentCharacterData.handEquipment = playerNetworkManager.handEquipmentID.Value;

            currentCharacterData.rightWeaponIndex = playerInventoryManager.rightHandWeaponIndex;
            currentCharacterData.rightWeapon01 = playerInventoryManager.weaponsInRightHandSlots[0].itemID;
            currentCharacterData.rightWeapon02 = playerInventoryManager.weaponsInRightHandSlots[1].itemID;
            currentCharacterData.rightWeapon03 = playerInventoryManager.weaponsInRightHandSlots[2].itemID;


            currentCharacterData.leftWeaponIndex = playerInventoryManager.leftHandWeaponIndex;
            currentCharacterData.leftWeapon01 = playerInventoryManager.weaponsInLeftHandSlots[0].itemID;
            currentCharacterData.leftWeapon02 = playerInventoryManager.weaponsInLeftHandSlots[1].itemID;
            currentCharacterData.leftWeapon03 = playerInventoryManager.weaponsInLeftHandSlots[2].itemID;

            if(playerInventoryManager.currentSpell != null)
            {
                currentCharacterData.currentSpell = playerInventoryManager.currentSpell.itemID;
            }
        }
        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;

            playerNetworkManager.isMale.Value = currentCharacterData.IsMale;
            playerBodyManager.ToggleBodyType(currentCharacterData.IsMale);    // Toggle incase the value is the same as default (OnvalueChanged only works when value is changed)

            Vector3 playerPosition = new Vector3(currentCharacterData.xCoord, currentCharacterData.yCoord, currentCharacterData.zCoord);
            transform.position = playerPosition;

            playerNetworkManager.vitality.Value = currentCharacterData.vitality;
            playerNetworkManager.endurance.Value = currentCharacterData.endurance;
            playerNetworkManager.mind.Value = currentCharacterData.mind;

            // Health
            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            
            // Stamina
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            
            // Mind 
            playerNetworkManager.maxFocusPoints.Value = playerStatsManager.CalculateFocusPointsBasedOnMindLevel(playerNetworkManager.mind.Value);
            playerNetworkManager.currentFocusPoints.Value = currentCharacterData.currentFocusPoints;
            

            // Equipment


            if (WorldItemDatabase.Instance.GetHeadEquipmentByID(currentCharacterData.bodyEquipment))
            {
                HeadEquipmentItem headEquipment = Instantiate(WorldItemDatabase.Instance.GetHeadEquipmentByID(currentCharacterData.headEquipment));
                playerInventoryManager.headEquipment = headEquipment;
            }
            else
            {
                playerInventoryManager.headEquipment = null;
            }

            if (WorldItemDatabase.Instance.GetHandEquipmentByID(currentCharacterData.handEquipment))
            {
                HandEquipmentItem handEquipment = Instantiate(WorldItemDatabase.Instance.GetHandEquipmentByID(currentCharacterData.handEquipment));
                playerInventoryManager.handEquipment = handEquipment;
            }
            else
            {
                playerInventoryManager.handEquipment = null;
            }

            if (WorldItemDatabase.Instance.GetLegEquipmentByID(currentCharacterData.legEquipment))
            {
                LegEquipmentItem legEquipment = Instantiate(WorldItemDatabase.Instance.GetLegEquipmentByID(currentCharacterData.legEquipment));
                playerInventoryManager.legEquipment = legEquipment;
            }
            else
            {
                playerInventoryManager.legEquipment = null;
            }

            if (WorldItemDatabase.Instance.GetBodyEquipmentByID(currentCharacterData.bodyEquipment))
            {

                BodyEquipmentItem bodyEquipment = Instantiate(WorldItemDatabase.Instance.GetBodyEquipmentByID(currentCharacterData.bodyEquipment));
                playerInventoryManager.bodyEquipment = bodyEquipment;
            }
            else
            {
                playerInventoryManager.bodyEquipment = null;
            }

            if(WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.rightWeapon01))
            {
                playerNetworkManager.currentRightHandWeaponID.Value = currentCharacterData.rightWeapon01;
            }
            else
            {
                playerNetworkManager.currentRightHandWeaponID.Value = 0;
            }
            if (WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.rightWeapon02))
            {
                playerNetworkManager.currentRightHandWeaponID.Value = currentCharacterData.rightWeapon02;
            }
            else
            {
                playerNetworkManager.currentRightHandWeaponID.Value = 0;
            }
            if (WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.rightWeapon03))
            {
                playerNetworkManager.currentRightHandWeaponID.Value = currentCharacterData.rightWeapon03;
            }
            else
            {
                playerNetworkManager.currentRightHandWeaponID.Value = 0;
            }
            if (WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.leftWeapon01))
            {
                playerNetworkManager.currentLeftHandWeaponID.Value = currentCharacterData.leftWeapon01;
            }
            else
            {
                playerNetworkManager.currentLeftHandWeaponID.Value = 0;
            }
            if (WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.leftWeapon02))
            {
                playerNetworkManager.currentLeftHandWeaponID.Value = currentCharacterData.leftWeapon02;
            }
            else
            {
                playerNetworkManager.currentLeftHandWeaponID.Value = 0;
            }
            if (WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.leftWeapon03))
            {
                playerNetworkManager.currentLeftHandWeaponID.Value = currentCharacterData.leftWeapon03;
            }
            else
            {
                playerNetworkManager.currentLeftHandWeaponID.Value = 0;
            }
            if (WorldItemDatabase.Instance.GetSpellByID(currentCharacterData.currentSpell))
            {
                SpellItem currentSpell = Instantiate(WorldItemDatabase.Instance.GetSpellByID(currentCharacterData.currentSpell));
                playerNetworkManager.currentSpellID.Value = currentSpell.itemID;
            }
            else
            {
                playerNetworkManager.currentSpellID.Value = -1; // -1 Sets spell to null as its not a valid ID
            }

            playerEquipmentManager.EquipArmors();

            playerInventoryManager.rightHandWeaponIndex = currentCharacterData.rightWeaponIndex;
            playerNetworkManager.currentRightHandWeaponID.Value = playerInventoryManager.weaponsInRightHandSlots[currentCharacterData.rightWeaponIndex].itemID;

            playerInventoryManager.leftHandWeaponIndex = currentCharacterData.leftWeaponIndex;
            playerNetworkManager.currentLeftHandWeaponID.Value = playerInventoryManager.weaponsInLeftHandSlots[currentCharacterData.leftWeaponIndex].itemID;
        }
         
        // When a player late joins the server
        public void LoadOtherPlayerCharacterWhenJoiningServer( )
        {
            // Sync Body Type
            playerNetworkManager.OnIsMaleChanged(false, playerNetworkManager.isMale.Value);
            // Sync Weapons
            playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, playerNetworkManager.currentRightHandWeaponID.Value);
            playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, playerNetworkManager.currentLeftHandWeaponID.Value);
            playerNetworkManager.OnCurrentSpellIDChange(0, playerNetworkManager.currentSpellID.Value);

            // Sync Armor
            playerNetworkManager.OnHeadEquipmentChanged(0, playerNetworkManager.headEquipmentID.Value);
            playerNetworkManager.OnBodyEquipmentChanged(0, playerNetworkManager.bodyEquipmentID.Value);
            playerNetworkManager.OnLegEquipmentChanged(0, playerNetworkManager.legEquipmentID.Value);
            playerNetworkManager.OnHandEquipmentChanged(0, playerNetworkManager.handEquipmentID.Value);           


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
  