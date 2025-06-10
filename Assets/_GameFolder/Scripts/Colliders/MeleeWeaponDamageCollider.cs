using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD

{
    public class MeleeWeaponDamageCollider : DamageCollider
    {

        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage; // (When calculating damage this is the used check for attackers damage modifiers, effect etc)
    }
}
