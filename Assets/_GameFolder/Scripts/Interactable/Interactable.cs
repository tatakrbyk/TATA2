using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace XD
{
    public class Interactable : NetworkBehaviour
    {
        public string interactableText; // Text prompt when entering the interactable collider ( PickUp Item, Pull Lever Ect)
        [SerializeField] protected Collider interactableCollider;
        [SerializeField] protected bool hostOnlyInteractable = true; // When enabled, Object cannot be interacted with by co-op players

        protected virtual void Awake()
        {
            // Check if its null, In Some cases you may want to manually assign the collider as a child object in the inspector
            if (interactableCollider == null)
            {
                interactableCollider = GetComponent<Collider>();
            }
        }

        protected virtual void Start()
        {

        }

        public virtual void Interact(PlayerManager player)
        {
            Debug.Log("Interactable Interact called on " + gameObject.name + " by " + player.gameObject.name);

            if (!player.IsOwner) { return; }
            
            interactableCollider.enabled = false; // Disable the collider so it cannot be interacted with again
            player.playerInteractionManager.RemoveInterationFromList(this);
            PlayerUIManager.Instance.playerUIPopUpManager.CloseAllPopUpWindows();
        }
        public virtual void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                if(!player.playerNetworkManager.IsHost && hostOnlyInteractable)
                {
                    return; // If this is a host-only interactable, do not allow co-op players to interact
                }
                if(!player.IsOwner) { return; }

                player.playerInteractionManager.AddInteractionToList(this);

            }
        }

        public virtual void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                if (!player.playerNetworkManager.IsHost && hostOnlyInteractable)
                {
                    return; // If this is a host-only interactable, do not allow co-op players to interact
                }
                if (!player.IsOwner) { return; }
                player.playerInteractionManager.RemoveInterationFromList(this);

                PlayerUIManager.Instance.playerUIPopUpManager.CloseAllPopUpWindows();
            }
        }
    }

}
