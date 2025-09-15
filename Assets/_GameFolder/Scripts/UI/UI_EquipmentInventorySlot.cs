using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace XD
{
    public class UI_EquipmentInventorySlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightedIcon;
        [SerializeField] public Item currentItem;

        public void AddItem(Item item)
        {
            if(item == null) 
            {
                itemIcon.enabled = false;
                return; 
            }

            itemIcon.enabled = true;

            currentItem = item;
            itemIcon.sprite = item.itemIcon;
        }

        public void SelectSlot()
        {
            highlightedIcon.enabled = true;
        }
        public void DeselectSlot()
        {
            highlightedIcon.enabled = false;
        }

        public void EquipItem()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            Item  equippedItem;
            switch (PlayerUIManager.Instance.playerUIEquipmentManager.currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    equippedItem = player.playerInventoryManager.weaponsInRightHandSlots[0];
                    if(equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.weaponsInRightHandSlots[0] = currentItem as WeaponItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    // RE-Equip new weapon if we are holding the current weapon int his slot (if you change right weapon 3 and you are holding right weapon 1 nothing would happen here
                    if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                    {
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID; 
                    }
                    // Refresh Equipment Menu
                    PlayerUIManager.Instance.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.RightWeapon02:
                    equippedItem = player.playerInventoryManager.weaponsInRightHandSlots[1];
                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.weaponsInRightHandSlots[1] = currentItem as WeaponItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                    {
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;
                    }
                    PlayerUIManager.Instance.playerUIEquipmentManager.RefreshMenu();

                    break;
                case EquipmentType.RightWeapon03:
                    equippedItem = player.playerInventoryManager.weaponsInRightHandSlots[2];
                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.weaponsInRightHandSlots[2] = currentItem as WeaponItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                    {
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;
                    }
                    PlayerUIManager.Instance.playerUIEquipmentManager.RefreshMenu();

                    break;
                case EquipmentType.LeftWeapon01:
                    equippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[0];
                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.weaponsInLeftHandSlots[0] = currentItem as WeaponItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    if (player.playerInventoryManager.leftHandWeaponIndex == 0)
                    {
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;
                    }
                    PlayerUIManager.Instance.playerUIEquipmentManager.RefreshMenu();

                    break;
                case EquipmentType.LeftWeapon02:
                    equippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[1];
                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.weaponsInLeftHandSlots[1] = currentItem as WeaponItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    if (player.playerInventoryManager.leftHandWeaponIndex == 1)
                    {
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;
                    }
                    PlayerUIManager.Instance.playerUIEquipmentManager.RefreshMenu();

                    break;
                case EquipmentType.LeftWeapon03:
                    equippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[2];
                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.weaponsInLeftHandSlots[2] = currentItem as WeaponItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    if (player.playerInventoryManager.leftHandWeaponIndex == 2)
                    {
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;
                    }
                    PlayerUIManager.Instance.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.Head:
                    equippedItem = player.playerInventoryManager.headEquipment;
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.headEquipment = currentItem as HeadEquipmentItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    player.playerEquipmentManager.LoadHeadEquipment(player.playerInventoryManager.headEquipment);

                    PlayerUIManager.Instance.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.Body:
                    equippedItem = player.playerInventoryManager.bodyEquipment;
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.bodyEquipment = currentItem as BodyEquipmentItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    player.playerEquipmentManager.LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);
                    
                    PlayerUIManager.Instance.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.Legs:
                    equippedItem = player.playerInventoryManager.legEquipment;
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.legEquipment = currentItem as LegEquipmentItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    player.playerEquipmentManager.LoadLegEquipment(player.playerInventoryManager.legEquipment);
                    
                    PlayerUIManager.Instance.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.Hands:

                    equippedItem = player.playerInventoryManager.handEquipment;
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.handEquipment = currentItem as HandEquipmentItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    player.playerEquipmentManager.LoadHandEquipment(player.playerInventoryManager.handEquipment);

                    PlayerUIManager.Instance.playerUIEquipmentManager.RefreshMenu();
                    break;
                default:
                    break;
            }

            PlayerUIManager.Instance.playerUIEquipmentManager.SelectLastSelectedEquipmentSlot();
        }
    }

}
