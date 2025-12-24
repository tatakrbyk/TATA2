using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace XD
{
    public class PlayerUIEquipmentManager : PlayerUIMenu
    {
        [Header("Weapon Slots Button")]
        private Button rightHandSlot01Button;
        private Button rightHandSlot02Button;
        private Button rightHandSlot03Button;
        private Button leftHandSlot01Button;
        private Button leftHandSlot02Button;
        private Button leftHandSlot03Button;

        [Header("Armor Slots Button")]
        private Button headEquipmentSlotButton;
        private Button bodyEquipmentSloButton;
        private Button legsEquipmentSlotButton;
        private Button handsEquipmentSlotButton;
        [Header("Projectile Slots Button")]
        private Button mainProjectileSlotButton;
        private Button secondaryProjectileSlotButton;

        [Header("Quick Slots Button")]
        private Button quickSlot01Button;
        private Button quickSlot02Button;
        private Button quickSlot03Button;

        // Option 1
        [Header("Weapon Slot Images")]
        [SerializeField] Image rightHandSlot01;
        [SerializeField] Image rightHandSlot02;
        [SerializeField] Image rightHandSlot03;
        [SerializeField] Image leftHandSlot01;
        [SerializeField] Image leftHandSlot02;
        [SerializeField] Image leftHandSlot03;
        [Header("Armor Slot Images")]
        [SerializeField] Image headEquipmentSlot;
        [SerializeField] Image bodyEquipmentSlot;
        [SerializeField] Image legsEquipmentSlot;
        [SerializeField] Image handsEquipmentSlot;

        [Header("Projectile Slot Images")]
        [SerializeField] Image mainProjectileSlot;
        [SerializeField] Image secondaryProjectileSlot;
        [Header("Quick Slot Images")]
        [SerializeField] Image quickSlot01EquipmentSlot;
        [SerializeField] Image quickSlot02EquipmentSlot;
        [SerializeField] Image quickSlot03EquipmentSlot;
        [SerializeField] TextMeshProUGUI quickSlo01tCountText;
        [SerializeField] TextMeshProUGUI quickSlo02tCountText;
        [SerializeField] TextMeshProUGUI quickSlo03tCountText;


        [Header("Projectile Counts Text")]
        [SerializeField] TextMeshProUGUI mainProjectileContText;
        [SerializeField] TextMeshProUGUI secondaryProjectileContText;

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
            mainProjectileSlotButton = mainProjectileSlot.GetComponentInParent<Button>(true);
            secondaryProjectileSlotButton = secondaryProjectileSlot.GetComponentInParent<Button>(true);
            quickSlot01Button = quickSlot01EquipmentSlot.GetComponentInParent<Button>(true);
            quickSlot02Button = quickSlot02EquipmentSlot.GetComponentInParent<Button>(true);
            quickSlot03Button = quickSlot03EquipmentSlot.GetComponentInParent<Button>(true); 
        }

        public override void OpenMenu()
        {
            base.OpenMenu();
            ToggleEquipmentButtons(true);
            equipmentInventoryWindow.SetActive(false);
            ClearEquipmentInventory();
            RefreshEquipmentSlotIcons();

            // NOTE: Close work PlayerUIMenu
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
            mainProjectileSlotButton.enabled = isEnabled;
            secondaryProjectileSlotButton.enabled = isEnabled;

            quickSlot01Button.enabled = isEnabled;
            quickSlot02Button.enabled = isEnabled;
            quickSlot03Button.enabled = isEnabled;

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
                case EquipmentType.MainProjectile:
                    lastSelectedButon = mainProjectileSlotButton;
                    break;
                case EquipmentType.SecondaryProjectile:
                    lastSelectedButon = secondaryProjectileSlotButton;
                    break;
                case EquipmentType.QuickSlot01:
                    lastSelectedButon = quickSlot01Button;
                    break;
                case EquipmentType.QuickSlot02:
                    lastSelectedButon = quickSlot02Button;
                    break;
                case EquipmentType.QuickSlot03:
                    lastSelectedButon = quickSlot03Button;
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

            // Projectile Equipment
            RangedProjectileItem mainProjectile = player.playerInventoryManager.mainProjectile;
            if(mainProjectile != null)
            {
                mainProjectileSlot.enabled = true;
                mainProjectileSlot.sprite = mainProjectile.itemIcon;
                mainProjectileContText.enabled = true;
                mainProjectileContText.text = mainProjectile.currentAmmoAmount.ToString();
            }
            else
            {
                mainProjectileSlot.enabled = false;
                mainProjectileContText.enabled = false;
            }

            // Secondary Projectile Equipment
            RangedProjectileItem secondaryProjectile = player.playerInventoryManager.secondaryProjectile;
            if (secondaryProjectile != null)
            {
                secondaryProjectileSlot.enabled = true;
                secondaryProjectileSlot.sprite = secondaryProjectile.itemIcon;
                secondaryProjectileContText.enabled = true;
                secondaryProjectileContText.text = secondaryProjectile.currentAmmoAmount.ToString();
            }
            else
            {
                secondaryProjectileSlot.enabled = false;
                secondaryProjectileContText.enabled = false;
            }

            // Quick slots
            QuickSlotItem quickSlotEquipment01 = player.playerInventoryManager.quickSlotItemsInQuickSlots[0];
            if (quickSlotEquipment01 != null)
            {
                quickSlot01EquipmentSlot.enabled = true;
                quickSlot01EquipmentSlot.sprite = quickSlotEquipment01.itemIcon;

                if(quickSlotEquipment01.isConsumable)
                {
                    quickSlo01tCountText.enabled = true;
                    quickSlo01tCountText.text = quickSlotEquipment01.GetCurrentAmount(player).ToString();
                }
                else
                {
                    quickSlo01tCountText.enabled = false;
                }
            }
            else
            {
                quickSlot01EquipmentSlot.enabled = false;
                quickSlo01tCountText.enabled = false;
            }

            QuickSlotItem quickSlotEquipment02 = player.playerInventoryManager.quickSlotItemsInQuickSlots[1];
            if (quickSlotEquipment02 != null)
            {
                quickSlot02EquipmentSlot.enabled = true;
                quickSlot02EquipmentSlot.sprite = quickSlotEquipment02.itemIcon;
                if (quickSlotEquipment02.isConsumable)
                {
                    quickSlo02tCountText.enabled = true;
                    quickSlo02tCountText.text = quickSlotEquipment02.GetCurrentAmount(player).ToString();
                }
                else
                {
                    quickSlo02tCountText.enabled = false;
                }
            }
            else
            {
                quickSlot02EquipmentSlot.enabled = false;
                quickSlo02tCountText.enabled = false;
            }

            QuickSlotItem quickSlotEquipment03 = player.playerInventoryManager.quickSlotItemsInQuickSlots[2];
            if (quickSlotEquipment03 != null)
            {
                quickSlot03EquipmentSlot.enabled = true;
                quickSlot03EquipmentSlot.sprite = quickSlotEquipment03.itemIcon;
                if (quickSlotEquipment03.isConsumable)
                {
                    quickSlo03tCountText.enabled = true;
                    quickSlo03tCountText.text = quickSlotEquipment03.GetCurrentAmount(player).ToString();
                }
                else
                {
                    quickSlo03tCountText.enabled = false;
                }
            }
            else
            {
                quickSlot03EquipmentSlot.enabled = false;
                quickSlo03tCountText.enabled = false;
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
                case EquipmentType.MainProjectile:
                    LoadProjectileInventory();
                    break;
                case EquipmentType.SecondaryProjectile:
                    LoadProjectileInventory();
                    break;
                 case EquipmentType.QuickSlot01:
                    LoadQuickSlotInventory();
                    break;
                case EquipmentType.QuickSlot02:
                    LoadQuickSlotInventory();
                    break;
                case EquipmentType.QuickSlot03:
                    LoadQuickSlotInventory();
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

        private void LoadProjectileInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<RangedProjectileItem> projectilesInInventory = new List<RangedProjectileItem>();

            // Search Our Entire Inventory, And Out Of All Items In Our Inventory if the item is a weapon add it  to our weapons list
            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                RangedProjectileItem projectile = player.playerInventoryManager.itemsInInventory[i] as RangedProjectileItem;
                if (projectile != null)
                {
                    projectilesInInventory.Add(projectile);
                }
            }

            for (int i = 0; i < projectilesInInventory.Count; i++)
            {
                // TODO: Send a player message that he none of item type in inventory
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();

                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < projectilesInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(projectilesInInventory[i]);


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

        private void LoadQuickSlotInventory()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            List<QuickSlotItem> quickSlotItemsInInventory = new List<QuickSlotItem>();

            // Search Our Entire Inventory, And Out Of All Items In Our Inventory if the item is a weapon add it  to our weapons list
            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                QuickSlotItem quickSlotItem = player.playerInventoryManager.itemsInInventory[i] as QuickSlotItem;
                if (quickSlotItem != null)
                {
                    quickSlotItemsInInventory.Add(quickSlotItem);
                }
            }

            for (int i = 0; i < quickSlotItemsInInventory.Count; i++)
            {
                // TODO: Send a player message that he none of item type in inventory
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();

                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < quickSlotItemsInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(quickSlotItemsInInventory[i]);


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
                case EquipmentType.MainProjectile:
                    unequippedItem = player.playerInventoryManager.mainProjectile;
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                    player.playerInventoryManager.mainProjectile = null;
                    player.playerEquipmentManager.LoadMainProjectileEquipment(player.playerInventoryManager.mainProjectile);
                    break;
                case EquipmentType.SecondaryProjectile:
                    unequippedItem = player.playerInventoryManager.secondaryProjectile;
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                    player.playerInventoryManager.secondaryProjectile = null;
                    player.playerEquipmentManager.LoadSecondaryProjectileEquipment(player.playerInventoryManager.secondaryProjectile);
                    break;
                case EquipmentType.QuickSlot01:
                    unequippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[0];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[0] = null;
                    if (player.playerInventoryManager.quickSlotItemIndex == 0)
                    {
                        player.playerNetworkManager.currentQuickSlotItemID.Value = -1;
                    }
                    break;
                case EquipmentType.QuickSlot02:
                    unequippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[1];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[1] = null;
                    if (player.playerInventoryManager.quickSlotItemIndex == 1)
                    {
                        player.playerNetworkManager.currentQuickSlotItemID.Value = -1;
                    }
                    break;
                case EquipmentType.QuickSlot03:
                    unequippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[2];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[2] = null;
                    if(player.playerInventoryManager.quickSlotItemIndex == 2)
                    {
                        player.playerNetworkManager.currentQuickSlotItemID.Value = -1;
                    }
                    break;
                default:
                    break;
            }

            RefreshMenu();
        }
    }

}
