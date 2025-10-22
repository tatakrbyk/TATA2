using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class ArmorItem : EquipmentItem
    {
        [Header("Equipment Absorption Bonus")]
        public float physicalDamageAbsorption;
        public float magicDamageAbsorption;
        public float fireDamageAbsorption;
        public float lightningDamageAbsorption;
        public float holyDamageAbsorption;

        [Header("Equipment Resistance Bonus")]
        public float immunity;          // Resistance To Rot and Poison
        public float robustness;        // Resistance To Bleed and Frostbite
        public float focus;             // Resistance To Sleep and Madness
        public float vitality;          // Resistance To Death Curse

        [Header("Poise")]
        public float poise;

        public EquipmentModel[] equipmentModels;
    }

}
