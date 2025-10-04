using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace XD
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup[] canvasGroup;
        [Header("Stats Bars")]
        [SerializeField] UI_StatBar healthBar;
        [SerializeField] UI_StatBar staminaBar;
        [SerializeField] UI_StatBar FocusPointBar;

        [Header("Quick Slots")]
        [SerializeField] Image leftWeaponQuickSlotIcon;
        [SerializeField] Image rightWeaponQuickSlotIcon;
        [SerializeField] Image spellItemQuickSlotIcon;

        [Header("Boss Health BAR")]
        public Transform bossHealthBarParent;
        public GameObject bossHealthBarObject;

        public void ToggleHUD(bool status)
        {
            if(status)
            {
                foreach(var canvas in canvasGroup)
                {
                    canvas.alpha = 1f;
                }                
            }
            else
            {
                foreach (var canvas in canvasGroup)
                {
                    canvas.alpha = 0f;
                }
            }
        }
        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);

            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);

            FocusPointBar.gameObject.SetActive(false);
            FocusPointBar.gameObject.SetActive(true);
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

        public void SetNewFocusPointValue(int oldValue, int newValue)
        {
            FocusPointBar.SetStat(newValue);
        }
        public void SetMaxFocusPointValue(int maxValue)
        {
            FocusPointBar.SetMaxStat(maxValue);
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


        public void SetSpellItemQuickSlotIcon(int spellID)
        {
            SpellItem spellItem = WorldItemDatabase.Instance.GetSpellByID(spellID);

            if (spellItem == null)
            {
                Debug.LogError("Spell with ID " + spellID + " not found in the database.");
                spellItemQuickSlotIcon.enabled = false;
                spellItemQuickSlotIcon.sprite = null;
                return;
            }
            if (spellItem.itemIcon == null)
            {
                Debug.LogError("Spell with ID " + spellID + " does not have an icon.");
                spellItemQuickSlotIcon.enabled = false;
                spellItemQuickSlotIcon.sprite = null;
                return;
            }

            spellItemQuickSlotIcon.sprite = spellItem.itemIcon;
            spellItemQuickSlotIcon.enabled = true;

        }


    }

}
