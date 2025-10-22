using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class Enums 
    {
        
    }

    // Used for Character Data Saving
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

    // Used for To Process Damage, And Character Targetting
    public enum CharacterGroup
    {
        Team01,// Player
        Team02 // AI 
    }

    // Used As A Tag For Each Weapýýn Model Instantiation Slot
    public enum WeaponModelSlot
    {
        RightHand,
        LeftHandWeaponSlot,
        LeftHandShieldSlot,
        BackSlot
        // RightHips,
        // LeftHips,
    }

    // Used To Know Where To Instantiate The Weapon Model Based On Model Type
    public enum WeaponModelType
    {
        Weapon,
        Shield,
    }

    // Used For Any Information Specific To A Weapons Class, Such As Being Able To Riposte ETC
    public enum WeaponClass
    {
        StraightSword,
        Spear,
        MediumShield,
        Fist,
        LightShield,
        Bow
    }

    // Used To Determina Which Item (Catalyst) is needed To Cast Spell
    public enum SpellClass
    {
        Incantation,
        Sorcery
    }

    // Used To Determine Which Ranged Weapon Can Fire This Ammo
    public enum ProjectileClass
    {
        Arrow,
        Bolt
    }
    
    public enum ProjectileSlot
    {
        Main,
        Secondary
    }

    // Used To Tag Equipment Models With Specific Body Parts That They Will Cover
    public enum EquipmentModelType
    {
        FullHelmet,    // Would Always Hide Face, Hair ECT
        Hat,    // Would Always Hide Hair
        Hood,         // Would Always Hide Hair
        HelmetAcessorie,
        FaceCover,
        Torso,
        Back,
        RightShoulder,
        RightUpperArm,
        RightElbow,
        RightLowerArm,
        RightHand,
        LeftShoulder,
        LeftUpperArm,
        LeftElbow,
        LeftLowerArm,
        LeftHand,
        Hips,
        HipsAttachment,
        RightLeg,
        RightKnee,
        LeftLeg,
        LeftKnee
    }

    // USED TO DETERMINE WHICH EQUIPMENT SLOT IS CURRENTLY SELECTED (Helmet, Body, Legs ETC)
    public enum EquipmentType
    {
        RightWeapon01,      // 0
        RightWeapon02,      // 1
        RightWeapon03,      // 2    
        LeftWeapon01,       // 3
        LeftWeapon02,       // 4
        LeftWeapon03,       // 5
        Head,               // 6
        Body,               // 7
        Legs,               // 8
        Hands,              // 9
    }

    // Used To Tag Helmet Type, So Specific Head Portions Can Be Hidden During Equip Process ( Hair, Beard, Face ECT )
    public enum HeadEquipmentType
    {
        FullHelmet, // Hide Entire Head + Features  
        Hat,        // Does Not Hide Anything
        Hood,       // Hides Hair    
        FaceCover   // Hides Beard       
    }

    // Used To Calculate Damage Based On Attack Type
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

        LightJumpingAttack01,
        HeavyJumpingAttack01, 

    }

    // Used To Calculate Damage Animation Intensity
    public enum DamageIntensity
    {
        Ping,
        Light,
        Medium,
        Heavy,
        Colossal
    }

    // Used To Determine Item PickUp Type
    public enum ItemPickUpType
    {
        WorldSpawn,
        CharacterDrop
    }
}
