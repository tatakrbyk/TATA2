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

        [Header("Creature Loot Pick Up")]
        public NetworkVariable<int> ItemID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("World Spawn Pick Up")]
        [SerializeField] private int WorldSpawnInteractableID;  // This Is A Unique ID Given To Each World Spawn Item, So You May Not Loot The More Than Once
        [SerializeField] private bool hasBeenLooted = false;

        [Header("SFX")]
        [SerializeField] AudioClip itemDropSFX;
        private AudioSource audioSource;

        protected override void Awake()
        {
            base.Awake();

            audioSource = GetComponent<AudioSource>();
        }
        protected override void Start()
        {
            base.Start();
            if(pickUpType == ItemPickUpType.WorldSpawn)
            {
                CheckIfWorldItemWasAlreadyLooted();
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            ItemID.OnValueChanged += OnItemIDChanged;
            networkPosition.OnValueChanged += OnNetworkPositionChanged;

            if(pickUpType == ItemPickUpType.CharacterDrop)
            {                
                if (audioSource != null && itemDropSFX != null)
                {
                    audioSource.PlayOneShot(itemDropSFX);
                }
            }

            if(!IsOwner)
            {
                OnItemIDChanged(0, ItemID.Value);
                OnNetworkPositionChanged(Vector3.zero, networkPosition.Value);
            }
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            ItemID.OnValueChanged -= OnItemIDChanged;
            networkPosition.OnValueChanged -= OnNetworkPositionChanged;
        }
        private void CheckIfWorldItemWasAlreadyLooted()
        {
            if(!NetworkManager.Singleton.IsHost)
            {
                gameObject.SetActive(false);
                return;
            }

            // Compare the data of looted items ID with this Item's ID
            if(!WorldSaveGameManager.Instance.currentCharacterData.worldItemsLooted.ContainsKey(WorldSpawnInteractableID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.worldItemsLooted.Add(WorldSpawnInteractableID, false);
            }

            hasBeenLooted = WorldSaveGameManager.Instance.currentCharacterData.worldItemsLooted[WorldSpawnInteractableID]; 

            if(hasBeenLooted )
            {
                gameObject.SetActive(false );
            }
        }

        public override void Interact(PlayerManager player)
        {
            if(player.isPerformingAction) { return; }

            base.Interact(player);

            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Instance.pickUpItemSFX);

            // Play Pick Up Animation
            player.playerAnimatorManager.PlayActionAnimation("Pick_Up_Item_01", true); 
            // Add Item To Inventory
            player.playerInventoryManager.AddItemToInventory(item);

            // Display Pop UP (like tooltip)
            PlayerUIManager.Instance.playerUIPopUpManager.SendItemPopUp(item, 1);

            // Save loot status if it'S a world spawn
            if(pickUpType == ItemPickUpType.WorldSpawn)
            {
                if(WorldSaveGameManager.Instance.currentCharacterData.worldItemsLooted.ContainsKey((int)WorldSpawnInteractableID))
                {
                    WorldSaveGameManager.Instance.currentCharacterData.worldItemsLooted.Remove(WorldSpawnInteractableID);
                }

                WorldSaveGameManager.Instance.currentCharacterData.worldItemsLooted.Add((int)WorldSpawnInteractableID, true);
            }

            DestroyThisNetworkObjectServerRpc();
        }

        protected void OnItemIDChanged(int oldValue, int newValue)
        {
            if(pickUpType != ItemPickUpType.CharacterDrop) { return; }
            
            item = WorldItemDatabase.Instance.GetItemByID(ItemID.Value);
            
        }

        protected void OnNetworkPositionChanged(Vector3 oldPosition, Vector3 newPosition)
        {
            if (pickUpType != ItemPickUpType.CharacterDrop) { return; }

            transform.position = networkPosition.Value;
        }

        #region Network
        [ServerRpc(RequireOwnership = false)]
        protected void DestroyThisNetworkObjectServerRpc()
        {
            if (IsServer)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
        #endregion
    }

}
