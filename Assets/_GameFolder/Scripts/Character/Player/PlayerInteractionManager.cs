using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        private PlayerManager player;

        private List<Interactable> currentInteractableActions;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }
        private void Start()
        {
            currentInteractableActions = new List<Interactable>();
        }

        private void FixedUpdate()
        {
            if(!player.IsOwner) { return; }

            if(!PlayerUIManager.Instance.menuWindowIsOpen && !PlayerUIManager.Instance.popUpWindowIsOpen)
            {
                CheckForInteractable();
            }
        }

        private void CheckForInteractable()
        {
            if (currentInteractableActions.Count == 0) { return; }

            if (currentInteractableActions[0] == null)
            {
                currentInteractableActions.RemoveAt(0); // If the current interactable item at position 0 becomes null (removed from game), we remove position 0 from the list
                return;
            }

            if (currentInteractableActions[0] != null)
            {
                PlayerUIManager.Instance.playerUIPopUpManager.SendPlayerMessagePopUp(currentInteractableActions[0].interactableText);
            }
        }

        private void RefreshInteractionList()
        {
            for(int i = currentInteractableActions.Count - 1; i > -1; i--)
            {
                if (currentInteractableActions[i] == null)
                {
                    currentInteractableActions.RemoveAt(i);
                }
            }
        }

        public void AddInteractionToList(Interactable interactableObject)
        {
            RefreshInteractionList();

            if(!currentInteractableActions.Contains(interactableObject))
            {
                currentInteractableActions.Add(interactableObject);
            }
        }
        public void RemoveInterationFromList(Interactable interactableObject)
        {
            if (currentInteractableActions.Contains(interactableObject))
            {
                currentInteractableActions.Remove(interactableObject);
            }

            RefreshInteractionList();
        }
        public void Interact()
        {
            if(currentInteractableActions.Count == 0) { return; }
            if (currentInteractableActions[0] != null)
            {
                currentInteractableActions[0].Interact(player);
                RefreshInteractionList();
            }
        }
    }

}
