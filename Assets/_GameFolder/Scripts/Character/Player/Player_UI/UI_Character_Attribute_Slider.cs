using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class UI_Character_Attribute_Slider : MonoBehaviour
    {
        [SerializeField] private CharacterAttribute sliderAttribute;

        public void SetCurrentSelectedAttribute()
        {
            PlayerUIManager.Instance.playerUILevelUpManager.currentSelectedAttribute = sliderAttribute;
        }
    }
}
