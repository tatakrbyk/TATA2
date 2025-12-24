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

        public GameObject pickUpItemPrefab;

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

        [Header("Spells")]
        [SerializeField] List<SpellItem> spells = new List<SpellItem>();

        [Header("Projectiles")]
        [SerializeField] List<RangedProjectileItem> projectiles = new List<RangedProjectileItem>();

        [Header("Quick Slot")]
        [SerializeField] List<QuickSlotItem> quickSlotItems = new List<QuickSlotItem>();

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
            foreach (var spell in spells)
            {
                items.Add(spell);
            }
            foreach (var projectile in projectiles)
            {
                items.Add(projectile);
            }

            foreach(var item in quickSlotItems)
            {
                items.Add(item);
            }
            // Assign of our items a unique item ID
            for (int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }

        #region Item Database
        public Item GetItemByID(int ID)
        {
            return items.FirstOrDefault(item => item.itemID == ID);
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

        public SpellItem GetSpellByID(int ID)
        {
            return spells.FirstOrDefault(spell => spell.itemID == ID);
        }
        public RangedProjectileItem GetProjectileByID(int ID)
        {
            return projectiles.FirstOrDefault(projectile => projectile.itemID == ID);
        }
        
        public QuickSlotItem GetQuickSlotItemByID(int ID)
        {
            return quickSlotItems.FirstOrDefault(item => item.itemID == ID);
        }
        #endregion

        #region Item Serialization

        public WeaponItem GetWeaponFromSerializedData(SerializableWeapon serializableWeapon)
        {
            WeaponItem weapon = null;
            
            if(GetWeaponByID(serializableWeapon.itemID))
            {
                weapon = Instantiate(GetWeaponByID(serializableWeapon.itemID));
            }

            if (weapon == null)
            {
                return Instantiate(unarmedWeapon);
            }

            if(GetAshOfWarByID(serializableWeapon.ashOfWarID))
            {
                AshOfWar ashOfWar = Instantiate(GetAshOfWarByID(serializableWeapon.ashOfWarID));
                weapon.ashOfWarAction = ashOfWar;
            }
            return weapon;
        }

        public RangedProjectileItem GetRangedProjectileFromSerializedData(SerializableRangedProjectile serializableRangedProjectile)
        {
            RangedProjectileItem rangedProjectile = null;

            if (GetProjectileByID(serializableRangedProjectile.itemID))
            {
                rangedProjectile = Instantiate(GetProjectileByID(serializableRangedProjectile.itemID));
                rangedProjectile.currentAmmoAmount = serializableRangedProjectile.itemAmount;
            }

            return rangedProjectile;
        }

        public FlaskItem GetFlaskFromSerializedData(SerializableFlask serializableFlask)
        {
            FlaskItem flask = null;
            if (GetQuickSlotItemByID(serializableFlask.itemID))
            {
                flask = Instantiate(GetQuickSlotItemByID(serializableFlask.itemID)) as FlaskItem;
            }
            return flask;
        }

        public QuickSlotItem GetQuickSlotItemSerializedData(SerializableQuickSlotItem serializableQickSlotItem)
        {
            QuickSlotItem quickSlotItem = null;

            if(GetQuickSlotItemByID(serializableQickSlotItem.itemID))
            {
                quickSlotItem = Instantiate(GetQuickSlotItemByID(serializableQickSlotItem.itemID));
                quickSlotItem.itemAmount = serializableQickSlotItem.itemAmount;
            }
            return quickSlotItem;
        }
        #endregion
    }

}
