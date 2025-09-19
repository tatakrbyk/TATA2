using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using Unity.VisualScripting;

namespace XD
{
    public class WorldItemDatabase : MonoBehaviour
    {
        private static WorldItemDatabase instance; public static WorldItemDatabase Instance { get { return instance; } }

        public WeaponItem unarmedWeapon;

        [Header("Weapons Database")]
        [SerializeField] private List<WeaponItem> weapons = new List<WeaponItem>();

        [Header("Head Equipment")]
        [SerializeField] List<HeadEquipmentItem> headEquipment = new List<HeadEquipmentItem>();

        [Header("Body Equipment")]
        [SerializeField] List<BodyEquipmentItem> bodyEquipment = new List<BodyEquipmentItem>();

        [Header("Leg Equipment")]
        [SerializeField] List<LegEquipmentItem> legEquipment = new List<LegEquipmentItem>();

        [Header("Hand Equipment")]
        [SerializeField] List<HandEquipmentItem> handEquipment = new List<HandEquipmentItem>();

        [Header("Ashes of War")]
        [SerializeField] List<AshOfWar> ashesOfWar = new List<AshOfWar>();

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
            foreach(var head in headEquipment)
            {
                items.Add(head);
            }
            foreach (var body in bodyEquipment)
            {
                items.Add(body);
            }
            foreach (var leg in legEquipment)
            {
                items.Add(leg);
            }
            foreach (var hand in handEquipment)
            {
                items.Add(hand);
            }
            foreach (var ash in ashesOfWar)
            {
                items.Add(ash);
            }
            // Assign of our items a unique item ID
            for (int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }

        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }

        public HeadEquipmentItem GetHeadEquipmentByID(int ID)
        {
            return headEquipment.FirstOrDefault(head => head.itemID == ID);
        }

        public BodyEquipmentItem GetBodyEquipmentByID(int ID)
        {
            return bodyEquipment.FirstOrDefault(body => body.itemID == ID);
        }

        public LegEquipmentItem GetLegEquipmentByID(int ID)
        {
            return legEquipment.FirstOrDefault(leg => leg.itemID == ID);
        }

        public HandEquipmentItem GetHandEquipmentByID(int ID)
        {
            return handEquipment.FirstOrDefault(hand => hand.itemID == ID);
        }

        public AshOfWar GetAshOfWarByID(int ID)
        {
            return ashesOfWar.FirstOrDefault(ash => ash.itemID == ID);
        }
    }

}
