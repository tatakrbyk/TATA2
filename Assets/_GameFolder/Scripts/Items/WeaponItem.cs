using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class WeaponItem : EquipmentItem
    {
        [Header("Animations")]
        public AnimatorOverrideController weaponAnimator;

        [Header("Model Instatiation")]
        public WeaponModelType weaponModelType;

        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Class")]
        public WeaponClass weaponClass;

        [Header("Weapon Requirements")]
        public int strenghtREQ = 0;
        public int dexREQ = 0; // DEXterity
        public int intREQ = 0; // INTelligence
        public int faithREQ = 0; 
        
        [Header("Weapon Base Damage")] 
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int holyDamage = 0;
        public int lightningDamage = 0;


        // Weapon guard absorptions 

        [Header("Weapon Poise)")]
        public float poiseDamage = 10;
        // Offensive poise bonus when attacking

        [Header("Attack Modifiers")]
        public float light_Attack_01_Modifier = 1.0f;
        public float light_Attack_02_Modifier = 1.2f;
        public float heavy_Attack_01_Modifier = 1.4f;
        public float heavy_Attack_02_Modifier = 1.6f;
        public float charge_Attack_01_Modifier = 2.0f;
        public float charge_Attack_02_Modifier = 2.2f;
        public float running_Attack_01_Modifier = 1.1f;
        public float rolling_Attack_01_Modifier = 1.1f;
        public float backstep_Attack_01_Modifier = 1.1f;
        
        // Weapon modifiers
        // Light Attack modifier
        // Heavy Attack modifier
        // Critical damage modifier etc

        [Header("Stamina Cost")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostMultiplier = 0.9f;
        public float heavyAttackStaminaCostMultiplier = 1.3f;
        public float chargedAttackStaminaCostMultiplier = 1.5f;
        public float runningAttackStaminaCostMultiplier = 1.1f;
        public float rollingAttackStaminaCostMultiplier = 1.1f;
        public float backstepAttackStaminaCostMultiplier = 1.1f;

        [Header("Weapon Blocking Absorptions")]
        public float physicalBaseDamageAbsorption = 50;
        public float magicBaseDamageAbsorption = 50;
        public float fireBaseDamageAbsorption = 50;
        public float holyBaseDamageAbsorption = 50;
        public float lightningBaseDamageAbsorption = 50;
        public float stability = 50;  // Reduce stamina lost from block


        // Runnincg attack stamina cost modifier
        // Heavy attack stamina cost modifier

        // Item based Actions ( RB, RT, LB, LT )
        [Header("Weapon Actions")]
        public WeaponItemAction oh_RB_Action; // One Hand Right Bummper Action
        public WeaponItemAction oh_RT_Action; // One Hand Right Trigger Action
        public WeaponItemAction oh_LB_Action; // One Hand Left Bummper Action
        public AshOfWar ashOfWarAction; // oh_LT_Action; // One Hand Left Trigger Action

        // Ash of war 

        // Blocking Sounds
        [Header("SFX")]
        public AudioClip[] whooshes;
        public AudioClip[] blocking;

    }

}
