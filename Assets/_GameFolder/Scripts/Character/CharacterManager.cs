using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using SG;

// Player And AI base sc
namespace XD
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Status")]
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;

        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;
        [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool isGrounded = true;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;


        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        }
        
        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }
        protected virtual void Update()
        {
            animator.SetBool("IsGrounded", isGrounded);
            // If this character is being controlled from our side, then assign its network position to the position of our transform
            if(IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            // If this character is being controlled from else where, then assign its position here locally by the position of its network transform
            else
            {   // AI, other plaeyer vs.

                // Position
                transform.position = Vector3.SmoothDamp(transform.position,
                    characterNetworkManager.networkPosition.Value,
                    ref characterNetworkManager.networkPositionVelocity, 
                    characterNetworkManager.networkPositionSmoothTime);

                // Rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                    characterNetworkManager.networkRotation.Value,
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate()
        {
            
        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if(IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                // Reset Any Flags here 

                // if we are not grounded, play an aerial death animation

                if(!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayActionAnimation("Death_01", true);  
                }
            }

            // Play Death SFX

            yield return new WaitForSeconds(5f);

            // Award Players With Runes

            // Disable Character
        }

        public virtual void ReviveCharacter()
        {

        }
        
        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();
            List<Collider> collidersToIgnore = new List<Collider>();

            foreach (var collider in damageableCharacterColliders)
            {
                collidersToIgnore.Add(collider);
            }
            collidersToIgnore.Add(characterControllerCollider);

            foreach (var collider in collidersToIgnore)
            {
                foreach(var otherCollider in collidersToIgnore)
                {
                        Physics.IgnoreCollision(collider, otherCollider, true);
                }
            }
        }
    }
}
