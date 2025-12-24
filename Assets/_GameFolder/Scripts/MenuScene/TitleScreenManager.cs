using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.InputManagerEntry;

namespace XD
{
    public class TitleScreenManager : MonoBehaviour
    {
        private static TitleScreenManager instance; public static TitleScreenManager Instance { get { return instance; } }

        // Main Menu 
        [Header("MeMain Menu Menus")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;
        [SerializeField] GameObject titleScreenCreationMenu;

        [Header("Main Menu Buttons")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [SerializeField] Button mainMenuNewGameButton;

        [Header("Main Menu Pop Up")]
        [SerializeField] GameObject noCharacterSlotsPopup;
        [SerializeField] Button noCharacterSlotsClose;
        [SerializeField] GameObject deleteCharacterSlotPopup;
        [SerializeField] Button deleteCharacterPopupConfirmButton;
        [SerializeField] Button deleteCharacterPopupCancelButton;

        // Character Creation Menu

        [Header("Character Creation Main Panel Buttons")]
        [SerializeField] Button characterNameButton;
        [SerializeField] Button characterClassButton;
        [SerializeField] Button characterHairButton;
        [SerializeField] Button characterHairColorButton;
        [SerializeField] Button characterSexButton;
        [SerializeField] Button startGameButton;

        [SerializeField] TextMeshProUGUI characterSexText;

        [Header("Character Creation Class Panel Buttons")]
        [SerializeField] Button[] characterClassButtons;
        [SerializeField] Button[] characterHairButtons;
        [SerializeField] Button[] characterHairColorButtons;

        [Header("Character Creation Secondary Panel Menus")]
        [SerializeField] GameObject characterNameMenu;
        [SerializeField] GameObject characterClassMenu;
        [SerializeField] GameObject characterHairMenu;
        [SerializeField] GameObject characterHairColorMenu;

        [SerializeField] TMP_InputField characterNameInputField;
        [Header("Color Sliders")]
        [SerializeField] private Slider redSlider;
        [SerializeField] private Slider greenSlider;
        [SerializeField] private Slider blueSlider;

        [Header("Hidden Gear")]
        private HeadEquipmentItem hiddenHelmet;

        [Header("Character Slots")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

        [Header("Character Classes")]
        public CharacterClass[] startingClasses;

 
        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
            }
        }
        public void StartNetworkAsHost() // Oress Start
        {
            NetworkManager.Singleton.StartHost();   
        }

        public void AttemptToCreateNewCharacter()
        {
            if(WorldSaveGameManager.Instance.HasFreeCharacterSlot())
            {
                OpenCharacterCreationMenu();
            }
            else
            {
                CloseCharacterCreationMenu();
                DisplayNoFreeCharacterSlotsPopUp();
            }
        }
        public void StartNewGame()
        {
            //WorldSaveGameManager.Instance.LoadWorldScene(WorldSaveGameManager.Instance.GetWorldSceneIndex());
            WorldSaveGameManager.Instance.AttemptToCreateNewGame();
        }
        public void OpenLoadGameMenu()
        {
            titleScreenMainMenu.SetActive(false);
            
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }
        public void ReturnToMainMenu()
        { // CloseLoadGameM
            titleScreenLoadMenu.SetActive(false);
            
            titleScreenMainMenu.SetActive(true);

            mainMenuLoadGameButton.Select();
        }

        // Call : Creation Menu Sex Button
        public void ToggleBodyType()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            player.playerNetworkManager.isMale.Value = !player.playerNetworkManager.isMale.Value;
            if(player.playerNetworkManager.isMale.Value)
            {
                characterSexText.text = "MALE";
            }
            else
            {
                characterSexText.text = "FEMALE";
            }
        }

        public void OpenTitleScreenMainMenu()
        {
            titleScreenMainMenu.SetActive(true);
        }

        public void CloseTitleScreenMainMenu()
        {
            titleScreenMainMenu.SetActive(false);
        }
        public void OpenCharacterCreationMenu()
        {
            CloseTitleScreenMainMenu();
            titleScreenCreationMenu.SetActive(true);

            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            player.playerBodyManager.ToggleBodyType(true); // Set Male Character
        }

        public void CloseCharacterCreationMenu()
        {
            titleScreenCreationMenu.SetActive(false);

            OpenTitleScreenMainMenu();
        }
        
        // Character Creation Menu -> Class Button assigned
        public void OpenChooseCharacterClassSubMenu()
        {
            // Disable Main Menu Buttons
            ToGgleCharacterCreationScreenMainMenuButtons(false);

            // Enable Sub Menu Object (Class List With Buttons)
            characterClassMenu.SetActive(true);

            // Auto Select First Button
            if(characterClassButtons.Length > 0)
            {
                characterClassButtons[0].Select();
                characterClassButtons[0].OnSelect(null);
            }
        }

        public void CloseChooseCharacterClassSubMenu()
        {
            // Re-Enable Main Menu Buttons
            ToGgleCharacterCreationScreenMainMenuButtons(true);

            // Disable Sub Menu Object
            characterClassMenu.SetActive(false);

            // Auto Select "Choose Class Button" (Since it was the last button you hit during the main menu) 
            characterClassButton.Select();
            characterClassButton.OnSelect(null);

        }

        // Character Creation Menu -> Hair Button assigned
        public void OpenChooseHairStyleSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // Disable Main Menu Buttons
            ToGgleCharacterCreationScreenMainMenuButtons(false);

            // Enable Sub Menu Object (Class List With Buttons)
            characterHairMenu.SetActive(true);

            // Auto Select First Button
            if (characterHairButtons.Length > 0)
            {
                characterHairButtons[0].Select();
                characterHairButtons[0].OnSelect(null);
            }
            
            // Store the helmet the player had on
            if(player.playerInventoryManager.headEquipment != null)
            {
                hiddenHelmet = Instantiate(player.playerInventoryManager.headEquipment);
            }

            // Unequip the helmet and Reload the Gear
            player.playerInventoryManager.headEquipment = null;
            player.playerEquipmentManager.EquipArmors();
        }

        public void CloseChooseHairStyleSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // Re-Enable Main Menu Buttons
            ToGgleCharacterCreationScreenMainMenuButtons(true);

            // Disable Sub Menu Object
            characterHairColorMenu.SetActive(false);

            // Auto Select "Choose Hair Button" (Since it was the last button you hit during the main menu) 
            characterHairColorButton.Select();
            characterHairColorButton.OnSelect(null);  
            
            if(hiddenHelmet != null)
            {
                player.playerInventoryManager.headEquipment = hiddenHelmet;
            }

            player.playerEquipmentManager.EquipArmors();
        }

        // Character Creation Menu -> Hair Button assigned
        public void OpenChooseHairColorubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // Disable Main Menu Buttons
            ToGgleCharacterCreationScreenMainMenuButtons(false);

            // Enable Sub Menu Object (Class List With Buttons)
            characterHairColorMenu.SetActive(true);

            // Auto Select First Button
            if (characterHairColorButtons.Length > 0)
            {
                characterHairColorButtons[0].Select();
                characterHairColorButtons[0].OnSelect(null);
            }

            // Store the helmet the player had on
            if (player.playerInventoryManager.headEquipment != null)
            {
                hiddenHelmet = Instantiate(player.playerInventoryManager.headEquipment);
            }

            // Unequip the helmet and Reload the Gear
            player.playerInventoryManager.headEquipment = null;
            player.playerEquipmentManager.EquipArmors();
        }

        public void CloseChooseHairColorSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // Re-Enable Main Menu Buttons
            ToGgleCharacterCreationScreenMainMenuButtons(true);

            // Disable Sub Menu Object
            characterHairMenu.SetActive(false);

            // Auto Select "Choose Hair Button" (Since it was the last button you hit during the main menu) 
            characterHairButton.Select();
            characterHairButton.OnSelect(null);

            if (hiddenHelmet != null)
            {
                player.playerInventoryManager.headEquipment = hiddenHelmet;
            }

            player.playerEquipmentManager.EquipArmors();
        }

        // Character Creation Menu -> Hair Button assigned
        public void OpenChooseNameSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // Disable Main Menu Buttons
            ToGgleCharacterCreationScreenMainMenuButtons(false);

            characterNameButton.gameObject.SetActive(false);
            characterNameMenu.SetActive(true);

            characterNameInputField.Select();
        }

        public void CloseChooseNameSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // Re-Enable Main Menu Buttons
            ToGgleCharacterCreationScreenMainMenuButtons(true);

            characterNameMenu.SetActive(false);
            characterNameButton.gameObject.SetActive(true);

            characterNameButton.Select();

            player.playerNetworkManager.characterName.Value = characterNameInputField.text;


        }
        private void ToGgleCharacterCreationScreenMainMenuButtons(bool status)
        {
            characterNameButton.enabled = status;
            characterClassButton.enabled = status;
            characterHairButton.enabled = status;
            characterHairColorButton.enabled = status;
            characterSexButton.enabled = status;
            startGameButton.enabled = status;
        }
        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            titleScreenMainMenu.SetActive(false);
            noCharacterSlotsPopup.SetActive(true);
            noCharacterSlotsClose.Select();
        }
        public void CloseNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopup.SetActive(false);
            titleScreenMainMenu.SetActive(true) ;
            mainMenuNewGameButton.Select();
        }

        #region CHARACTER SLOTS
        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }
        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }
        public void AttemptToDeleteCharacterSlot()
        {
            if(currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopup.SetActive(true);
                deleteCharacterPopupCancelButton.Select();
            }

        }
        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopup.SetActive(false) ;
            WorldSaveGameManager.Instance.DeleteGame(currentSelectedSlot);

            // Refresh
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }
        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopup.SetActive(false);
            loadMenuReturnButton.Select() ;
        }
        #endregion

        #region Character Class

        // Call: MiddlePanel -> Character Class Menu -> Class Buttons
        // (SelectClass parameter classID) Index == StartingClasses Array Index
        public void SelectClass(int classID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            if (startingClasses.Length <= 0) return;

            startingClasses[classID].SetClass(player);
            CloseChooseCharacterClassSubMenu();
        }

        // Call: MiddlePanel -> Character Class Menu -> Class Buttons (Event Trigger )
        // (SelectClass parameter classID) Index == StartingClasses Array Index
        public void PreviewClass(int classID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            if (startingClasses.Length <= 0) return;

            startingClasses[classID].SetClass(player);
        }
        public void SetCharacterClass(PlayerManager player, 
            int vitality, int endurance, int mind, int strength, int dexterity, int intelligence, int faith,
            WeaponItem[] mainHandWeapons, WeaponItem[] offHandWeapons,
            HeadEquipmentItem headEquipment, BodyEquipmentItem bodyEquipment, LegEquipmentItem legEquipment, HandEquipmentItem handEquipment,
            QuickSlotItem[] quickSlotItem)
        {

            // Hide The Helmet
            hiddenHelmet = null;
            // Set The Stats
            player.playerNetworkManager.vigor.Value = vitality;
            player.playerNetworkManager.endurance.Value = endurance;
            player.playerNetworkManager.mind.Value = mind;
            player.playerNetworkManager.strength.Value = strength;
            player.playerNetworkManager.dexterity.Value = dexterity;
            player.playerNetworkManager.intelligence.Value = intelligence;
            player.playerNetworkManager.faith.Value = faith;

            // Set The Weapons
            player.playerInventoryManager.weaponsInRightHandSlots[0] = Instantiate(mainHandWeapons[0]);
            player.playerInventoryManager.weaponsInRightHandSlots[1] = Instantiate(mainHandWeapons[1]);
            player.playerInventoryManager.weaponsInRightHandSlots[2] = Instantiate(mainHandWeapons[2]);

            player.playerInventoryManager.currentRightHandWeapon = player.playerInventoryManager.weaponsInRightHandSlots[0];
            player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[0].itemID;

            player.playerInventoryManager.weaponsInLeftHandSlots[0] = Instantiate(offHandWeapons[0]);
            player.playerInventoryManager.weaponsInLeftHandSlots[1] = Instantiate(offHandWeapons[1]);
            player.playerInventoryManager.weaponsInLeftHandSlots[2] = Instantiate(offHandWeapons[2]);

            player.playerInventoryManager.currentLeftHandWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[0];
            player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponsInLeftHandSlots[0].itemID;

            // Set The Armor
            if(headEquipment != null)
            {
                HeadEquipmentItem equipment = Instantiate(headEquipment);
                player.playerInventoryManager.headEquipment = equipment;
            }
            else
            {
                player.playerInventoryManager.headEquipment = null;
            }
            if (bodyEquipment != null)
            {
                BodyEquipmentItem equipment = Instantiate(bodyEquipment);
                player.playerInventoryManager.bodyEquipment = equipment;
            }
            else
            {
                player.playerInventoryManager.bodyEquipment = null;
            }
            if (legEquipment != null)
            {
                LegEquipmentItem equipment = Instantiate(legEquipment);
                player.playerInventoryManager.legEquipment = equipment;
            }
            else
            {
                player.playerInventoryManager.legEquipment = null;
            }
            if (handEquipment != null)
            {
                HandEquipmentItem equipment = Instantiate(handEquipment);
                player.playerInventoryManager.handEquipment = equipment;
            }
            else
            {
                player.playerInventoryManager.handEquipment = null;
            }

            player.playerEquipmentManager.EquipArmors();

            // Set The Quick Slot Items
            player.playerInventoryManager.quickSlotItemIndex = 0;
            if (quickSlotItem[0] != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[0] = Instantiate(quickSlotItem[0]);
            if (quickSlotItem[1] != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[1] = Instantiate(quickSlotItem[1]);
            if (quickSlotItem[2] != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[2] = Instantiate(quickSlotItem[2]);

            player.playerEquipmentManager.LoadQuickSlotEquipment(player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex]);
        }
        #endregion

        #region Character Hair


        // Call: MiddlePanel -> Character Hair Menu -> Hair  Buttons
        // (SelectClass parameter hairID) Index == StartingClasses Array Index
        public void SelectHair(int hairID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            player.playerNetworkManager.hairStyleID.Value = hairID;

            CloseChooseHairStyleSubMenu();
        }

        // Call: MiddlePanel -> Character Hair Menu -> Hair Buttons (Event Trigger )
        // (SelectClass parameter hairID) Index == StartingClasses Array Index
        public void PreviewHair(int hairID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            player.playerNetworkManager.hairStyleID.Value = hairID;

        }

        // Call: MiddlePanel -> Character Hair Menu -> Hair Color Buttons
        public void SelectHairColor()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            player.playerNetworkManager.hairColorRed.Value = redSlider.value;
            player.playerNetworkManager.hairColorGreen.Value = greenSlider.value;
            player.playerNetworkManager.hairColorBlue.Value = blueSlider.value;

            CloseChooseHairColorSubMenu();
        }

        // Call: MiddlePanel -> Character Hair Menu -> Hair Color Buttons (Event Trigger )

        public void PreviewHairColor()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            player.playerNetworkManager.hairColorRed.Value = redSlider.value;
            player.playerNetworkManager.hairColorGreen.Value = greenSlider.value;
            player.playerNetworkManager.hairColorBlue.Value = blueSlider.value;
        }

        public void SetRedColorSlider(float redValue)
        {
            redSlider.value = redValue;
        }
        public void SetGreenColorSlider(float greenValue)
        {
            greenSlider.value = greenValue;
        }
        public void SetBlueColorSlider(float blueValue)
        {
            blueSlider.value = blueValue;
        }
        #endregion
    }

}
