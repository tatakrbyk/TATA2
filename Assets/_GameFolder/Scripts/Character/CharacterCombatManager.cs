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

        [Header("Attack Flags")]
        public bool canPerformRollingtAttack = false;
        public bool canPerformBackstepAttack = false;
        public bool canBlock = true;

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
        // Call: Player Roll Animation Events, Fog Walk anim
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

        // Call: Player Roll Animation Events
        public void EnableCanDoRollingAttack()
        {
            canPerformRollingtAttack = true;
        }
        // Call: Player Roll Animation Events
        public void DisableCanDoRollingAttack()
        {
            canPerformRollingtAttack = false;
        }

        // Call: Player Backstep Animation Events
        public void EnableCanDoBackstepAttack()
        {
            canPerformBackstepAttack = true;
        }
        // Call: Player BackStep Animation Events
        public void DisableCanDoBackstepAttack()
        {
            canPerformBackstepAttack = false;
        }

        // Call Animation = "straight_sword_light/Heavy_attack_01/02" + release(+)_full
        public virtual void EnableCanDoCombo()
        {

        }

        public virtual void DisableCanDoCombo()
        {

        }
    }
}
