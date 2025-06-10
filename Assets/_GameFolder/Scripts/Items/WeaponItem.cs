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

        // Weapon modifiers
        // Light Attack modifier
        // Heavy Attack modifier
        // Critical damage modifier etc

        [Header("Stamina Cost")]
        public int baseStaminaCost = 20;
        // Runnincg attack stamina cost modifier
        // Light attack stamina cost modifier
        // Heavy attack stamina cost modifier

        // Item based Actions ( RB, RT, LB, LT )

        // Ash of war 

        // Blocking Sounds

    }

}
