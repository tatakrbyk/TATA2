using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace XD
{
    [CreateAssetMenu(menuName = "AI/States/PursueTargetState", fileName = "PursueTargetState")]
    public class PursueTargetState : AIState
    {
        
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            // Check if we are performing an action ( If so do nothing until action is complete )
            if(aiCharacter.isPerformingAction) { return this; }

            // Check if our target is null , return to idle state
            if(aiCharacter.aiCharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aiCharacter, aiCharacter.idle);
            }

            // Navmesh Agent active?
            if(!aiCharacter.navMeshAgent.enabled)
            {
                aiCharacter.navMeshAgent.enabled = true;
            }

            // If target goes outside of the characters Field Of View, pivot to face them
            if(aiCharacter.aiCharacterCombatManager.enablePivot)
            {
                if(aiCharacter.aiCharacterCombatManager.viewableAngle < aiCharacter.aiCharacterCombatManager.minimumFOV || aiCharacter.aiCharacterCombatManager.viewableAngle > aiCharacter.aiCharacterCombatManager.maximumFOV)
                {
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
                }
                
            }
            aiCharacter.aICharacterLocomotionManager.RotateTowardsAgent(aiCharacter);


            if(aiCharacter.aiCharacterCombatManager.distanceFromTarget <= aiCharacter.navMeshAgent.stoppingDistance )
            {
                return SwitchState(aiCharacter, aiCharacter.combatStance);
            }


            // TODO: If we are within combat range, switch to combat state

            // TODO: If the target is not reachable, and they are far away, return base

            // Pursue the target

            // Option 1
            //aiCharacter.navMeshAgent.SetDestination(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position);

            // Option 2
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;

        }
    }

}
