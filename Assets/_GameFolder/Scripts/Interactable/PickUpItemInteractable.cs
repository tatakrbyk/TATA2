using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace XD
{
    public class PickUpItemInteractable : Interactable
    {
        public ItemPickUpType pickUpType;

        [Header("Item")]
        [SerializeField] Item item;

        [Header("World Spawn Pick Up")]
        [SerializeField] private int itemID;
        [SerializeField] private bool hasBeenLooted = false;

        protected override void Start()
        {
            base.Start();
            if(pickUpType == ItemPickUpType.WorldSpawn)
            {
                CheckIfWorldItemWasAlreadyLooted();
            }
        }

        private void CheckIfWorldItemWasAlreadyLooted()
        {
            if(!NetworkManager.Singleton.IsHost)
            {
                gameObject.SetActive(false);
                return;
            }

            // Compare the data of looted items ID with this Item's ID
            if(!WorldSaveGameManager.Instance.currentCharacterData.worldItemsLooted.ContainsKey(itemID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.worldItemsLooted.Add(itemID, false);
            }

            hasBeenLooted = WorldSaveGameManager.Instance.currentCharacterData.worldItemsLooted[itemID]; 

            if(hasBeenLooted )
            {
                gameObject.SetActive(false );
            }
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Instance.pickUpItemSFX);
            
            // Add Item To Inventory
            player.playerInventoryManager.AddItemToInventory(item);

            // Display Pop UP (like tooltip)
            PlayerUIManager.Instance.playerUIPopUpManager.SendItemPopUp(item, 1);

            // Save loot status if it'S a world spawn
            if(pickUpType == ItemPickUpType.WorldSpawn)
            {
                if(WorldSaveGameManager.Instance.currentCharacterData.worldItemsLooted.ContainsKey((int)itemID))
                {
                    WorldSaveGameManager.Instance.currentCharacterData.worldItemsLooted.Remove(itemID);
                }

                WorldSaveGameManager.Instance.currentCharacterData.worldItemsLooted.Add((int)itemID, true);
            }

            Destroy(gameObject);
        }
    }

}
