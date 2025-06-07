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
        private RectTransform rectTransform;

        [SerializeField] protected bool scaleBarLengthWithStats = true;
        [SerializeField] protected float widthScaleMultiplier = 1;
        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;

            if(scaleBarLengthWithStats)
            {
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);

                // Resets the position of the bars based on their layut group's settings
                PlayerUIManager.Instance.playerUIHUDManager.RefreshHUD();
            }
        }
    }

}

