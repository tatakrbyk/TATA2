using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class AIDurkCombatManager : AICharacterCombatManager
    {
        AIDurkCharacterManager durkManager;

        [Header("Damage Collider")]
        [SerializeField] private DurkClubDamageCollider clubDamageCollider;
        [SerializeField] private DurkStompCollider durkStompCollider;
        public float stompAttackAreaOfEffectRadius = 1.5f;


        [Header("Damage")]
        [SerializeField] private int baseDamage = 25;
        [SerializeField] private int basePoiseDamage = 25;
        [SerializeField] private float attack01DamageModifier = 1.0f;
        [SerializeField] private float attack02DamageModifier = 1.4f;
        [SerializeField] private float attack03DamageModifier = 1.6f;
        public float stompDamage = 25f;

        [Header("VFX")]
        public GameObject durkImpactVFX;
        protected override void Awake()
        {
            base.Awake();

            durkManager = GetComponent<AIDurkCharacterManager>();
        }

        public override void PivotTowardsTarget(AICharacterManager aiCharacterManager)
        {
            if (aiCharacterManager.isPerformingAction) return;

            if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacterManager.characterAnimatorManager.PlayActionAnimation("Turn_Right_90", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacterManager.characterAnimatorManager.PlayActionAnimation("Turn_Left_90", true);
            }
            else if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacterManager.characterAnimatorManager.PlayActionAnimation("Turn_Right_180", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180)
            {
                aiCharacterManager.characterAnimatorManager.PlayActionAnimation("Turn_Left_180", true);
            }
        }
        #region Animation 
        // Call(All region): Durk 01/02

        public void SetAttack01Damage()
        {
            aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
            clubDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
            clubDamageCollider.poiseDamage = basePoiseDamage * attack01DamageModifier;
        }

        public void SetAttack02Damage()
        {
            aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
            clubDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
            clubDamageCollider.poiseDamage = basePoiseDamage * attack02DamageModifier;
        }

        public void SetAttack03Damage()
        {
            aiCharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
            clubDamageCollider.physicalDamage = baseDamage * attack03DamageModifier;
            clubDamageCollider.poiseDamage = basePoiseDamage * attack03DamageModifier;
        }

        public void OpenClubDamageCollider()
        {
            clubDamageCollider.EnableDamageCollider();
            durkManager.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(durkManager.durkSoundFXManager.clubWhooshes));
        }

        public void CloseClubDamageCollider()
        {
            clubDamageCollider.DisableDamageCollider();
        }

        public void ActivateDurkStomp()
        {
            durkStompCollider.StompAttack(); 

        }
        #endregion
    }

}
