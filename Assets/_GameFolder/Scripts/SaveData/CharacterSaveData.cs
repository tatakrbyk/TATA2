using UnityEngine;

namespace XD
{
    [System.Serializable]
    public class CharacterSaveData 
    {
        [Header("SCENE INDEX")]
        public int sceneIndex = 1;

        [Header("CHARACTER NAME")]
        public string characterName = "Character";

        [Header("TIME PLAYED")]
        public float secondsPlayed;

        [Header("World Coordinates")]
        public float xCoord;
        public float yCoord;
        public float zCoord;

        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;
        
        [Header("Stats")]
        public int vitality;
        public int endurance;

        [Header("Bossess")]
        public SerializableDictionary<int, bool> sitesOfGrace; // Int = Site Of Grace ID, bool = Activated Status

        [Header("Bossess")]
        public SerializableDictionary<int, bool> bossesAwakened; // Int = Boss ID, bool = Awakened Status
        public SerializableDictionary<int, bool> bossesDefeated; // Int = Boss ID, bool = Defeated Status

        public CharacterSaveData()
        {
            sitesOfGrace = new SerializableDictionary<int, bool>();
            bossesAwakened = new SerializableDictionary<int, bool>();
            bossesDefeated = new SerializableDictionary<int, bool>();
        }
    }
}
