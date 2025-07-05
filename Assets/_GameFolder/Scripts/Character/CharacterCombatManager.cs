using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
namespace XD
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        protected CharacterManager character;

        [Header("Last Attack Animation Performed")]
        public string lastAttackAnimationPerformed;

        [Header("Attack Target")]
        public CharacterManager currentTarget;

        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Attack Type")]
        public AttackType currentAttackType;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void SetTarget(CharacterManager newTarget)
        {
            if(character.IsOwner)
            {
                if(newTarget != null)
                {
                    currentTarget = newTarget;
                    character.characterNetworkManager.currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
                }
                else
                {
                    currentTarget = null;
                }
            }
        }
        // Call: Player Roll Animation Events
        public void EnableIsInvulnerable()
        {
            if (character.IsOwner)
            {
                character.characterNetworkManager.isInvulnerable.Value = true;
            }
        }

        // Call: Player Roll Animation Events
        public void DisableIsInvulnerable()
        {
            if (character.IsOwner)
            { 
                character.characterNetworkManager.isInvulnerable.Value = false;
            }
        }

    }
}
