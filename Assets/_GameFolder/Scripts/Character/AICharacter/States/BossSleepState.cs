using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "AI/States/BossSleepState", fileName = "BossSleepState")]
    public class BossSleepState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            return base.Tick(aiCharacter);
        }
    }

}
