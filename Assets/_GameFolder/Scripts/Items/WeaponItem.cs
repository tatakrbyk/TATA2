using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class WeaponItem : Item
    {
        // Animator Contoller override (Change attack animations based on weapon type)

        [Header("Weapon Model")]
        public GameObject weaponModel;

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
        // Weapon modifiers
        // Light Attack modifier
        // Heavy Attack modifier
        // Critical damage modifier etc

        [Header("Stamina Cost")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostMultiplier = 0.9f;

        // Runnincg attack stamina cost modifier
        // Heavy attack stamina cost modifier

        // Item based Actions ( RB, RT, LB, LT )
        [Header("Weapon Actions")]
        public WeaponItemAction oh_RB_Action; // One Hand Right Bummper Action
        public WeaponItemAction oh_RT_Action; // One Hand Right Trigger Action

        // Ash of war 

        // Blocking Sounds
        [Header("Whooshes")]
        public AudioClip[] whooshes;

    }

}
