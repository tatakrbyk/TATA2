using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace XD
{
    public class AICharacterInventoryManager : CharacterInventoryManager
    {
        AICharacterManager a›character;

        [Header("Loot Chance")]
        public int dropItemChance = 10;
        [SerializeField] private Item[] droppableItems;
        private Vector3 offset = new Vector3(0, 0.45f, 0);
        protected override void Awake()
        {
            base.Awake();
            a›character = GetComponent<AICharacterManager>();
        }

        public void DropItem()
        {
            if (!a›character.IsOwner) { return; }

            bool willDropItem = false;
            int itemChanceRoll = Random.Range(0, 100);

            if (itemChanceRoll <= dropItemChance)
            {
                willDropItem = true;
            }

            if (!willDropItem) { return; }

            Item generatedItem = droppableItems[Random.Range(0, droppableItems.Length)];
            if (generatedItem == null) { return; }

            GameObject itemPickUpInteractableGameObject = Instantiate(WorldItemDatabase.Instance.pickUpItemPrefab);
            PickUpItemInteractable pickUpItemInteractable = itemPickUpInteractableGameObject.GetComponent<PickUpItemInteractable>();
            itemPickUpInteractableGameObject.GetComponent<NetworkObject>().Spawn();
            pickUpItemInteractable.ItemID.Value = generatedItem.itemID;
            pickUpItemInteractable.networkPosition.Value = transform.position + offset;

        }
    }
}
