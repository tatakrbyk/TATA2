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
        public void SetNewMaxHealthValue(int oldValue, int newValue)
        {
            maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnVitalityLevel(newValue);
            PlayerUIManager.Instance.playerUIHUDManager.SetMaxHealthValue(maxHealth.Value);
            currentHealth.Value = maxHealth.Value;
        }

        public void SetNewMaxStaminaValue(int oldValue, int newValue)
        {
            maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newValue);
            PlayerUIManager.Instance.playerUIHUDManager.SetMaxStaminaValue(maxStamina.Value);
            currentStamina.Value = maxStamina.Value;
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
            SpellItem newSpell = Instantiate(WorldItemDatabase.Instance.GetSpellByID(newID));

            if(newSpell != null)
            {
                player.playerInventoryManager.currentSpell = newSpell;
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

        #endregion
    }

}
