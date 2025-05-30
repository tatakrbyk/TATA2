using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace XD
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
    }

}

