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



    }
}
