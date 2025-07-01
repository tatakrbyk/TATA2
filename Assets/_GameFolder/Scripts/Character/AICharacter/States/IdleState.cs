using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "AI/States/IdleState", fileName = "IdleState")]
    public class IdleState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
//            return base.Tick(aiCharacter);

            if(aiCharacter.characterCombatManager.currentTarget != null)
            {
                //Debug.Log("We have a target");
                return SwitchState(aiCharacter, aiCharacter.pursueTarget); 
            }
            else
            {
                // Return this state, to continually search for a target ( Keep the state here, until a target is found)
                //Debug.Log("Searchin Target");
                aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);
                return this;    
            }

        }
    }

}
