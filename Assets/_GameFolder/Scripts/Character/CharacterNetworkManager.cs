using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using XD;

namespace XD
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        CharacterManager character;

        [Header("Active")]
        public NetworkVariable<bool> isActive = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Position")]    
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;


        [Header("Animator")]
        public NetworkVariable<bool> isMoving = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> moveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Target")]
        public NetworkVariable<ulong> currentTargetNetworkObjectID = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Flags")]
        public NetworkVariable<bool> isBlocking = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isParrying = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isParryable = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isAttacking = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isInvulnerable = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isLockedOn = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isJumping = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isChargingAttack = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isRipostable = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isBeginCriticallyDamaged = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Resources")]
        public NetworkVariable<float> currentStamina = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxStamina = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        public NetworkVariable<int> currentHealth = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxHealth = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<int> currentFocusPoints = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxFocusPoints = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [Header("Stats")]
        public NetworkVariable<int> vitality = new NetworkVariable<int>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> endurance = new NetworkVariable<int>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> mind = new NetworkVariable<int>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> strength = new NetworkVariable<int>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void CheckHP(int oldValue, int newValue)
        {
            if(currentHealth.Value <= 0)
            {
                StartCoroutine(character.ProcessDeathEvent());
            }
            if(character.IsOwner)
            {
                if(currentHealth.Value > maxHealth.Value)
                {
                    currentHealth.Value = maxHealth.Value;
                }
            }
        }

        public virtual void OnIsDeadChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("IsDead", character. isDead.Value);     
        }
        public void OnLockOnTargetIDChange(ulong oldID, ulong newID)
        {
            // If (IsOwner) charactercommbatmanager set target
            if(!IsOwner)
            {
                character.characterCombatManager.currentTarget = NetworkManager.Singleton.SpawnManager.SpawnedObjects[newID].gameObject.GetComponent<CharacterManager>();
            }
        }
        public void OnIsLockedOnChanged(bool old, bool isLockedOn)
        {
            if (!isLockedOn)
            {
                character.characterCombatManager.currentTarget = null;
            }
        }
        public void OnIsChargingAttackChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("IsChargingAttack", isChargingAttack.Value);

        }

        public void OnIsMovingChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("IsMoving", isMoving.Value);
        }

        public virtual void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            gameObject.SetActive(isActive.Value);
        }

        public virtual void OnIsBlockingChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("IsBlocking", isBlocking.Value);
        }

        #region Destroy All Current Action FX

        // Used To Cancel FX When Poise Broken
        [ServerRpc]
        public void DestroyAllCurrentActionFXServerRPC()
        {
            if(IsServer)
            {
                DestroyAllCurrentActionFXClientRPC();
            }
        }

        [ClientRpc]
        public void DestroyAllCurrentActionFXClientRPC()
        {
            if(character.characterEffectsManager.activeSpellWarmUpFX != null)
            {
                Destroy(character.characterEffectsManager.activeSpellWarmUpFX);
            }
        }

        #endregion

        #region Animation Action
        [ServerRpc]
        public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            // If the character is the host/server, we play the animation for all clients (client Rpc)
            if(IsServer)
            {
                PlayActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion); 
            }
        }

        [ClientRpc]
        public void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            // We make sure to not run the function on the character who sent it ( So we dont play the animation twice)
            if(clientID != NetworkManager.Singleton.LocalClientId) 
            {
                PerformActionAnimationFromServer(animationID, applyRootMotion);
            }
        }

        private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, 0.2f);
        }

        // Instantly

        [ServerRpc]
        public void NotifyTheServerOfInstantActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            // If the character is the host/server, we play the animation for all clients (client Rpc)
            if (IsServer)
            {
                PlayInstantActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }

        [ClientRpc]
        public void PlayInstantActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            // We make sure to not run the function on the character who sent it ( So we dont play the animation twice)
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformInstantActionAnimationFromServer(animationID, applyRootMotion);
            }
        }

        private void PerformInstantActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.Play(animationID);
        }
        #endregion
        #region Attack Action Animation
        [ServerRpc]
        public void NotifyTheServerOfAttackActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            // If the character is the host/server, we play the animation for all clients (client Rpc)
            if (IsServer)
            {
                PlayAttackActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }

        [ClientRpc]
        public void PlayAttackActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            // We make sure to not run the function on the character who sent it ( So we dont play the animation twice)
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformAttackActionAnimationFromServer(animationID, applyRootMotion);
            }
        }

        private void PerformAttackActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, 0.2f);
        }
        #endregion

        #region Damage
        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfCharacterDamageServerRpc(
            ulong damageCharacterID, ulong characterCausingDamageID,
            float physicalDamage,  float magicDamage, float fireDamage, float holyDamage, float poiseDamage,
            float angleHitFrom, float contactPointX, float contactPointY, float contactPointZ)
        {
            if(IsServer)
            {
                NotifyTheServerOfCharacterDamageClientRpc(
                    damageCharacterID, characterCausingDamageID,
                    physicalDamage, magicDamage, fireDamage, holyDamage, poiseDamage,
                    angleHitFrom, contactPointX, contactPointY, contactPointZ);
            }
        }

        [ClientRpc]
        public void NotifyTheServerOfCharacterDamageClientRpc(
            ulong damageCharacterID, ulong characterCausingDamageID,
            float physicalDamage, float magicDamage, float fireDamage, float holyDamage, float poiseDamage,
            float angleHitFrom, float contactPointX, float contactPointY, float contactPointZ)
        {
            ProcessCharacterDamageFromServer(
                   damageCharacterID, characterCausingDamageID,
                   physicalDamage, magicDamage, fireDamage, holyDamage, poiseDamage,
                   angleHitFrom, contactPointX, contactPointY, contactPointZ);

        }

        private void ProcessCharacterDamageFromServer(
            ulong damageCharacterID, ulong characterCausingDamageID,
            float physicalDamage, float magicDamage, float fireDamage, float holyDamage, float poiseDamage,
            float angleHitFrom, float contactPointX, float contactPointY, float contactPointZ)
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damageCharacterID].gameObject.GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>(); 
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeDamageEffect);

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            
            damageEffect.angleHitFrom = angleHitFrom;
            damageEffect.contactPoint = new Vector3(contactPointX, contactPointY, contactPointZ);
            damageEffect.characterCausingDamage = characterCausingDamage;
            
            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }
        #endregion

        #region Riposte (Critical Attack)

        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfRiposteServerRPC(
            ulong damageCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation, int weaponID,
            float physicalDamage, float magicDamage, float fireDamage, float holyDamage, float poiseDamage
            )
        {
            if (IsServer)
            {
                NotifyTheServerOfRiposteClientRPC(
                    damageCharacterID, characterCausingDamageID, criticalDamageAnimation, weaponID,
                    physicalDamage, magicDamage, fireDamage, holyDamage, poiseDamage
                    );
            }
        }

        [ClientRpc]
        public void NotifyTheServerOfRiposteClientRPC(
            ulong damageCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation, int weaponID,
            float physicalDamage, float magicDamage, float fireDamage, float holyDamage, float poiseDamage)
        {
            ProcessRiposteFromServer(
                   damageCharacterID, characterCausingDamageID, criticalDamageAnimation, weaponID,
                   physicalDamage, magicDamage, fireDamage, holyDamage, poiseDamage
                   );

        }

        private void ProcessRiposteFromServer(
            ulong damageCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation, int weaponID,
            float physicalDamage, float magicDamage, float fireDamage, float holyDamage, float poiseDamage)
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damageCharacterID].gameObject.GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeCriticalDamageEffect);

            if(damagedCharacter.IsOwner)
            {
                damagedCharacter.characterNetworkManager.isBeginCriticallyDamaged.Value = true;
            }

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;

            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);
            
            if(damagedCharacter.IsOwner)
            {            
                damagedCharacter.characterAnimatorManager.PlayActionAnimationInstantly(criticalDamageAnimation, true);
            }


            // Move the enemy to the proper riposte position

            StartCoroutine(damagedCharacter.characterCombatManager.ForceMoveEnemyCharacterToRipostePosition(characterCausingDamage, WorldUtilityManager.Instance.GetRipostingPositionBasedOnWeaponClass(weapon.weaponClass)));
        }
        #endregion

        #region Backstab (Critical Attack)

        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfBackstabServerRPC(
            ulong damageCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation, int weaponID,
            float physicalDamage, float magicDamage, float fireDamage, float holyDamage, float poiseDamage
            )
        {
            if (IsServer)
            {
                NotifyTheServerOfRiposteClientRPC(
                    damageCharacterID, characterCausingDamageID, criticalDamageAnimation, weaponID,
                    physicalDamage, magicDamage, fireDamage, holyDamage, poiseDamage
                    );
            }
        }

        [ClientRpc]
        public void NotifyTheServerOfBackstabClientRPC(
            ulong damageCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation, int weaponID,
            float physicalDamage, float magicDamage, float fireDamage, float holyDamage, float poiseDamage)
        {
            ProcessBackstabFromServer(
                   damageCharacterID, characterCausingDamageID, criticalDamageAnimation, weaponID,
                   physicalDamage, magicDamage, fireDamage, holyDamage, poiseDamage
                   );

        }

        private void ProcessBackstabFromServer(
            ulong damageCharacterID,
            ulong characterCausingDamageID,
            string criticalDamageAnimation, int weaponID,
            float physicalDamage, float magicDamage, float fireDamage, float holyDamage, float poiseDamage)
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damageCharacterID].gameObject.GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeCriticalDamageEffect);

            if (damagedCharacter.IsOwner)
            {
                damagedCharacter.characterNetworkManager.isBeginCriticallyDamaged.Value = true;
            }

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;

            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);
            damagedCharacter.characterAnimatorManager.PlayActionAnimationInstantly(criticalDamageAnimation, true);


            // Move the backstab target to the position of the backstabber

            StartCoroutine(characterCausingDamage.characterCombatManager.ForceMoveEnemyCharacterToBackstabPosition(damagedCharacter, WorldUtilityManager.Instance.GetBackstabPositionBasedOnWeaponClass(weapon.weaponClass)));
        }
        #endregion
        #region Parry
        [ServerRpc(RequireOwnership = false)]
        public void NotifyServerOfParryServerRpc(ulong parriedClientID)
        { 
            if(IsServer)
            { 
                NotifyServerOfParryClientRpc(parriedClientID);
            }
        }

        [ClientRpc]
        protected void NotifyServerOfParryClientRpc(ulong parriedClientID)
        {
            ProcessParryFromServer(parriedClientID);
        }

        protected void ProcessParryFromServer(ulong parriedClient)
        {
            CharacterManager parriedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[parriedClient].gameObject.GetComponent<CharacterManager>();

            if(parriedCharacter == null) { return; }

            if (parriedCharacter.IsOwner)
            {
                parriedCharacter.characterAnimatorManager.PlayActionAnimationInstantly("Parried_01", true);
            }
        }
        #endregion
    }
}
