using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "Items/Weapons/Melee Weapon")]
    public class MeleeWeaponItem : WeaponItem
    {
        // Weapon "deflection" (if the weapon will bounce off another weapon when it is being guarded against)
        // Can be buffed

        [Header("Attack Modifiers")]
        public float riposte_Attack_01_Modifier = 3.3f;
        public float backstab_Attack_01_Modifier = 3.3f;
    }

}
