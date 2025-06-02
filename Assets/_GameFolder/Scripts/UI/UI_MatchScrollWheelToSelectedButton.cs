using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XD
{
    public class UI_MatchScrollWheelToSelectedButton : MonoBehaviour
    {
        [SerializeField] GameObject currentSelected;
        [SerializeField] GameObject previouslySelected;
        [SerializeField] RectTransform currentSelectedTransform;

        [SerializeField] RectTransform contentPanel;
        [SerializeField] ScrollRect scrollRect;
        void Update()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if(currentSelected != null)
            {
                if (currentSelected != previouslySelected)
                {
                    previouslySelected = currentSelected;
                    currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                    SnapTo(currentSelectedTransform);
                }

            }
        }

        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            Vector2 newPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) 
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

            // Only want  to lock the position on the y axis ( Up and Down)
            newPosition.x = 0;


            contentPanel.anchoredPosition = newPosition;
        }
    }

}
