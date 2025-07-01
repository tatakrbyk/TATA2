using UnityEngine;
using UnityEngine.UI;

namespace XD
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [Header("Stats Bars")]
        [SerializeField] UI_StatBar healthBar;
        [SerializeField] UI_StatBar staminaBar;

        [Header("Quick Slots")]
        [SerializeField] Image leftWeaponQuickSlotIcon;
        [SerializeField] Image rightWeaponQuickSlotIcon;
        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);

            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }
        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetStat(newValue);  
        }

        public void SetMaxHealthValue(int maxValue)
        {
            healthBar.SetMaxStat(maxValue);
        }
        public void SetNewStaminaValue(float oldValue,  float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));        
        }

        public void SetMaxStaminaValue(int maxValue)
        {
            staminaBar.SetMaxStat(maxValue);
        }

        public void SetLeftWeaponQuickSlotIcon(int WeaponID)
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(WeaponID);

            if (weapon == null)
            {
                Debug.LogError("Weapon with ID " + WeaponID + " not found in the database.");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }
            if (weapon.itemIcon == null)
            {
                Debug.LogError("Weapon with ID " + WeaponID + " does not have an icon.");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }
            leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            leftWeaponQuickSlotIcon.enabled = true;
        }
        
            
        

        public void SetRightWeaponQuickSlotIcon(int WeaponID)
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(WeaponID);

            if(weapon == null)
            {
                Debug.LogError("Weapon with ID " + WeaponID + " not found in the database.");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }
            if (weapon.itemIcon == null)
            {
                Debug.LogError("Weapon with ID " + WeaponID + " does not have an icon.");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }

            rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            rightWeaponQuickSlotIcon.enabled = true;

        }


    }

}
