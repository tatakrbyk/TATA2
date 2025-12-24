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
                PlayerUIManager.Instance.localPlayer = this;

                // Update (if increase statü value )
                playerNetworkManager.vigor.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;
                playerNetworkManager.mind.OnValueChanged += playerNetworkManager.SetNewMaxFocusPointValue;


                // Update UI Stat Bars
                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.Instance.playerUIHUDManager.SetNewHealthValue;

                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.Instance.playerUIHUDManager.SetNewStaminaValue; 
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegeneraationTimer;

                playerNetworkManager.currentFocusPoints.OnValueChanged += PlayerUIManager.Instance.playerUIHUDManager.SetNewFocusPointValue;

                // Resets Camera Rotation to Standart When Aimig Is Disabled
                playerNetworkManager.isAiming.OnValueChanged += playerNetworkManager.OnIsAimingChanged;

            }

            // Only Update hp bar if this character is not the local character
            if (!IsOwner)
            {
                characterNetworkManager.currentHealth.OnValueChanged += characterUIManager.OnHPChanged;
                
            }
            
            // BodyType
            playerNetworkManager.isMale.OnValueChanged += playerNetworkManager.OnIsMaleChanged;
            // Stats
            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.OnHPChanged;
            playerNetworkManager.currentFocusPoints.OnValueChanged += playerNetworkManager.OnFocusPointsChanged;
            playerNetworkManager.maxFocusPoints.OnValueChanged += playerNetworkManager.OnMaxFocusPointsChanged;

            // Lock On
            playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChanged;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged += playerNetworkManager.OnLockOnTargetIDChange;
            
            // Equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange; 
            playerNetworkManager.currentSpellID.OnValueChanged += playerNetworkManager.OnCurrentSpellIDChange;
            playerNetworkManager.currentQuickSlotItemID.OnValueChanged += playerNetworkManager.OnCurrentQuickSlotItemIDChange;
            playerNetworkManager.isChugging.OnValueChanged += playerNetworkManager.OnIsChuggingChanged;


            playerNetworkManager.isBlocking.OnValueChanged += playerNetworkManager.OnIsBlockingChanged;

            playerNetworkManager.headEquipmentID.OnValueChanged += playerNetworkManager.OnHeadEquipmentChanged;
            playerNetworkManager.bodyEquipmentID.OnValueChanged += playerNetworkManager.OnBodyEquipmentChanged;
            playerNetworkManager.legEquipmentID.OnValueChanged += playerNetworkManager.OnLegEquipmentChanged;
            playerNetworkManager.handEquipmentID.OnValueChanged += playerNetworkManager.OnHandEquipmentChanged;
            playerNetworkManager.mainProjectileID.OnValueChanged += playerNetworkManager.OnMainProjectileIDChange;
            playerNetworkManager.secondaryProjectileID.OnValueChanged += playerNetworkManager.OnSecondaryProjectileIDChange;
            playerNetworkManager.isHoldingArrow.OnValueChanged += playerNetworkManager.OnIsHoldingArrowChanged;


            // Body 
            playerNetworkManager.hairStyleID.OnValueChanged += playerNetworkManager.OnHairStyleIDChanged;
            playerNetworkManager.hairColorRed.OnValueChanged += playerNetworkManager.OnHairColorRedChanged;
            playerNetworkManager.hairColorGreen.OnValueChanged += playerNetworkManager.OnHairColorGreenChanged;
            playerNetworkManager.hairColorBlue.OnValueChanged += playerNetworkManager.OnHairColorBlueChanged;

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
                playerNetworkManager.vigor.OnValueChanged -= playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewMaxStaminaValue;
                playerNetworkManager.mind.OnValueChanged -= playerNetworkManager.SetNewMaxFocusPointValue;

                // Update UI Stat Bars
                playerNetworkManager.currentHealth.OnValueChanged -= PlayerUIManager.Instance.playerUIHUDManager.SetNewHealthValue;

                playerNetworkManager.currentStamina.OnValueChanged -= PlayerUIManager.Instance.playerUIHUDManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged -= playerStatsManager.ResetStaminaRegeneraationTimer;

                playerNetworkManager.currentFocusPoints.OnValueChanged -= PlayerUIManager.Instance.playerUIHUDManager.SetNewFocusPointValue;

                // Resets Camera Rotation to Standart When Aimig Is Disabled
                playerNetworkManager.isAiming.OnValueChanged -= playerNetworkManager.OnIsAimingChanged;

            }
            if (!IsOwner)
            {
                characterNetworkManager.currentHealth.OnValueChanged -= characterUIManager.OnHPChanged;
            }
            // BodyType
            playerNetworkManager.isMale.OnValueChanged -= playerNetworkManager.OnIsMaleChanged;
            // Stats
            playerNetworkManager.currentHealth.OnValueChanged -= playerNetworkManager.OnHPChanged;
            playerNetworkManager.currentFocusPoints.OnValueChanged -= playerNetworkManager.OnFocusPointsChanged;
            playerNetworkManager.maxFocusPoints.OnValueChanged -= playerNetworkManager.OnMaxFocusPointsChanged;

            // Lock On
            playerNetworkManager.isLockedOn.OnValueChanged -= playerNetworkManager.OnIsLockedOnChanged;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged -= playerNetworkManager.OnLockOnTargetIDChange;
            
            // Equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged -= playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            playerNetworkManager.currentSpellID.OnValueChanged -= playerNetworkManager.OnCurrentSpellIDChange;
            playerNetworkManager.currentQuickSlotItemID.OnValueChanged -= playerNetworkManager.OnCurrentQuickSlotItemIDChange;
            playerNetworkManager.isChugging.OnValueChanged -= playerNetworkManager.OnIsChuggingChanged;


            playerNetworkManager.isBlocking.OnValueChanged -= playerNetworkManager.OnIsBlockingChanged;

            playerNetworkManager.headEquipmentID.OnValueChanged -= playerNetworkManager.OnHeadEquipmentChanged;
            playerNetworkManager.bodyEquipmentID.OnValueChanged -= playerNetworkManager.OnBodyEquipmentChanged;
            playerNetworkManager.legEquipmentID.OnValueChanged -= playerNetworkManager.OnLegEquipmentChanged;
            playerNetworkManager.handEquipmentID.OnValueChanged -= playerNetworkManager.OnHandEquipmentChanged;

            playerNetworkManager.mainProjectileID.OnValueChanged -= playerNetworkManager.OnMainProjectileIDChange;
            playerNetworkManager.secondaryProjectileID.OnValueChanged -= playerNetworkManager.OnSecondaryProjectileIDChange;
            playerNetworkManager.isHoldingArrow.OnValueChanged -= playerNetworkManager.OnIsHoldingArrowChanged;

            // Body
            playerNetworkManager.hairStyleID.OnValueChanged -= playerNetworkManager.OnHairStyleIDChanged;
            playerNetworkManager.hairColorRed.OnValueChanged -= playerNetworkManager.OnHairColorRedChanged;
            playerNetworkManager.hairColorGreen.OnValueChanged -= playerNetworkManager.OnHairColorGreenChanged;
            playerNetworkManager.hairColorBlue.OnValueChanged -= playerNetworkManager.OnHairColorBlueChanged;

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
            
            // 
            WorldGameSessionManager.Instance.WaitThenReviveHost();

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

            // Stats
            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;
            currentCharacterData.currentFocusPoints = playerNetworkManager.currentFocusPoints.Value;

            currentCharacterData.runes = playerStatsManager.runes;
            PlayerUIManager.Instance.playerUIHUDManager.SetRunesCount(currentCharacterData.runes); 

            currentCharacterData.vitality = playerNetworkManager.vigor.Value;
            currentCharacterData.mind = playerNetworkManager.mind.Value;
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;
            currentCharacterData.strength = playerNetworkManager.strength.Value;
            currentCharacterData.dexterity = playerNetworkManager.dexterity.Value;
            currentCharacterData.intelligence = playerNetworkManager.intelligence.Value;
            currentCharacterData.faith = playerNetworkManager.faith.Value;


            // BODY 
            currentCharacterData.hairStyleID = playerNetworkManager.hairStyleID.Value;
            currentCharacterData.hairColorRed = playerNetworkManager.hairColorRed.Value;
            currentCharacterData.hairColorGreen = playerNetworkManager.hairColorGreen.Value;
            currentCharacterData.hairColorBlue = playerNetworkManager.hairColorBlue.Value;

            currentCharacterData.currentHealthFlaskRemaining = playerNetworkManager.remainingHealthFlasks.Value;
            currentCharacterData.currentFocusPointsFlaskRemaining = playerNetworkManager.remainingFocusPointsFlasks.Value;

            // Equipment
            currentCharacterData.headEquipment = playerNetworkManager.headEquipmentID.Value;
            currentCharacterData.bodyEquipment = playerNetworkManager.bodyEquipmentID.Value;
            currentCharacterData.legEquipment = playerNetworkManager.legEquipmentID.Value;
            currentCharacterData.handEquipment = playerNetworkManager.handEquipmentID.Value;

            currentCharacterData.rightWeaponIndex = playerInventoryManager.rightHandWeaponIndex;
            currentCharacterData.rightWeapon01 =  WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlots[0]);
            currentCharacterData.rightWeapon02 = WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlots[1]);
            currentCharacterData.rightWeapon03 = WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlots[2]);

            currentCharacterData.leftWeaponIndex = playerInventoryManager.leftHandWeaponIndex;
            currentCharacterData.leftWeapon01 = WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInLeftHandSlots[0]);
            currentCharacterData.leftWeapon02 = WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInLeftHandSlots[1]);
            currentCharacterData.leftWeapon03 = WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInLeftHandSlots[2]);

            currentCharacterData.quickSlotIndex = playerInventoryManager.quickSlotItemIndex;
            currentCharacterData.quickSlotItem01 = WorldSaveGameManager.Instance.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[0]);
            currentCharacterData.quickSlotItem02 = WorldSaveGameManager.Instance.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[1]);
            currentCharacterData.quickSlotItem03 = WorldSaveGameManager.Instance.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[2]);

            currentCharacterData.mainProjectile = WorldSaveGameManager.Instance.GetSerializableRangedProjectileFromRangedProjectileItem(playerInventoryManager.mainProjectile);
            currentCharacterData.SecondaryProjectile = WorldSaveGameManager.Instance.GetSerializableRangedProjectileFromRangedProjectileItem(playerInventoryManager.secondaryProjectile);
            
            if (playerInventoryManager.currentSpell != null)
            {
                currentCharacterData.currentSpell = playerInventoryManager.currentSpell.itemID;
            }
            // Clear list before sace
            currentCharacterData.weaponsInInventory = new List<SerializableWeapon>();
            currentCharacterData.headEquipmentInInventory = new List<int>();
            currentCharacterData.bodyEquipmentInInventory = new List<int>();
            currentCharacterData.legEquipmentInInventory = new List<int>();
            currentCharacterData.handEquipmentInInventory = new List<int>();
            currentCharacterData.quickSlotItemsInInventory = new List<SerializableQuickSlotItem>();
            currentCharacterData.projectilesInInventory = new List<SerializableRangedProjectile>();


            for (int i = 0; i < playerInventoryManager.itemsInInventory.Count; i++)
            {
                if(playerInventoryManager.itemsInInventory[i] == null) { continue; }
                
                WeaponItem weaponInInventory = playerInventoryManager.itemsInInventory[i] as WeaponItem;
                HeadEquipmentItem headEquipmmentInInventory = playerInventoryManager.itemsInInventory[i] as HeadEquipmentItem;
                BodyEquipmentItem bodyEquipmmentInInventory = playerInventoryManager.itemsInInventory[i] as BodyEquipmentItem;
                LegEquipmentItem legEquipmmentInInventory = playerInventoryManager.itemsInInventory[i] as LegEquipmentItem;
                HandEquipmentItem handEquipmmentInInventory = playerInventoryManager.itemsInInventory[i] as HandEquipmentItem;
                QuickSlotItem quickSlotItemInInventory = playerInventoryManager.itemsInInventory[i] as QuickSlotItem;
                RangedProjectileItem rangedProjectileItem = playerInventoryManager.itemsInInventory[i] as RangedProjectileItem;

                if(weaponInInventory != null)
                {
                    currentCharacterData.weaponsInInventory.Add(WorldSaveGameManager.Instance.GetSerializableWeaponFromWeaponItem(weaponInInventory));
                }
                if (headEquipmmentInInventory != null)
                {
                    currentCharacterData.headEquipmentInInventory.Add(headEquipmmentInInventory.itemID);
                }
                if (bodyEquipmmentInInventory != null)
                {
                    currentCharacterData.bodyEquipmentInInventory.Add(bodyEquipmmentInInventory.itemID);
                }
                if (legEquipmmentInInventory != null)
                {
                    currentCharacterData.legEquipmentInInventory.Add(legEquipmmentInInventory.itemID);
                }
                if (handEquipmmentInInventory != null)
                {
                    currentCharacterData.handEquipmentInInventory.Add(handEquipmmentInInventory.itemID);
                }
                if (quickSlotItemInInventory != null)
                {
                    currentCharacterData.quickSlotItemsInInventory.Add(WorldSaveGameManager.Instance.GetSerializableQuickSlotItemFromQuickSlotItem(quickSlotItemInInventory));
                }
                if (rangedProjectileItem != null)
                {
                    currentCharacterData.projectilesInInventory.Add(WorldSaveGameManager.Instance.GetSerializableRangedProjectileFromRangedProjectileItem(rangedProjectileItem));
                }


            }
        }
        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;

            playerNetworkManager.isMale.Value = currentCharacterData.IsMale;
            playerBodyManager.ToggleBodyType(currentCharacterData.IsMale);    // Toggle incase the value is the same as default (OnvalueChanged only works when value is changed)

            Vector3 playerPosition = new Vector3(currentCharacterData.xCoord, currentCharacterData.yCoord, currentCharacterData.zCoord);
            transform.position = playerPosition;

            playerNetworkManager.vigor.Value = currentCharacterData.vitality;
            playerNetworkManager.mind.Value = currentCharacterData.mind;
            playerNetworkManager.endurance.Value = currentCharacterData.endurance;
            playerNetworkManager.strength.Value = currentCharacterData.strength;
            playerNetworkManager.dexterity.Value = currentCharacterData.dexterity;
            playerNetworkManager.intelligence.Value = currentCharacterData.intelligence;
            playerNetworkManager.faith.Value = currentCharacterData.faith;
             
            playerStatsManager.runes = currentCharacterData.runes;

            // Health
            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vigor.Value);
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            PlayerUIManager.Instance.playerUIHUDManager.SetMaxHealthValue(playerNetworkManager.maxHealth.Value);

            // Stamina
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            PlayerUIManager.Instance.playerUIHUDManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);

            // Mind 
            playerNetworkManager.maxFocusPoints.Value = playerStatsManager.CalculateFocusPointsBasedOnMindLevel(playerNetworkManager.mind.Value);
            playerNetworkManager.currentFocusPoints.Value = currentCharacterData.currentFocusPoints;
            PlayerUIManager.Instance.playerUIHUDManager.SetMaxFocusPointValue(playerNetworkManager.maxFocusPoints.Value);

            // Remaining Flasks
            playerNetworkManager.remainingHealthFlasks.Value = currentCharacterData.currentHealthFlaskRemaining;
            playerNetworkManager.remainingFocusPointsFlasks.Value = currentCharacterData.currentFocusPointsFlaskRemaining;

            // BodY
            playerNetworkManager.hairStyleID.Value = currentCharacterData.hairStyleID;
            playerNetworkManager.hairColorRed.Value = currentCharacterData.hairColorRed;
            playerNetworkManager.hairColorGreen.Value = currentCharacterData.hairColorGreen;
            playerNetworkManager.hairColorBlue.Value = currentCharacterData.hairColorBlue;

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

            //if(WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.rightWeapon01))
            //{
            //    playerNetworkManager.currentRightHandWeaponID.Value = currentCharacterData.rightWeapon01;
            //}
            //else
            //{
            //    playerNetworkManager.currentRightHandWeaponID.Value = 0;
            //}
            
            // Weapon Items
            playerInventoryManager.rightHandWeaponIndex = currentCharacterData.rightWeaponIndex;
            playerInventoryManager.weaponsInRightHandSlots[0] = currentCharacterData.rightWeapon01.GetWeapon();
            playerInventoryManager.weaponsInRightHandSlots[1] = currentCharacterData.rightWeapon02.GetWeapon();
            playerInventoryManager.weaponsInRightHandSlots[2] = currentCharacterData.rightWeapon03.GetWeapon();
            playerInventoryManager.leftHandWeaponIndex = currentCharacterData.leftWeaponIndex;
            playerInventoryManager.weaponsInLeftHandSlots[0] = currentCharacterData.leftWeapon01.GetWeapon();
            playerInventoryManager.weaponsInLeftHandSlots[1] = currentCharacterData.leftWeapon02.GetWeapon();
            playerInventoryManager.weaponsInLeftHandSlots[2] = currentCharacterData.leftWeapon03.GetWeapon();

            // Quick Slots Items
            playerInventoryManager.quickSlotItemIndex = currentCharacterData.quickSlotIndex;
            playerInventoryManager.quickSlotItemsInQuickSlots[0] = currentCharacterData.quickSlotItem01.GetQuickSlotItem();
            playerInventoryManager.quickSlotItemsInQuickSlots[1] = currentCharacterData.quickSlotItem02.GetQuickSlotItem();
            playerInventoryManager.quickSlotItemsInQuickSlots[2] = currentCharacterData.quickSlotItem03.GetQuickSlotItem();

            playerEquipmentManager.LoadQuickSlotEquipment(playerInventoryManager.quickSlotItemsInQuickSlots[playerInventoryManager.quickSlotItemIndex]); // This Refreshes the HUD

            if (currentCharacterData.rightWeaponIndex >= 0)
            {
                playerInventoryManager.currentRightHandWeapon = playerInventoryManager.weaponsInRightHandSlots[currentCharacterData.rightWeaponIndex];
                playerNetworkManager.currentRightHandWeaponID.Value = playerInventoryManager.weaponsInRightHandSlots[currentCharacterData.rightWeaponIndex].itemID;
            }
            else
            {
                playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
            }


            if(currentCharacterData.leftWeaponIndex >= 0)
            {
                playerInventoryManager.currentLeftHandWeapon = playerInventoryManager.weaponsInLeftHandSlots[currentCharacterData.leftWeaponIndex];
                playerNetworkManager.currentLeftHandWeaponID.Value = playerInventoryManager.weaponsInLeftHandSlots[currentCharacterData.leftWeaponIndex].itemID;
            }
            else
            {
                playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
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

            for(int i = 0; i <currentCharacterData.weaponsInInventory.Count; i++)
            {
                WeaponItem weaponItem = currentCharacterData.weaponsInInventory[i].GetWeapon();
                playerInventoryManager.AddItemToInventory(weaponItem);
            }
            for (int i = 0; i < currentCharacterData.headEquipmentInInventory.Count; i++)
            {
                EquipmentItem equipmentItem = WorldItemDatabase.Instance.GetHeadEquipmentByID(currentCharacterData.headEquipmentInInventory[i]);
                playerInventoryManager.AddItemToInventory(Instantiate(equipmentItem));
            }
            for (int i = 0; i < currentCharacterData.bodyEquipmentInInventory.Count; i++)
            {
                EquipmentItem equipmentItem = WorldItemDatabase.Instance.GetBodyEquipmentByID(currentCharacterData.bodyEquipmentInInventory[i]);
                playerInventoryManager.AddItemToInventory(Instantiate(equipmentItem));
            }
            for (int i = 0; i < currentCharacterData.legEquipmentInInventory.Count; i++)
            {
                EquipmentItem equipmentItem = WorldItemDatabase.Instance.GetLegEquipmentByID(currentCharacterData.legEquipmentInInventory[i]);
                playerInventoryManager.AddItemToInventory(Instantiate(equipmentItem));
            }
            for (int i = 0; i < currentCharacterData.handEquipmentInInventory.Count; i++)
            {
                EquipmentItem equipmentItem = WorldItemDatabase.Instance.GetHandEquipmentByID(currentCharacterData.handEquipmentInInventory[i]);
                playerInventoryManager.AddItemToInventory(Instantiate(equipmentItem));
            }
            for (int i = 0; i < currentCharacterData.quickSlotItemsInInventory.Count; i++)
            {
                QuickSlotItem quickSlotItem = currentCharacterData.quickSlotItemsInInventory[i].GetQuickSlotItem();
                playerInventoryManager.AddItemToInventory(quickSlotItem);
            }
            for (int i = 0; i < currentCharacterData.projectilesInInventory.Count; i++)
            {
                RangedProjectileItem rangedProjectileItem = currentCharacterData.projectilesInInventory[i].GetProjectile();
                playerInventoryManager.AddItemToInventory(rangedProjectileItem);
            }

            playerEquipmentManager.EquipArmors();

            playerEquipmentManager.LoadMainProjectileEquipment(currentCharacterData.mainProjectile.GetProjectile());
            playerEquipmentManager.LoadSecondaryProjectileEquipment(currentCharacterData.SecondaryProjectile.GetProjectile());  

        }
         
        // When a player late joins the server
        public void LoadOtherPlayerCharacterWhenJoiningServer( )
        {
            // Sync Body Type
            playerNetworkManager.OnIsMaleChanged(false, playerNetworkManager.isMale.Value);
            playerNetworkManager.OnHairStyleIDChanged(0, playerNetworkManager.hairStyleID.Value);

            playerNetworkManager.OnHairColorRedChanged(0, playerNetworkManager.hairColorRed.Value);
            playerNetworkManager.OnHairColorGreenChanged(0, playerNetworkManager.hairColorGreen.Value);
            playerNetworkManager.OnHairColorBlueChanged(0, playerNetworkManager.hairColorBlue.Value);

            // Sync Weapons
            playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, playerNetworkManager.currentRightHandWeaponID.Value);
            playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, playerNetworkManager.currentLeftHandWeaponID.Value);
            playerNetworkManager.OnCurrentSpellIDChange(0, playerNetworkManager.currentSpellID.Value);

            // Sync Armor
            playerNetworkManager.OnHeadEquipmentChanged(0, playerNetworkManager.headEquipmentID.Value);
            playerNetworkManager.OnBodyEquipmentChanged(0, playerNetworkManager.bodyEquipmentID.Value);
            playerNetworkManager.OnLegEquipmentChanged(0, playerNetworkManager.legEquipmentID.Value);
            playerNetworkManager.OnHandEquipmentChanged(0, playerNetworkManager.handEquipmentID.Value);

            // Sync Projectiles
            playerNetworkManager.OnMainProjectileIDChange(0, playerNetworkManager.mainProjectileID.Value);
            playerNetworkManager.OnSecondaryProjectileIDChange(0, playerNetworkManager.secondaryProjectileID.Value);
            playerNetworkManager.OnIsHoldingArrowChanged(false, playerNetworkManager.isHoldingArrow.Value);

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
  