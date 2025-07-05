using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.AI;

namespace XD
{
    [CreateAssetMenu(menuName = "AI/States/CombatStanceState", fileName = "CombatStanceState")]

    public class CombatStanceState : AIState
    {
        [Header("Attacks")]
        public List<AICharacterAttackAction> aiCharacterAttacks;
        protected List<AICharacterAttackAction> potentialAttacks;
        private AICharacterAttackAction choosenAttack;
        private AICharacterAttackAction previousAttack;
        protected bool hasAttack = false;

        [Header("Combo")]
        [SerializeField] protected bool canPerformCombo = false;
        [SerializeField] protected int chanceToPerformCombp = 25;
        protected bool hasRolledForComboChance = false;

        [Header("Engagement Distance")]
        [SerializeField] public float maximumEngagementDistance = 5f;
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if(aiCharacter.isPerformingAction) { return this; }
            if ((aiCharacter.navMeshAgent.enabled))
            {
                aiCharacter.navMeshAgent.enabled = true;
            }
            
            if(aiCharacter.aiCharacterCombatManager.enablePivot)
            {
                // Rotate to face our target
                if(!aiCharacter.aiCharacterNetworkManager.isMoving.Value)
                {
                    if(aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                    {
                        aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);                        
                    }
                }
            }

            aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);
            if(aiCharacter.aiCharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aiCharacter, aiCharacter.idle);
            }

            if(!hasAttack)
            {
                GetNewAttack(aiCharacter);
            }
            else
            {
                aiCharacter.attack.currentAttack = choosenAttack;
                // roll for combo chance
                return SwitchState(aiCharacter, aiCharacter.attack);
            }

            // If we are outside of the combat engagement distance, switch to pursue states
            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
            {
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }

        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            potentialAttacks = new List<AICharacterAttackAction>();

            foreach(var potentialAttack in aiCharacterAttacks)
            {
                // If we are too close for this attack, check the next
                if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                { continue; }

                // If we are too far for this attack, check the next
                if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                { continue; }

                // If the target is outside minimum F.O.V  for this attack, check the next
                if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                { continue; }

                // If the target is outside minimum F.O.V  for this attack, check the next
                if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.viewableAngle)
                { continue; }

                potentialAttacks.Add(potentialAttack);

                if(potentialAttacks.Count < 0) { return; }

                var totalWeight = 0;

                foreach(var attack in potentialAttacks)
                {
                    totalWeight += attack.attackWeight;
                }

                var randomWeightValue = Random.Range(0, totalWeight+1);
                var processedWeight = 0;

                foreach (var attack in potentialAttacks)
                {
                    processedWeight += attack.attackWeight;
                    if(randomWeightValue <=  processedWeight)
                    {
                        choosenAttack = attack;
                        previousAttack = choosenAttack;
                        hasAttack = true;
                        return;
                    }
                }
            }
        }

        protected virtual bool RollForOutcomeChance(int outcomeChance)
        {
            bool outcomeWillBePerformed = false;
            int randomPercentage = Random.Range(0, 100);

            if(randomPercentage < outcomeChance)
            {
                outcomeWillBePerformed = true;
            }
            return outcomeWillBePerformed;
        }

        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);

            hasAttack = false;
            hasRolledForComboChance = false;
        }
    }

}
