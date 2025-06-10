using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using System.Linq;

namespace XD
{
    public class WorldItemDatabase : MonoBehaviour
    {
        private static WorldItemDatabase instance; public static WorldItemDatabase Instance { get { return instance; } }

        public WeaponItem unarmedWeapon;

        [Header("Weapons Database")]
        [SerializeField] private List<WeaponItem> weapons = new List<WeaponItem>();

        [Header("Items Database")]
        // Every Item we have in the game
        private List<Item> items = new List<Item>();
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            foreach(var weapon in weapons)
            {
                items.Add(weapon);
            }

            // Assign of our items a unique item ID
            for(int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }

        public WeaponItem GetWeaponByID(int id)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == id);
        }


    }

}
