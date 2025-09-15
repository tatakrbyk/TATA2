using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        protected AICharacterManager aiCharacter;

        [Header("Recovery Timer")]
        public float actionRecoveryTimer = 0;

        [Header("Pivot")]
        public bool enablePivot = true;
        [Header("Target Info")]
        public float distanceFromTarget;
        public float viewableAngle;
        public Vector3 targetsDirection;

        [Header("Detection")]
        [SerializeField] private float detectionRadius = 15f;
        public float minimumFOV = -35f;
        public float maximumFOV = 35f;

        [Header("Attack Rotation Speed")]
        public float attackRotationSpeed = 25f;

        [Header("Stance/Posture Settings")]
        public float maxStance = 150;
        public float currentStance;
        [SerializeField] float stanceRegeneratedPersecond = 15f;
        [SerializeField] private bool ignoreStanceBreak = false;

        [Header("Stance/Posture Timer")]
        [SerializeField] private float stanceRegenerationTimer = 0;  // player continue hit not regenerate stance
        [SerializeField] private float defaultTimeUntilStanceRegenerationBegins = 15f;
        private float stanceTickTimer = 0;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
            lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
        }

        private void FixedUpdate()
        {
            HandleStanceBreak();
        }
        private void HandleStanceBreak()
        {
            if (!aiCharacter.IsOwner) { return; }
            if (aiCharacter.isDead.Value) { return; }

            if (stanceRegenerationTimer > 0)
            {
                stanceRegenerationTimer -= Time.deltaTime;
            }
            else
            {
                stanceRegenerationTimer = 0;

                if (currentStance < maxStance)
                {
                    currentStance += stanceRegeneratedPersecond * Time.deltaTime;

                    stanceTickTimer += Time.deltaTime;
                    if (stanceTickTimer >= 1)
                    {
                        stanceTickTimer = 0;
                        currentStance += stanceRegeneratedPersecond;
                    }
                }
                else
                {
                    currentStance = maxStance;
                }
            }

            if(currentStance <= 0)
            {
                DamageIntensity previousDamageIntensity = WorldUtilityManager.Instance.GetDamageIntensityBasedOnPoiseDamage(previousPoiseDamageTaken);

                if(previousDamageIntensity == DamageIntensity.Colossal)
                {
                    currentStance = 1;
                    return;
                }

                currentStance = maxStance;

                if(ignoreStanceBreak) { return; }

                aiCharacter.characterAnimatorManager.PlayActionAnimation("Stance_Break_01", true);
            }
        }

        public void DamageStance(int stanceDamage)
        {
            // When stance is damaged, The timer is reset, Meaning Constant attacks give no chance at recovering stance that is lost 
            stanceRegenerationTimer = defaultTimeUntilStanceRegenerationBegins;
            currentStance -= stanceDamage;
        }
        public void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
        {
            if (currentTarget != null) return;

            Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.Instance.GetCharacterLayer());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                if(targetCharacter == null) continue;
                if(targetCharacter == aiCharacter) continue;
                if(targetCharacter.isDead.Value) continue;

                if(WorldUtilityManager.Instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
                {
                    Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                    float angleOfPotentialTarget = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                    if(angleOfPotentialTarget > minimumFOV && angleOfPotentialTarget < maximumFOV)
                    {
                        //  we check for environment blocks
                        if(Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position, WorldUtilityManager.Instance.GetEnvironmentLayers()))
                        {
                            Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                            
                        }
                        else
                        {
                            targetsDirection = targetCharacter.transform.position - transform.position;
                            viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, targetsDirection);
                            aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                            
                            if (enablePivot) {
                                PivotTowardsTarget(aiCharacter);}
                        }
                    }
                }  


            }
        }

        public virtual void PivotTowardsTarget(AICharacterManager aiCharacterManager)
        { 
            if(aiCharacterManager.isPerformingAction) return;

            if(viewableAngle >= 20 && viewableAngle <= 60)
            {
                aiCharacterManager.characterAnimatorManager.PlayActionAnimation("Zombie_Turn_Right_45_01", true);
            }
            else if (viewableAngle <= -20 && viewableAngle >= -60)
            {
                aiCharacterManager.characterAnimatorManager.PlayActionAnimation("Zombie_Turn_Left_45_01", true);
            }
            else if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacterManager.characterAnimatorManager.PlayActionAnimation("Zombie_Turn_Right_90_01", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacterManager.characterAnimatorManager.PlayActionAnimation("Zombie_Turn_Left_90_01", true);
            }
            else if (viewableAngle >= 110 && viewableAngle <= 145)
            {
                aiCharacterManager.characterAnimatorManager.PlayActionAnimation("Zombie_Turn_Right_135_01", true);
            }
            else if (viewableAngle <= -110 && viewableAngle >= -145)
            {
                aiCharacterManager.characterAnimatorManager.PlayActionAnimation("Zombie_Turn_Left_135_01", true);
            }
            else if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacterManager.characterAnimatorManager.PlayActionAnimation("Zombie_Turn_Right_180_01", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180 )
            {
                aiCharacterManager.characterAnimatorManager.PlayActionAnimation("Zombie_Turn_Left_180_01", true);
            }
        }

        public void RotateTowardsAgent(AICharacterManager aICharacter)
        {
            if(aICharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                aICharacter.transform.rotation = aICharacter.navMeshAgent.transform.rotation;
            }
        }

        public void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacter)
        {
            if(currentTarget == null) {  return; }
            if(!aiCharacter.aICharacterLocomotionManager.canRotate) { return; }
            if(!aiCharacter.isPerformingAction) { return; }

                
            Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();

            if(targetDirection == Vector3.zero)
            {
                targetDirection = aiCharacter.transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
        }
        public void HandleActionRecovery(AICharacterManager aiCharacter)
        {
            if(actionRecoveryTimer > 0)
            {
                if(!aiCharacter.isPerformingAction)
                {
                    actionRecoveryTimer -= Time.deltaTime;
                }
            }
        }
    }

}

