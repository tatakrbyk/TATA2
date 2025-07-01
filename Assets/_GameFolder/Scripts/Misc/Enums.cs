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
        LeftHand,
        // RightHips,
        // LeftHips,
        // Back
    }
    
    public enum AttackType
    {
        LightAttack01,
        LightAttack02,
              
        HeavyAttack01,
        HeavyAttack02,

        ChargedAttack01,
        ChargedAttack02
    }
}
