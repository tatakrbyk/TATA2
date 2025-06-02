using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace XD
{
    public class UI_CharacterSaveSlot : MonoBehaviour
    {
        SaveFileDataWriter saveFileWriter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timePlayed;

        private void OnEnable()
        {
            LoadSaveSlot();
        }

        private void LoadSaveSlot()
        {
            saveFileWriter = new SaveFileDataWriter();
            saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

            switch(characterSlot)
            {
                // SAVE SLOT 01
                case CharacterSlot.CharacterSlot_01:
                    saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot01.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                // SAVE SLOT 02
                case CharacterSlot.CharacterSlot_02:
                    saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot02.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                // SAVE SLOT 03
                case CharacterSlot.CharacterSlot_03:
                    saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot03.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                // SAVE SLOT 04
                case CharacterSlot.CharacterSlot_04:
                    saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot04.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                // SAVE SLOT 05
                case CharacterSlot.CharacterSlot_05:
                    saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot05.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                // SAVE SLOT 06
                case CharacterSlot.CharacterSlot_06:
                    saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot06.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                // SAVE SLOT 07
                case CharacterSlot.CharacterSlot_07:
                    saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot07.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                // SAVE SLOT 08
                case CharacterSlot.CharacterSlot_08:
                    saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot08.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                // SAVE SLOT 09
                case CharacterSlot.CharacterSlot_09:
                    saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot09.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                // SAVE SLOT 10
                case CharacterSlot.CharacterSlot_10:
                    saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot10.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.Instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.Instance.LoadGame();
        }

        // SaveSlot Prefab Event Trigger Select
        public void SelectCurrentSlot()
        {
            TitleScreenManager.Instance.SelectCharacterSlot(characterSlot);

        }
    }

}
