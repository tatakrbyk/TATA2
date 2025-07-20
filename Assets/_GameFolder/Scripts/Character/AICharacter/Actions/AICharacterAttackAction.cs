using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{   
    [CreateAssetMenu(menuName = "AI/Actions/Attack", fileName = "AIAttackAction")]
    public class AICharacterAttackAction : ScriptableObject
    {
        [Header("Attack")]
        [SerializeField] private string attackActionName;

        [Header("Combo")]
        public AICharacterAttackAction comboAction;

        [Header("Action Values")]
        [SerializeField] AttackType attackType;
        public int attackWeight = 50;

        public float actionRecoveryTimer = 1.5f;
        public float minimumAttackAngle = -35;
        public float maximumAttackAngle = 35;
        public float minimumAttackDistance = 0;
        public float maximumAttackDistance = 2;

        public void AttemptToPerformAction(AICharacterManager aiCharacter)
        {
            aiCharacter.characterAnimatorManager.PlayActionAnimation(attackActionName, true);
        }
    }

}
