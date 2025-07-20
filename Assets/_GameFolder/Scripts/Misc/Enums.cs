using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class Enums 
    {
        
    }

    public enum CharacterSlot
    {
        CharacterSlot_01,
        CharacterSlot_02,
        CharacterSlot_03,
        CharacterSlot_04,
        CharacterSlot_05,
        CharacterSlot_06,
        CharacterSlot_07,
        CharacterSlot_08,
        CharacterSlot_09,
        CharacterSlot_10,
        NO_SLOT
    }

    public enum CharacterGroup
    {
        Team01,
        Team02 // AI 
    }

    public enum WeaponModelSlot
    {
        RightHand,
        LeftHandWeaponSlot,
        LeftHandShieldSlot,
        BackSlot
        // RightHips,
        // LeftHips,
        
    }

    public enum WeaponModelType
    {
        Weapon,
        Shield,
    }

    public enum WeaponClass
    {
        StraightSword,
        Spear,
        MediumShield,
        Fist
    }
    
    public enum AttackType
    {
        LightAttack01,
        LightAttack02,
              
        HeavyAttack01,
        HeavyAttack02,

        ChargedAttack01,
        ChargedAttack02,
        RunningAttack01,
        RollingAttack01,
        BackstepAttack01,

    }

    public enum DamageIntensity
    {
        Ping,
        Light,
        Medium,
        Heavy,
        Colossal
    }
}
