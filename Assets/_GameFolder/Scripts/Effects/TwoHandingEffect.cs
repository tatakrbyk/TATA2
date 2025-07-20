using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(fileName = "TwoHandingEffect", menuName = "Character Effects/Static Effects/Two Handing Effect")]
    public class TwoHandingEffect : StaticCharacterEffect
    {
        [SerializeField] int strengthGainedFromTwoHandingWeapon;

        public override void ProcessStaticEffect(CharacterManager character)
        {
            base.ProcessStaticEffect(character);

            if (character.IsOwner)
            {
                strengthGainedFromTwoHandingWeapon = Mathf.RoundToInt(character.characterNetworkManager.strength.Value / 2);
                Debug.Log("Two Handing Strength Effect Applied: " + strengthGainedFromTwoHandingWeapon);
                character.characterNetworkManager.strength.Value += strengthGainedFromTwoHandingWeapon;
            }
        }

        public override void RemoveStaticEffect(CharacterManager character)
        {
            base.RemoveStaticEffect(character);
            if (character.IsOwner)
            {
                Debug.Log("Two Handing Strength Effect Removed: " + strengthGainedFromTwoHandingWeapon);
                character.characterNetworkManager.strength.Value -= strengthGainedFromTwoHandingWeapon;
            }
        }
    }

}
