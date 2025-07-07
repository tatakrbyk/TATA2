using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace XD
{
    public class CharacterUIManager : MonoBehaviour
    {
        [Header("UI")]
        public bool hasFloatingHPBar = true;
        public UI_Character_HP_Bar characterHPBar;

        public void OnHPChanged(int oldValue, int newValue)
        {
            characterHPBar.oldHealthValue = oldValue;
            characterHPBar.SetStat(newValue);
        }

    }

}
