using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace XD
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        PlayerManager player;

        public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Equipment")]
        public NetworkVariable<int> currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentSpellID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isUsingLeftHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Two Hanging)")]
        public NetworkVariable<int> currentWeaponBeingTwoHanded = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> IsTwoHandingWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> IsTwoHandingRightWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> IsTwoHandingLeftWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Spells")]
        public NetworkVariable<bool> isChargingRightSpell = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isChargingLeftSpell = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); 

        [Header("Armor")]
        public NetworkVariable<bool> isMale = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> headEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> bodyEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> legEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> handEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Projectiles")]
        public NetworkVariable<int> mainProjectileID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> secondaryProjectileID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasArrowNotched = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);  // This lets us know if we already have a projectile loaded
        public NetworkVariable<bool> isHoldingArrow = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); // This lets us know if we are holding that projectile so it does not releasen
        public NetworkVariable<bool> isAiming = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); // This lets us know if we are "ZOOMED" in and using our aiming Camera

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public void SetCharacterActionHand(bool rightHandedAction)
        {
            if(rightHandedAction)
            {
                isUsingLeftHand.Value = false;
                isUsingRightHand.Value = true;
            }
            else
            {
                isUsingRightHand.Value = false;
                isUsingLeftHand.Value = true;
            }
        }
        public void SetNewMaxHealthValue(int oldValue, int newValue)  // Vitality
        {
            maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnVitalityLevel(newValue);
            PlayerUIManager.Instance.playerUIHUDManager.SetMaxHealthValue(maxHealth.Value);
            currentHealth.Value = maxHealth.Value;
        }

        public void SetNewMaxStaminaValue(int oldValue, int newValue) // Endurance
        {
            maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newValue);
            PlayerUIManager.Instance.playerUIHUDManager.SetMaxStaminaValue(maxStamina.Value);
            currentStamina.Value = maxStamina.Value;
        }

        public void SetNewMaxFocusPointValue(int oldValue, int newValue) // Mind
        {
            maxFocusPoints.Value = player.playerStatsManager.CalculateFocusPointsBasedOnMindLevel(newValue);
            PlayerUIManager.Instance.playerUIHUDManager.SetMaxFocusPointValue(maxFocusPoints.Value);
            currentFocusPoints.Value = maxFocusPoints.Value;
        }


        public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));            
            player.playerInventoryManager.currentRightHandWeapon = newWeapon;
            player.playerEquipmentManager.LoadRightWeapon();

            if(player.IsOwner)
            {
                PlayerUIManager.Instance.playerUIHUDManager.SetRightWeaponQuickSlotIcon(newID);
            }
        }

        public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
            player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
            player.playerEquipmentManager.LoadLeftWeapon();

            if(player.IsOwner)
            {
                PlayerUIManager.Instance.playerUIHUDManager.SetLeftWeaponQuickSlotIcon(newID);
            }
        }     

        public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
            player.playerCombatManager.currentWeaponBeingUsed = newWeapon;

            if(player.IsOwner) {  return; }
            if(player.playerCombatManager.currentWeaponBeingUsed != null)
            {
                player.playerAnimatorManager.UpdateAnimatorController(player.playerCombatManager.currentWeaponBeingUsed.weaponAnimator);
            }
        }

        public void OnCurrentSpellIDChange(int oldID, int newID)
        {
            SpellItem newSpell = null;
            
            if(WorldItemDatabase.Instance.GetSpellByID(newID))
            {
                newSpell = Instantiate(WorldItemDatabase.Instance.GetSpellByID(newID));
            }

            if(newSpell != null)
            {
                player.playerInventoryManager.currentSpell = newSpell;
                if (player.IsOwner)
                {
                    PlayerUIManager.Instance.playerUIHUDManager.SetSpellItemQuickSlotIcon(newID);
                }
            }
            
        }

        public void OnMainProjectileIDChange(int oldID, int newID)
        {
            RangedProjectileItem newProjectile = null;

            if (WorldItemDatabase.Instance.GetProjectileByID(newID))
            {
                newProjectile = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(newID));
            }
            if(newProjectile != null)
            { 
                player.playerInventoryManager.mainProjectile = newProjectile;
            }
        }

        public void OnSecondaryProjectileIDChange(int oldID, int newID)
        {
            RangedProjectileItem newProjectile = null;
            if (WorldItemDatabase.Instance.GetProjectileByID(newID))
            {
                newProjectile = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(newID));
            }
            if (newProjectile != null)
            {
                player.playerInventoryManager.secondaryProjectile = newProjectile;
            }
        }

        public void OnIsHoldingArrowChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("IsHoldingArrow", isHoldingArrow.Value);
        }

        public void OnIsAimingChanged(bool oldStatus, bool newStatus)
        {
            Debug.Log("Aiming Status Changed: " + isAiming.Value);
            if (!isAiming.Value)
            {
                PlayerCamera.Instance.cameraObject.transform.localEulerAngles = new Vector3(0,0,0);
                PlayerCamera.Instance.cameraObject.fieldOfView = 60;
                PlayerCamera.Instance.cameraObject.nearClipPlane = 0.3f;
                PlayerCamera.Instance.cameraPivotTransform.localPosition = new Vector3(0, PlayerCamera.Instance.cameraPivotYPositionOffSet, 0);
                PlayerUIManager.Instance.playerUIHUDManager.crosshair.SetActive(false);
            }
            else
            {
                PlayerCamera.Instance.gameObject.transform.eulerAngles = new Vector3(0,0,0);
                PlayerCamera.Instance.cameraPivotTransform.eulerAngles = new Vector3(0,0,0);
                PlayerCamera.Instance.cameraObject.fieldOfView = 40;
                PlayerCamera.Instance.cameraObject.nearClipPlane = 1.3f;
                PlayerCamera.Instance.cameraPivotTransform.localPosition = Vector3.zero;
                PlayerUIManager.Instance.playerUIHUDManager.crosshair.SetActive(true);
            }
        }
        public void OnIsChargingRightSpellChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("IsChargingRightSpell", isChargingRightSpell.Value);
        }

        public void OnIsChargingLeftSpellChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("IsChargingLeftSpell", isChargingLeftSpell.Value);
        }
        public override void OnIsBlockingChanged(bool oldStatus, bool newStatus)
        {
            base.OnIsBlockingChanged(oldStatus, newStatus);

            if(IsOwner)
            {
                player.playerStatsManager.blockingPhysicalAbsorption = player.playerCombatManager.currentWeaponBeingUsed.physicalDamage * player.playerCombatManager.currentWeaponBeingUsed.physicalBaseDamageAbsorption;
                player.playerStatsManager.blockingMagicAbsorption = player.playerCombatManager.currentWeaponBeingUsed.magicDamage * player.playerCombatManager.currentWeaponBeingUsed.physicalBaseDamageAbsorption;
                player.playerStatsManager.blockingFireAbsorption = player.playerCombatManager.currentWeaponBeingUsed.fireDamage * player.playerCombatManager.currentWeaponBeingUsed.physicalBaseDamageAbsorption;
                player.playerStatsManager.blockingLightningAbsorption = player.playerCombatManager.currentWeaponBeingUsed.lightningDamage * player.playerCombatManager.currentWeaponBeingUsed.physicalBaseDamageAbsorption;
                player.playerStatsManager.blockingHolyAbsorption = player.playerCombatManager.currentWeaponBeingUsed.holyDamage * player.playerCombatManager.currentWeaponBeingUsed.physicalBaseDamageAbsorption;
                player.playerStatsManager.blockingStability = player.playerCombatManager.currentWeaponBeingUsed.stability;
            }
        }

        public  void OnIsTwoHandingWeaponChanged(bool oldStatus, bool newStatus)
        {
            if(!IsTwoHandingWeapon.Value)
            {
                if(IsOwner)
                {
                    IsTwoHandingLeftWeapon.Value = false;
                    IsTwoHandingRightWeapon.Value = false;
                }
                player.playerEquipmentManager.UnTwoHandWeapon();
                player.playerEffectsManager.RemoveStaticEffect(WorldCharacterEffectsManager.Instance.twoHandingEffect.staticEffectID);
            }
            else
            {
                StaticCharacterEffect twoHandEffect = Instantiate(WorldCharacterEffectsManager.Instance.twoHandingEffect);
                player.playerEffectsManager.AddStaticEffect(twoHandEffect);
            }
            player.animator.SetBool("IsTwoHandingWeapon", IsTwoHandingWeapon.Value);
        }

        public void OnIsTwoHandingRightWeaponChanged(bool oldStatus, bool newStatus)
        {
            if(!IsTwoHandingRightWeapon.Value) { return; }

            if(IsOwner)
            {
                currentWeaponBeingTwoHanded.Value = currentRightHandWeaponID.Value;
                IsTwoHandingWeapon.Value = true;
            }
            player.playerInventoryManager.currentTwoHandWeapon = player.playerInventoryManager.currentRightHandWeapon;
            player.playerEquipmentManager.TwoHandRightWeapon();
        }

        public void OnIsTwoHandingLeftWeaponChanged(bool oldStatus, bool newStatus)
        {
            if(!IsTwoHandingLeftWeapon.Value) { return; }

            if (IsOwner)
            {
                currentWeaponBeingTwoHanded.Value = currentLeftHandWeaponID.Value;
                IsTwoHandingWeapon.Value = true;
            }
            player.playerInventoryManager.currentTwoHandWeapon = player.playerInventoryManager.currentLeftHandWeapon;
            player.playerEquipmentManager.TwoHandLeftWeapon();
        }

        public void OnHeadEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner) return;

            HeadEquipmentItem equipment = WorldItemDatabase.Instance.GetHeadEquipmentByID(headEquipmentID.Value);

            if(equipment != null)
            {
                player.playerEquipmentManager.LoadHeadEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadHeadEquipment(null);
            }
        }

        public void OnBodyEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner) return;
            BodyEquipmentItem equipment = WorldItemDatabase.Instance.GetBodyEquipmentByID(bodyEquipmentID.Value);
            if (equipment != null)
            {
                player.playerEquipmentManager.LoadBodyEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadBodyEquipment(null);
            }
        }

        public void OnLegEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner) return;
            LegEquipmentItem equipment = WorldItemDatabase.Instance.GetLegEquipmentByID(legEquipmentID.Value);
            if (equipment != null)
            {
                player.playerEquipmentManager.LoadLegEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadLegEquipment(null);
            }
        }

        public void OnHandEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner) return;
            HandEquipmentItem equipment = WorldItemDatabase.Instance.GetHandEquipmentByID(handEquipmentID.Value);
            if (equipment != null)
            {
                player.playerEquipmentManager.LoadHandEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadHandEquipment(null);
            }
        }

        public void OnIsMaleChanged(bool oldStatus, bool newStatus)
        {
            player.playerBodyManager.ToggleBodyType(isMale.Value);
        }
        #region ItemAction
        // Item Actions
        [ServerRpc]
        public void NotifyTheServerOfWeaponActionServerRpc(ulong clientID, int actionID, int WeaponID)
        {
            if(IsServer)
            {
                NotifyTheServerOfWeaponActionClientRpc(clientID, actionID, WeaponID);
            }
        }

        [ClientRpc]
        private void NotifyTheServerOfWeaponActionClientRpc(ulong clientID, int actionID, int WeaponID)
        {
            if(clientID != NetworkManager.Singleton.LocalClientId)
            {

                PerformWeaponBasedAction(actionID, WeaponID);
            }
        }

        private void PerformWeaponBasedAction(int actionID, int WeaponID)
        {
            WeaponItemAction weaponAction = WorldActionManager.Instance.GetWeaponItemActionByID(actionID);
            if (weaponAction != null)
            {
                weaponAction.AttemptToPerformAction(player, WorldItemDatabase.Instance.GetWeaponByID(WeaponID));
            }
            else
            {
                Debug.LogError("Weapon Action with ID: " + actionID + " not found!");
            }
        }

        [ClientRpc]
        public override void DestroyAllCurrentActionFXClientRPC()
        {
            base.DestroyAllCurrentActionFXClientRPC();

            // For Canvel Arrow Action 

            if (hasArrowNotched.Value)
            {

                // Animate The Bow
                Animator bowAnimator;

                if (player.playerNetworkManager.IsTwoHandingLeftWeapon.Value)
                {
                    bowAnimator = player.playerEquipmentManager.leftHandWeaponModel.GetComponentInChildren<Animator>();
                }
                else
                {
                    bowAnimator = player.playerEquipmentManager.rightHandWeaponModel.GetComponentInChildren<Animator>();
                }

                bowAnimator.SetBool("IsDrawn", false);
                bowAnimator.Play("Bow_Fire_01");
                
                if(player.IsOwner) { hasArrowNotched.Value = false; }   
            }

                
        }

        #endregion

        #region Draw Projectile

        [ServerRpc]
        public void NotifyServerOfDrawnProjectileServerRpc(int projectileID)
        {
            if (IsServer)
            {
                NotifyClientsOfDrawnProjectileClientRpc(projectileID);
            }
        }
        [ClientRpc]
        private void NotifyClientsOfDrawnProjectileClientRpc(int projectileID)
        {
            Animator bowAnimator = null;

            if(IsTwoHandingLeftWeapon.Value)
            {
                bowAnimator = player.playerEquipmentManager.leftHandWeaponModel.GetComponentInChildren<Animator>();
            }
            else
            {
                bowAnimator = player.playerEquipmentManager.rightHandWeaponModel.GetComponentInChildren<Animator>();
            }

            if(bowAnimator != null)
            {

                bowAnimator.SetBool("IsDrawn", true);            
                bowAnimator.Play("Bow_Drawn_01");

            }
            GameObject arrow = Instantiate(WorldItemDatabase.Instance.GetProjectileByID(projectileID).drawProjectileModel, player.playerEquipmentManager.leftHandWeaponSlot.transform);
            player.playerEffectsManager.activeDrawnProjectileFX = arrow;

            // Play SFX
            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(WorldSoundFXManager.Instance.notchArrowSFX));

        }
        #endregion

        #region Release Projectile
        
        [ServerRpc]
        public void NotifyServerOfReleasedProjectileServerRpc(ulong playerClientID, int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
        {
            if (IsServer)
            {
                NotifyClientsOfReleasedProjectileClientRpc(playerClientID, projectileID, xPosition, yPosition, zPosition, yCharacterRotation);  
            }
        }
        [ClientRpc]
        public void NotifyClientsOfReleasedProjectileClientRpc(ulong playerClientID, int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
        {
            if(playerClientID != NetworkManager.Singleton.LocalClientId) // Not call 2 times on the owner
            {
                PerformReleasedProjectileFromRPC(projectileID, xPosition, yPosition, zPosition, yCharacterRotation);
            }            
        }

        private void PerformReleasedProjectileFromRPC(int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
        {
            // The Projectile We Are Firing
            RangedProjectileItem projectileItem = null;

            if(WorldItemDatabase.Instance.GetProjectileByID(projectileID) != null)
            {
                projectileItem = WorldItemDatabase.Instance.GetProjectileByID(projectileID);
            }

            if (projectileItem == null) { return; }

            Transform projectileInstantiationLocation;
            GameObject projectileGameObject;
            Rigidbody projectileRigidbody;
            RangedProjectileDamageCollider projectileDamageCollider;            

            projectileInstantiationLocation = player.playerCombatManager.lockOnTransform;
            projectileGameObject = Instantiate(projectileItem.releaseProjectileModel, projectileInstantiationLocation);
            projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
            projectileRigidbody = projectileGameObject.GetComponent<Rigidbody>();

            // TODO: Make Formula To Set Range Projectile Damage
            projectileDamageCollider.physicalDamage = 100;
            projectileDamageCollider.characterShootingProjectile = player;

            // Fire an arrow based on 1 of 3 varitions

            // Aiming
            if (player.playerNetworkManager.isAiming.Value)
            {
                projectileGameObject.transform.LookAt(new Vector3(xPosition, yPosition, zPosition));
            }
            else
            {
                //  Locked And Not Aiming
                if (player.playerCombatManager.currentTarget != null)
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position
                        - projectileGameObject.transform.position);
                    projectileGameObject.transform.rotation = arrowRotation;
                }
                // Unlocked and not aiming
                else
                {
                    // Hint. If ypu want to do this on your own look at the forward direction value of the camera , and direct the arrww accordingly
                    player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, yCharacterRotation, player.transform.rotation.z);
                    Quaternion arrowRotation = Quaternion.LookRotation(player.transform.forward);
                    projectileGameObject.transform.rotation = arrowRotation;

                }

            }

            // Get all character colliders and ignore self
            Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
            List<Collider> collidersArrowWillIgonore = new List<Collider>();

            foreach (var item in characterColliders) { collidersArrowWillIgonore.Add(item); }
            foreach (Collider hitBox in collidersArrowWillIgonore)
            {
                Physics.IgnoreCollision(projectileDamageCollider.damageCollider, hitBox, true);
            }

            projectileRigidbody.AddForce(projectileGameObject.transform.forward * projectileItem.forwardVelocity);
            projectileGameObject.transform.parent = null;
        }
        #endregion

    }

}
