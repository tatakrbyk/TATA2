using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace XD
{
    public class TitleScreenManager : MonoBehaviour
    {
        private static TitleScreenManager instance; public static TitleScreenManager Instance { get { return instance; } }

        [Header("Menus")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;

        [Header("Buttons")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [SerializeField] Button mainMenuNewGameButton;

        [Header("Pop Up")]
        [SerializeField] GameObject noCharacterSlotsPopup;
        [SerializeField] Button noCharacterSlotsClose;
        [SerializeField] GameObject deleteCharacterSlotPopup;
        [SerializeField] Button deleteCharacterPopupConfirmButton;
        [SerializeField] Button deleteCharacterPopupCancelButton;

        [Header("Character Slots")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

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

        public void StartNewGame()
        {
            StartCoroutine(WorldSaveGameManager.Instance.LoadWorldScene());
        }
        public void OpenLoadGameMenu()
        {
            titleScreenMainMenu.SetActive(false);
            
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }
        public void ReturnToMainMenu()
        {
            titleScreenLoadMenu.SetActive(false);
            
            titleScreenMainMenu.SetActive(true);

            mainMenuLoadGameButton.Select();
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
    }

}
