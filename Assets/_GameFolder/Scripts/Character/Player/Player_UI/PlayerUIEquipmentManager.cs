using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace XD
{
    public class PlayerUIEquipmentManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] private GameObject menu;

        [Header("Weapon Slots")]
        private Button rightHandSlot01Button;
        private Button rightHandSlot02Button;
        private Button rightHandSlot03Button;
        private Button leftHandSlot01Button;
        private Button leftHandSlot02Button;
        private Button leftHandSlot03Button;
        private Button headEquipmentSlotButton;
        private Button bodyEquipmentSloButton;
        private Button legsEquipmentSlotButton;
        private Button handsEquipmentSlotButton;

        // Option 1
        [SerializeField] Image rightHandSlot01;
        [SerializeField] Image rightHandSlot02;
        [SerializeField] Image rightHandSlot03;
        [SerializeField] Image leftHandSlot01;
        [SerializeField] Image leftHandSlot02;
        [SerializeField] Image leftHandSlot03;
        [SerializeField] Image headEquipmentSlot;
        [SerializeField] Image bodyEquipmentSlot;
        [SerializeField] Image legsEquipmentSlot;
        [SerializeField] Image handsEquipmentSlot;

        // Option 2 SOLID
        //[SerializeField] List<Image> weaponSlotImages;

        [Header("Equipment Inventory")]
        public EquipmentType currentSelectedEquipmentSlot;
        [SerializeField] GameObject equipmentInventoryWindow;
        [SerializeField] GameObject equipmentInventorySlotPrefab;
        [SerializeField] Transform equipmentInventoryContentWindow; 
        [SerializeField] Item currentSelectedItem;

        private void Awake()
        {
            rightHandSlot01Button = rightHandSlot01.GetComponentInParent<Button>(true);
            rightHandSlot02Button = rightHandSlot02.GetComponentInParent<Button>(true);
            rightHandSlot03Button = rightHandSlot03.GetComponentInParent<Button>(true);
            leftHandSlot01Button = leftHandSlot01.GetComponentInParent<Button>(true);
            leftHandSlot02Button = leftHandSlot02.GetComponentInParent<Button>(true);
            leftHandSlot03Button = leftHandSlot03.GetComponentInParent<Button>(true);
            headEquipmentSlotButton = headEquipmentSlot.GetComponentInParent<Button>(true);
            bodyEquipmentSloButton = bodyEquipmentSlot.GetComponentInParent<Button>(true);
            legsEquipmentSlotButton = legsEquipmentSlot.GetComponentInParent<Button>(true);
            handsEquipmentSlotButton = handsEquipmentSlot.GetComponentInParent<Button>(true);
        }
        public void OpenEquipmentManagerMenu()
        {
            PlayerUIManager.Instance.menuWindowIsOpen = true;
            ToggleEquipmentButtons(true);
            menu.SetActive(true);
            equipmentInventoryWindow.SetActive(false);
            ClearEquipmentInventory();
            RefreshEquipmentSlotIcons(); 
        }
        public void CloseEquipmentManagerMenu()
        {
            PlayerUIManager.Instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

        public void RefreshMenu()
        {
            ClearEquipmentInventory();
            RefreshEquipmentSlotIcons();
        }

        private void ToggleEquipmentButtons(bool isEnabled)
        {
            rightHandSlot01Button.enabled = isEnabled;
            rightHandSlot02Button.enabled = isEnabled;
            rightHandSlot03Button.enabled = isEnabled;

            leftHandSlot01Button.enabled = isEnabled;
            leftHandSlot02Button.enabled = isEnabled;
            leftHandSlot03Button.enabled = isEnabled;

            headEquipmentSlotButton.enabled = isEnabled;
            bodyEquipmentSloButton.enabled = isEnabled;
            legsEquipmentSlotButton.enabled = isEnabled;
            handsEquipmentSlotButton.enabled = isEnabled;
        }
        // This func simply returns you to the last selected button when you are finished equipping a new item
        public void SelectLastSelectedEquipmentSlot()
        {
            Button lastSelectedButon = null;

            ToggleEquipmentButtons(true);
            switch (currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    lastSelectedButon = rightHandSlot01Button;
                    break;
                case EquipmentType.RightWeapon02:
                    lastSelectedButon = rightHandSlot02Button;
                    break;
                case EquipmentType.RightWeapon03:
                    lastSelectedButon = rightHandSlot03Button;
                    break;
                case EquipmentType.LeftWeapon01:
                    lastSelectedButon = leftHandSlot01Button;
                    break;
                case EquipmentType.LeftWeapon02:
                    lastSelectedButon = leftHandSlot02Button;
                    break;
                case EquipmentType.LeftWeapon03:
                    lastSelectedButon = leftHandSlot03Button;
                    break;
                case EquipmentType.Head:
                    lastSelectedButon = headEquipmentSlotButton;
                    break;
                case EquipmentType.Body:
                    lastSelectedButon = bodyEquipmentSloButton;
                    break;
                case EquipmentType.Legs:
                    lastSelectedButon = legsEquipmentSlotButton;
                    break;
                case EquipmentType.Hands:
                    lastSelectedButon = handsEquipmentSlotButton;
                    break;


                default:
                    break;
            }

            if(lastSelectedButon != null)
            {
                lastSelectedButon.Select();
                lastSelectedButon.OnSelect(null);
            }

            equipmentInventoryWindow.SetActive(false);
        }

        private void RefreshEquipmentSlotIcons()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // Right Hand Weapon 01
            WeaponItem rightHandWeapon01 = player.playerInventoryManager.weaponsInRightHandSlots[0];

            if (rightHandWeapon01.itemIcon != null)
            {
                rightHandSlot01.enabled = true;
                rightHandSlot01.sprite = rightHandWeapon01.itemIcon;
            }
            else
            {
                rightHandSlot01.enabled = false;
            }

            // Right Hand Weapon 02
            WeaponItem rightHandWeapon02 = player.playerInventoryManager.weaponsInRightHandSlots[1];
            if (rightHandWeapon02.itemIcon != null)
            {
                rightHandSlot02.enabled = true;
                rightHandSlot02.sprite = rightHandWeapon02.itemIcon;
            }
            else
            {
                rightHandSlot02.enabled = false;
            }
            // Right Hand Weapon 03
            WeaponItem rightHandWeapon03 = player.playerInventoryManager.weaponsInRightHandSlots[2];
            if (rightHandWeapon03.itemIcon != null)
            {
                rightHandSlot03.enabled = true;
                rightHandSlot03.sprite = rightHandWeapon03.itemIcon;
            }
            else
            {
                rightHandSlot03.enabled = false;
            }
            // Left Hand Weapon 01
            WeaponItem leftHandWeapon01 = player.playerInventoryManager.weaponsInLeftHandSlots[0];
            if (leftHandWeapon01.itemIcon != null)
            {
                leftHandSlot01.enabled = true;
                leftHandSlot01.sprite = leftHandWeapon01.itemIcon;
            }
            else
            {
                leftHandSlot01.enabled = false;
            }
            // Left Hand Weapon 02
            WeaponItem leftHandWeapon02 = player.playerInventoryManager.weaponsInLeftHandSlots[1];
            if (leftHandWeapon02.itemIcon != null)
            {
                leftHandSlot02.enabled = true;
                leftHandSlot02.sprite = leftHandWeapon02.itemIcon;
            }
            else
            {
                leftHandSlot02.enabled = false;
            }
            // Left Hand Weapon 03
            WeaponItem leftHandWeapon03 = player.playerInventoryManager.weaponsInLeftHandSlots[2];
            if (leftHandWeapon03.itemIcon != null)
            {
                leftHandSlot03.enabled = true;
                leftHandSlot03.sprite = leftHandWeapon03.itemIcon;
            }
            else
            {
                leftHandSlot03.enabled = false;
            }

            // Head Equipment  
            HeadEquipmentItem headEquipment = player.playerInventoryManager.headEquipment;

            if (headEquipment != null)
            {
                headEquipmentSlot.enabled = true;
                headEquipmentSlot.sprite = headEquipment.itemIcon;
            }
            else
            {
                headEquipmentSlot.enabled = false;
            }

            // Body Equipment
            BodyEquipmentItem bodyEquipment = player.playerInventoryManager.bodyEquipment;
            if (bodyEquipment != null)
            {
                bodyEquipmentSlot.enabled = true;
                bodyEquipmentSlot.sprite = bodyEquipment.itemIcon;
            }
            else
            {
                bodyEquipmentSlot.enabled = false;
            }

            // Legs Equipment
            LegEquipmentItem legsEquipment = player.playerInventoryManager.legEquipment;
            if (legsEquipment != null)
            {
                legsEquipmentSlot.enabled = true;
                legsEquipmentSlot.sprite = legsEquipment.itemIcon;
            }
            else
            {
                legsEquipmentSlot.enabled = false;
            }

            // Hands Equipment
            HandEquipmentItem handEquipment = player.playerInventoryManager.handEquipment;
            if (handEquipment != null)
            {
                handsEquipmentSlot.enabled = true;
                handsEquipmentSlot.sprite = handEquipment.itemIcon;
            }
            else
            {
                handsEquipmentSlot.enabled = false;

            }
        }

        private void ClearEquipmentInventory()
        {
            foreach(Transform item in equipmentInventoryContentWindow)
            {
                Destroy(item.gameObject);
            }
        }
        
        // Call. OnClick, PLAYER UI MANAGER Inspector Equipment Slots Window
        public void LoadEquipmentInventory()
        {
            ToggleEquipmentButtons(false);
            equipmentInventoryWindow.SetActive(true);

            switch(currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.RightWeapon02:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.RightWeapon03:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon01:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon02:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon03:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.Head:
                    LoadHeadEquipmentInventory();
                    break;
                case EquipmentType.Body:
                    LoadBodyEquipmentInventory();
                    break;
                case EquipmentType.Legs:
                    LoadLegsEquipmentInventory();
                    break;
                case EquipmentType.Hands:
                    LoadHandsEquipmentInventory();
                    break;
                default:
                    break;
            }
        }

        private void LoadWeaponInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            
            List<WeaponItem> weaponsInInventory = new List<WeaponItem>();

            // Search Our Entire Inventory, And Out Of All Items In Our Inventory if the item is a weapon add it  to our weapons list
            for(int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                WeaponItem weapon = player.playerInventoryManager.itemsInInventory[i] as WeaponItem;
                if(weapon != null)
                {
                    weaponsInInventory.Add(weapon);
                }
            }

            for(int i = 0; i < weaponsInInventory.Count; i++)
            {
                // TODO: Send a player message that he none of item type in inventory
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();

                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < weaponsInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(weaponsInInventory[i]);


                // Select the firs button in the list
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);

                }
            }

        }

        private void LoadHeadEquipmentInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<HeadEquipmentItem> headEquipmentInInventory = new List<HeadEquipmentItem>();

            // Search Our Entire Inventory, And Out Of All Items In Our Inventory if the item is a weapon add it  to our weapons list
            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                HeadEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as HeadEquipmentItem;
                if (equipment != null)
                {
                    headEquipmentInInventory.Add(equipment);
                }
            }

            for (int i = 0; i < headEquipmentInInventory.Count; i++)
            {
                // TODO: Send a player message that he none of item type in inventory
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < headEquipmentInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(headEquipmentInInventory[i]);


                // Select the firs button in the list
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);

                }
            }
        }

        private void LoadBodyEquipmentInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<BodyEquipmentItem> bodyEquipmentInInventory = new List<BodyEquipmentItem>();

            // Search Our Entire Inventory, And Out Of All Items In Our Inventory if the item is a weapon add it  to our weapons list
            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                BodyEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as BodyEquipmentItem;
                if (equipment != null)
                {
                    bodyEquipmentInInventory.Add(equipment);
                }
            }

            for (int i = 0; i < bodyEquipmentInInventory.Count; i++)
            {
                // TODO: Send a player message that he none of item type in inventory
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < bodyEquipmentInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(bodyEquipmentInInventory[i]);


                // Select the firs button in the list
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);

                }
            }
        }

        private void LoadLegsEquipmentInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<LegEquipmentItem> legsEquipmentInInventory = new List<LegEquipmentItem>();

            // Search Our Entire Inventory, And Out Of All Items In Our Inventory if the item is a weapon add it  to our weapons list
            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                LegEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as LegEquipmentItem;
                if (equipment != null)
                {
                    legsEquipmentInInventory.Add(equipment);
                }
            }

            for (int i = 0; i < legsEquipmentInInventory.Count; i++)
            {
                // TODO: Send a player message that he none of item type in inventory
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < legsEquipmentInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(legsEquipmentInInventory[i]);


                // Select the firs button in the list
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);

                }

            }
        }

        private void LoadHandsEquipmentInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            List<HandEquipmentItem> handsEquipmentInInventory = new List<HandEquipmentItem>();
            // Search Our Entire Inventory, And Out Of All Items In Our Inventory if the item is a weapon add it  to our weapons list
            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                HandEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as HandEquipmentItem;
                if (equipment != null)
                {
                    handsEquipmentInInventory.Add(equipment);
                }
            }
            for (int i = 0; i < handsEquipmentInInventory.Count; i++)
            {
                // TODO: Send a player message that he none of item type in inventory
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }
            bool hasSelectedFirstInventorySlot = false;
            for (int i = 0; i < handsEquipmentInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(handsEquipmentInInventory[i]);

                // Select the firs button in the list
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);

                }
            }
        }

        // Call. EventTrigger(Select) PLAYER UI MANAGER Inspector Equipment Slots Window
        public void SelectEquipmentSlot(int equipmentSlot)
        {
            currentSelectedEquipmentSlot = (EquipmentType)equipmentSlot;
        }

        public void UnEquipSelectedItem()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            Item unequippedItem;
            switch (currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    unequippedItem = player.playerInventoryManager.weaponsInRightHandSlots[0];
                    
                    if(unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInRightHandSlots[0] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);
                        if(unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                        }
                    }
                    
                    if(player.playerInventoryManager.rightHandWeaponIndex == 0)
                    {   
                        player.playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    }
                    break;
                case EquipmentType.RightWeapon02:
                    unequippedItem = player.playerInventoryManager.weaponsInRightHandSlots[1];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInRightHandSlots[1] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);
                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                        }
                    }
                    if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                    {
                        player.playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    }
                    break;
                case EquipmentType.RightWeapon03:
                    unequippedItem = player.playerInventoryManager.weaponsInRightHandSlots[2];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInRightHandSlots[2] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);
                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                        }
                    }
                    if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                    {
                        player.playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    }

                    break;
                case EquipmentType.LeftWeapon01:
                    unequippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[0];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInLeftHandSlots[0] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);
                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                        }
                    }
                    if (player.playerInventoryManager.leftHandWeaponIndex == 0)
                    {
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    }
                    break;
                case EquipmentType.LeftWeapon02:
                    unequippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[1];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInLeftHandSlots[1] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);
                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                        }
                    }
                    if (player.playerInventoryManager.leftHandWeaponIndex == 1)
                    {
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    }
                    break;
                case EquipmentType.LeftWeapon03:
                    unequippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[2];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInLeftHandSlots[2] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);
                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                        }
                    }
                    if (player.playerInventoryManager.leftHandWeaponIndex == 2)
                    {
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    }
                    break;
                case EquipmentType.Head:
                    unequippedItem = player.playerInventoryManager.headEquipment;
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                    player.playerInventoryManager.headEquipment = null;
                    player.playerEquipmentManager.LoadHeadEquipment(player.playerInventoryManager.headEquipment);
                    break;

                case EquipmentType.Body:
                    unequippedItem = player.playerInventoryManager.bodyEquipment;
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                    player.playerInventoryManager.bodyEquipment = null;
                    player.playerEquipmentManager.LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);
                    break;

                case EquipmentType.Legs:
                    unequippedItem = player.playerInventoryManager.legEquipment;
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                    player.playerInventoryManager.legEquipment = null;
                    player.playerEquipmentManager.LoadLegEquipment(player.playerInventoryManager.legEquipment);
                    break;
                case EquipmentType.Hands:
                    unequippedItem = player.playerInventoryManager.handEquipment;
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                    player.playerInventoryManager.handEquipment = null;
                    player.playerEquipmentManager.LoadHandEquipment(player.playerInventoryManager.handEquipment);
                    break;
                default:
                    break;

            }

            RefreshMenu();
        }
    }

}
