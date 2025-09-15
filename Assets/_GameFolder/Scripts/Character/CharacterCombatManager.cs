using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Threading;
namespace XD
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        protected CharacterManager character;

        [Header("Last Attack Animation Performed")]
        public string lastAttackAnimationPerformed;

        [Header("Previous Poise Damage Taken")]
        public float previousPoiseDamageTaken;

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

        [Header("Critical Attack")]
        [SerializeField] private float criticalAttackDistanceCheck = 0.7f;
        public int pendingCriticalDamage;
        private Transform riposteReceiverTransform;
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

        public virtual void AttemptCriticalAttack()
        {
            if(character.isPerformingAction) { return; }
            if(character.characterNetworkManager.currentStamina.Value <= 0) { return; }
            
            RaycastHit[] hits = Physics.RaycastAll(character.characterCombatManager.lockOnTransform.position,
                character.transform.TransformDirection(Vector3.forward), criticalAttackDistanceCheck, WorldUtilityManager.Instance.GetCharacterLayer());

            for(int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                CharacterManager targetCharacter = hit.transform.GetComponent<CharacterManager>();

                if(targetCharacter != null)
                {
                    if(targetCharacter == character) { continue; }
                    if(!WorldUtilityManager.Instance.CanIDamageThisTarget(character.characterGroup, targetCharacter.characterGroup)) { continue; }

                    Vector3 directionFromCharacterToTarget = character.transform.position - targetCharacter.transform.position;
                    float targetViewableAngle = Vector3.SignedAngle(directionFromCharacterToTarget, targetCharacter.transform.forward, Vector3.up);

                    if(targetCharacter.characterNetworkManager.isRipostable.Value)
                    {
                        if(targetViewableAngle >= -60 && targetViewableAngle <= 60)
                        {
                            AttemptRiposte(hit);
                            Debug.Log("Attempting Riposte");
                            break;
                        }
                    }
                }
            }
        }

        // Call Ripsoted_01 animation
        public virtual void AttemptRiposte(RaycastHit hit)
        {

        }

        public virtual void ApplyCriticalDamage()
        {
            character.characterEffectsManager.PlayCriticalBloodSplatterVFX(character.characterCombatManager.lockOnTransform.position);
            character.characterSoundFXManager.PlayCriticalStrikeSoundFX();

            if (character.IsOwner)
            {
                character.characterNetworkManager.currentHealth.Value -= pendingCriticalDamage;
            }
        }
        public IEnumerator ForceMoveEnemyCharacterToRipostePosition(CharacterManager enemyCharacter , Vector3 ripostePosition)
        {
            float timer = 0; 

            while(timer < 0.5f)
            {
                timer += Time.deltaTime;

                if(riposteReceiverTransform == null)
                {
                    GameObject riposteTransformObject = new GameObject("Riposte Receiver Transform");
                    riposteTransformObject.transform.parent = transform;
                    riposteTransformObject.transform.position = Vector3.zero;
                    riposteReceiverTransform = riposteTransformObject.transform;
                }

                riposteReceiverTransform.localPosition = ripostePosition;
                enemyCharacter.transform.position = riposteReceiverTransform.position;
                transform.rotation = Quaternion.LookRotation(-enemyCharacter.transform.forward);
                yield return null;
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

        // Call: Stance_Break_01 (core_main_Stance_broken_f_01)
        public void EnableIsRipostable()
        {
            if (character.IsOwner)
            {
                character.characterNetworkManager.isRipostable.Value = true;
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
