using System.Collections.Generic;
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

        [Header("Dead Spot")]
        public bool hasDeadSpot = false;
        public float deadSpotPositionX;
        public float deadSpotPositionY;
        public float deadSpotPositionZ;
        public int deadSpotRuneCount;

        [Header("Body Type")]
        public bool IsMale = true;
        public int hairStyleID = 0;
        public float hairColorRed;
        public float hairColorGreen;
        public float hairColorBlue;

        [Header("TIME PLAYED")]
        public float secondsPlayed;

        [Header("World Coordinates")]
        public float xCoord;
        public float yCoord;
        public float zCoord;

        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;
        public int currentFocusPoints;
        public int runes;

        [Header("Stats")]
        public int vitality;
        public int mind;
        public int endurance;
        public int strength;
        public int dexterity;
        public int intelligence;
        public int faith;


        [Header("Sites Of Grace")]
        public int lastSiteOfGraveRestedAt = 0;
        public SerializableDictionary<int, bool> sitesOfGrace; // Int = Site Of Grace ID, bool = Activated Status

        [Header("Bossess")]
        public SerializableDictionary<int, bool> bossesAwakened; // Int = Boss ID, bool = Awakened Status
        public SerializableDictionary<int, bool> bossesDefeated; // Int = Boss ID, bool = Defeated Status

        [Header("World Items")]
        public SerializableDictionary<int, bool> worldItemsLooted; //  Int = Item ID, bool = Looted Status
        [Header("Equipment")]
        public int headEquipment;
        public int bodyEquipment;
        public int legEquipment;
        public int handEquipment;

        public int rightWeaponIndex;
        public SerializableWeapon rightWeapon01;
        public SerializableWeapon rightWeapon02;
        public SerializableWeapon rightWeapon03;

        public int leftWeaponIndex;
        public SerializableWeapon leftWeapon01;
        public SerializableWeapon leftWeapon02;
        public SerializableWeapon leftWeapon03;

        public int quickSlotIndex;
        public SerializableQuickSlotItem quickSlotItem01;
        public SerializableQuickSlotItem quickSlotItem02;
        public SerializableQuickSlotItem quickSlotItem03;

        public SerializableRangedProjectile mainProjectile;
        public SerializableRangedProjectile SecondaryProjectile;

        public int currentHealthFlaskRemaining = 3;
        public int currentFocusPointsFlaskRemaining = 1;

        [Header("Inventory")]
        public List<SerializableWeapon> weaponsInInventory;
        public List<SerializableQuickSlotItem> quickSlotItemsInInventory;
        public List<SerializableRangedProjectile> projectilesInInventory;
        public List<int> headEquipmentInInventory;
        public List<int> bodyEquipmentInInventory;
        public List<int> handEquipmentInInventory;
        public List<int> legEquipmentInInventory;

        // TODO: Multiple Spell Slot
        public int currentSpell;

        public CharacterSaveData()
        {
            sitesOfGrace = new SerializableDictionary<int, bool>();
            bossesAwakened = new SerializableDictionary<int, bool>();
            bossesDefeated = new SerializableDictionary<int, bool>();
            worldItemsLooted = new SerializableDictionary<int, bool>();

            weaponsInInventory = new List<SerializableWeapon>();
            quickSlotItemsInInventory = new List<SerializableQuickSlotItem>();
            projectilesInInventory = new List<SerializableRangedProjectile>();
            headEquipmentInInventory = new List<int>();
            bodyEquipmentInInventory = new List<int>();
            handEquipmentInInventory = new List<int>();
            legEquipmentInInventory = new List<int>();
        }
    }
}
