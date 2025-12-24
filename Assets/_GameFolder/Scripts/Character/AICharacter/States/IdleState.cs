using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace XD
{
    [CreateAssetMenu(menuName = "AI/States/IdleState", fileName = "IdleState")]
    public class IdleState : AIState
    {
        [Header("Idle Options")]
        public IdleStateMode idleStateMode;

        [Header("Patrol Options")]
        public AIPatrolPath patrolPath;
        [SerializeField] bool hasFoundClosestPointNearCharacterSpawn = false; // If the character spawns closer to the second point, start at the second point
        [SerializeField] bool patrolComplete = false; // Have we finished the entire patrol yet
        [SerializeField] bool repeatPatrol = false; // Upon Finishing, do we repeat the path again
        [SerializeField] int patrolDestinationIndex; // Which point of the patrol, are we currently working towards
        [SerializeField] bool hasPatrolDestination = false; // Do we have a point we are currently working towards
        [SerializeField] Vector3 currentPatrolDestination;  // The specific destination coords we are heading towards
        [SerializeField] float distanceFromCurrentDestination; // The distance from te a.i character to the destination
        [SerializeField] float timeBetweenPatrols = 15; // Minimum time before starting a new patrol
        [SerializeField] float restTimer = 0;  // Active Timer counting the time rested

        [Header("Idle Options")]
        [SerializeField] public bool willInvestigateSound = true;
        private bool sleepAnimationSet = false;

        [SerializeField] string sleepAnimation = "Sleep_01";
        [SerializeField] string wakingAnimation = "Wake_01";
        public override AIState Tick(AICharacterManager aiCharacter)
        {

            if(aiCharacter.aiCharacterNetworkManager.isAwake.Value)
            {
                aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);
            }

            switch(idleStateMode)
            {
                case IdleStateMode.Idle: return Idle(aiCharacter);
                case IdleStateMode.Patrol: return Patrol(aiCharacter);
                case IdleStateMode.Sleep: return SleepUntilDisturbed(aiCharacter);
                default:
                    break;
            }

            return this;
        }

        protected virtual AIState Idle(AICharacterManager aiCharacter)
        {
            if(aiCharacter.characterCombatManager.currentTarget != null)
            {
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }
            else
            {
                // Return this state, to continually search for a target (Keep the state here, until a target is found)
                return this;
            }
        }

        protected virtual AIState Patrol(AICharacterManager aiCharacter)
        {
            if (!aiCharacter.aICharacterLocomotionManager.isGrounded) return this;

            if(aiCharacter.isPerformingAction)
            {
                aiCharacter.navMeshAgent.enabled = false;
                aiCharacter.characterNetworkManager.isMoving.Value = false;
                return this;
            }

            if(!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            if(aiCharacter.aiCharacterCombatManager.currentTarget != null)
            {
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }

            // IF  OUR PATROL, IS COMPLETE AND WE WEÝLL REPEAT IT CHECK FOR REST TIME
            if (patrolComplete && repeatPatrol)
            {
                // If the time has not exceeded its set limit, stop and wait
                if (timeBetweenPatrols > restTimer)
                {
                   aiCharacter.navMeshAgent.enabled = false;
                    aiCharacter.characterNetworkManager.isMoving.Value = false;
                    restTimer += Time.deltaTime;
                }
                else
                {
                    patrolDestinationIndex = -1;
                    hasPatrolDestination = false;
                    currentPatrolDestination = aiCharacter.transform.position;
                    patrolComplete = false;
                    restTimer = 0;  
                }
            }
            else if(patrolComplete && !repeatPatrol)
            {
                aiCharacter.navMeshAgent.enabled = false;
                aiCharacter.characterNetworkManager.isMoving.Value = false;
            }

            // Of we have a destination, move towards it
            if (hasPatrolDestination)
            {
                distanceFromCurrentDestination = Vector3.Distance(aiCharacter.transform.position, currentPatrolDestination);

                if (distanceFromCurrentDestination > 2)
                {
                    aiCharacter.navMeshAgent.enabled = true;
                    aiCharacter.aICharacterLocomotionManager.RotateTowardsAgent(aiCharacter);
                }
                else
                {
                    currentPatrolDestination = aiCharacter.transform.position;
                    hasPatrolDestination = false;
                }
            }
            // Otherwise, get a new destination 
            else
            {
                patrolDestinationIndex += 1;

                if (patrolDestinationIndex > patrolPath.patrolWaypoints.Count - 1)
                {
                    patrolComplete = true;
                    return this;
                }

                if(!hasFoundClosestPointNearCharacterSpawn)
                {
                    hasFoundClosestPointNearCharacterSpawn = true;
                    float closestDistance = Mathf.Infinity;

                    for(int i = 0; i < patrolPath.patrolWaypoints.Count; i++)
                    {
                        float distanceFromThisPoint = Vector3.Distance(aiCharacter.transform.position, patrolPath.patrolWaypoints[i]);

                        if (distanceFromThisPoint < closestDistance)
                        {
                            closestDistance = distanceFromThisPoint;
                            patrolDestinationIndex = i;
                            currentPatrolDestination = patrolPath.patrolWaypoints[i];
                        }
                    }
                }
                else
                {
                    currentPatrolDestination = patrolPath.patrolWaypoints[patrolDestinationIndex];
                }
                hasPatrolDestination = true;
            }
            
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(currentPatrolDestination, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }

        protected virtual AIState SleepUntilDisturbed(AICharacterManager aiCharacter)
        {
            aiCharacter.navMeshAgent.enabled = false;

            // If we havent set our sleep animation, and the character is sleepimng set the aniamtion now 
            if(!sleepAnimationSet && !aiCharacter.aiCharacterNetworkManager.isAwake.Value)
            {
                sleepAnimationSet = true;
                aiCharacter.aiCharacterNetworkManager.sleepingAnimation.Value = sleepAnimation;
                aiCharacter.aiCharacterNetworkManager.wakingAnimation.Value = wakingAnimation;
                aiCharacter.characterAnimatorManager.PlayActionAnimation(aiCharacter.aiCharacterNetworkManager.sleepingAnimation.Value.ToString(), true);
            }

            if(aiCharacter.characterCombatManager.currentTarget != null && !aiCharacter.aiCharacterNetworkManager.isAwake.Value)
            {
                aiCharacter.aiCharacterNetworkManager.isAwake.Value = true;

                if(!aiCharacter.isPerformingAction && !aiCharacter.isDead.Value)
                {
                    aiCharacter.characterAnimatorManager.PlayActionAnimation(aiCharacter.aiCharacterNetworkManager.wakingAnimation.Value.ToString(), true);
                }

                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }

            return this;
        }
        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);

            sleepAnimationSet = false;
        }
    }

}
