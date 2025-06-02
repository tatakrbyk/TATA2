using UnityEngine;
using System.IO;
using System;


namespace XD
{
    public class SaveFileDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string saveFileName = "";

        public bool CheckToSeeIfFileExists()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
            {
                return true;
            }
            return false;
        }

        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        
        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

            try
            {
                // CREATE THE DIRECTORY IF ITS NOT ALREADY THERE
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("CREATING SAVE FILE AT SAVE PATH " + savePath);

                // SERIALIZE THE DATA INTO JSON
                string dataToStore = JsonUtility.ToJson(characterData, true);

                // WRITE THE FILE 
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch
            {
                Debug.LogError("ERROR WHILE TRYING TO SAVE DATA CHARACTER DATA, GAME NOT SAVED" + savePath + "\n");
            }
        }

       
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;
            
            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    String dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader fileReader = new StreamReader(stream))
                        {
                            dataToLoad = fileReader.ReadToEnd();
                        }
                    }

                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch
                {
                    characterData = null;
                }

            }

            return characterData;
        }
    }
}