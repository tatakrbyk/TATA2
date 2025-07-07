using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

namespace XD
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int vertical;
        int horizontal;

        [Header("Flags")]
        public bool applyRootMotion = false;


        [Header("Damage Animation")]
        public string lastDamageAnimationPlayed;

        [Header("Hit React Animation")]
        [SerializeField] private string hit_React_Forward_Medium_01 = "hit_React_Forward_Medium_01";
        [SerializeField] private string hit_React_Forward_Medium_02 = "hit_React_Forward_Medium_02";

        [SerializeField] private string hit_React_Backward_Medium_01 = "hit_React_Backward_Medium_01";
        [SerializeField] private string hit_React_Backward_Medium_02 = "hit_React_Backward_Medium_02";

        [SerializeField] private string hit_React_Left_Medium_01 = "hit_React_Left_Medium_01";
        [SerializeField] private string hit_React_Left_Medium_02 = "hit_React_Left_Medium_02";

        [SerializeField] private string hit_React_Right_Medium_01 = "hit_React_Right_Medium_01";
        [SerializeField] private string hit_React_Right_Medium_02 = "hit_React_Right_Medium_02";

        public List<string> forward_Medium_Damage = new List<string>();
        public List<string> backward_Medium_Damage = new List<string>();
        public List<string> left_Medium_Damage = new List<string>();
        public List<string> right_Medium_Damage = new List<string>();
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        protected virtual void Start()
        {
            forward_Medium_Damage.Add(hit_React_Forward_Medium_01);
            forward_Medium_Damage.Add(hit_React_Forward_Medium_02);

            backward_Medium_Damage.Add(hit_React_Backward_Medium_01);
            backward_Medium_Damage.Add(hit_React_Backward_Medium_02);

            left_Medium_Damage.Add(hit_React_Left_Medium_01);
            left_Medium_Damage.Add(hit_React_Left_Medium_02);

            right_Medium_Damage.Add(hit_React_Right_Medium_01);
            right_Medium_Damage.Add(hit_React_Right_Medium_02);
        }

        public string GetRandomAnimationFromList(List<string> animationList)
        {
            List<string> finalList = new List<string>();
            foreach (var item in animationList)
            {
                finalList.Add(item);
            }

            finalList.Remove(lastDamageAnimationPlayed);

            for (int i = finalList.Count - 1; i > -1; i--)
            {
                if (finalList[i] == null)
                {
                    finalList.RemoveAt(i);
                }
            }
            int randomValue = Random.Range(0, finalList.Count);
            return finalList[randomValue];
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting)
        {
            float snappedHorizontal;
            float snappedVertical;
            
            if(horizontalValue > 0 && horizontalValue <= 0.5f)
            {
                snappedHorizontal = 0.5f;
            }
            else if (horizontalValue > 0.5f && horizontalValue <= 1)
            {
                snappedHorizontal = 1;
            }
            else if (horizontalValue < 0 && horizontalValue >= -0.5f)
            {
                snappedHorizontal = -0.5f;
            }
            else if (horizontalValue < -0.5f && horizontalValue >= -1) 
            {
                snappedHorizontal = -1f;
            }
            else
            {
                snappedHorizontal = 0f;
            }

            if(verticalValue > 0 && verticalValue <= 0.5f)
            {
                snappedVertical = 0.5f;
            }
            else if (verticalValue > 0.5f && verticalValue <= 1)
            {
                snappedVertical = 1;
            }
            else if (verticalValue < 0 && verticalValue >= -0.5f)
            {
                snappedVertical = -0.5f;
            }
            else if (verticalValue < -0.5f && verticalValue >= -1)
            {
                snappedVertical = -1f;
            }
            else
            {
                snappedVertical = 0f;
            }

            if (isSprinting)
            {
                snappedVertical = 2;
            }
          
            character.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
            character.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        }

        public virtual void PlayActionAnimation(
            string animationName,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false )
        {

            this.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationName, 0.2f);
            // Can be used to stop character from attempting new actions 
            // This flag will turn true if you are stunned
            character.isPerformingAction = isPerformingAction; 
            character.characterLocomotionManager.canRotate = canRotate;
            character.characterLocomotionManager.canMove = canMove;

            // Animation Replicated
            character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, animationName, applyRootMotion);
        }

        public virtual void PlayAttackActionAnimation(AttackType attackType,
            string animationName,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            // Keep Track of last Attack performed ( for combos)
            // Keep Track of attack Type ( Light, Heavy, etc. )
            character.characterCombatManager.currentAttackType = attackType;
            character.characterCombatManager.lastAttackAnimationPerformed = animationName;
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationName, 0.2f);
            character.isPerformingAction = isPerformingAction;
            character.characterLocomotionManager.canRotate = canRotate;
            character.characterLocomotionManager.canMove = canMove;

            // Animation Replicated
            character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, animationName, applyRootMotion);
        }      
    }

    

}
