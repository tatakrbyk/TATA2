using System.Collections;
using TMPro;
using Unity.Netcode;
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

        [Header("Runes")]
        private float runeUpdateCountDelayTimer = 2.5f;
        private int pendingRunesToAdd = 0;
        private Coroutine WaitThenAddRunesCoroutine;

        [SerializeField] TextMeshProUGUI runesCountText;
        [SerializeField] TextMeshProUGUI runesToAddText;
        [Header("Quick Slots")]
        [SerializeField] Image leftWeaponQuickSlotIcon;
        [SerializeField] Image rightWeaponQuickSlotIcon;
        [SerializeField] Image spellItemQuickSlotIcon;
        [SerializeField] Image quickSlotItemQuickSlotIcon;
        [SerializeField] Image mainProjectileQuickSlotIcon;
        [SerializeField] Image secondaryProjectileQuickSlotIcon;
        
        [SerializeField] TextMeshProUGUI mainProjectileAmountText;
        [SerializeField] TextMeshProUGUI secondaryProjectileAmountText;
        [SerializeField] TextMeshProUGUI quickSlotconCountText;

        [SerializeField] GameObject projectileQuickSlotsGameObject; 

        [Header("Boss Health BAR")]
        public Transform bossHealthBarParent;
        public GameObject bossHealthBarObject;

        [Header("Crosshair")]
        public GameObject crosshair;

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

        public void SetRunesCount(int runesToAdd)
        {
            pendingRunesToAdd += runesToAdd;

            if (WaitThenAddRunesCoroutine != null)
            {
                StopCoroutine(WaitThenAddRunesCoroutine);
            }
            WaitThenAddRunesCoroutine = StartCoroutine(WaitThenUpdateRuneCount());
        }

        public IEnumerator WaitThenUpdateRuneCount()
        {
            float timer = runeUpdateCountDelayTimer;
            int runesToAdd = pendingRunesToAdd;

            if(runesToAdd >= 0)
            {
                runesToAddText.text = "+ " + runesToAdd.ToString();
            }
            else
            {
                runesToAddText.text = "- " + Mathf.Abs(runesToAdd).ToString();
            }
            runesToAddText.enabled = true;

            while (timer > 0)
            {
                timer -= Time.deltaTime;

                // If More runes are qued up, re-update total, new rune count
                if(runesToAdd != pendingRunesToAdd)
                {
                    runesToAdd = pendingRunesToAdd;
                    runesToAddText.text = "+" + runesToAdd.ToString();
                }

                yield return null;
            }

            runesToAddText.enabled = false;
            pendingRunesToAdd = 0;
            runesCountText.text = PlayerUIManager.Instance.localPlayer.playerStatsManager.runes.ToString();

            yield return null;
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
        
        public void SetQuickSlotItemQuickSlotIcon(QuickSlotItem quickSlotItem)
        {

            if (quickSlotItem == null)
            {
                quickSlotItemQuickSlotIcon.enabled = false;
                quickSlotItemQuickSlotIcon.sprite = null;
                quickSlotconCountText.enabled = false;

                return;
            }
            if (quickSlotItem.itemIcon == null)
            {
                quickSlotItemQuickSlotIcon.enabled = false;
                quickSlotItemQuickSlotIcon.sprite = null;
                quickSlotconCountText.enabled = false;

                return;
            }

            quickSlotItemQuickSlotIcon.sprite = quickSlotItem.itemIcon;
            quickSlotItemQuickSlotIcon.enabled = true;
            if (quickSlotItem.isConsumable)
            {
                quickSlotconCountText.text = quickSlotItem.GetCurrentAmount(PlayerUIManager.Instance.localPlayer).ToString();
                quickSlotconCountText.enabled = true;
            }
            else
            {
                quickSlotconCountText.enabled = false;
            }

        }
        public void SetMainProjectileQuickSlotIcon(RangedProjectileItem projectileItem)
        {
            if (projectileItem == null)
            {
                Debug.LogError("Projectile Item is null.");
                mainProjectileQuickSlotIcon.enabled = false;
                mainProjectileQuickSlotIcon.sprite = null;
                mainProjectileAmountText.enabled = false;
                return;
            }
            if (projectileItem.itemIcon == null)
            {
                Debug.LogError("Projectile Item does not have an icon.");
                mainProjectileQuickSlotIcon.enabled = false;
                mainProjectileQuickSlotIcon.sprite = null;
                mainProjectileAmountText.enabled = false;
                return;
            }

            mainProjectileQuickSlotIcon.sprite = projectileItem.itemIcon;
            mainProjectileAmountText.text = projectileItem.currentAmmoAmount.ToString();
            mainProjectileQuickSlotIcon.enabled = true;
            mainProjectileAmountText.enabled = true;
        }

        public void SetSecondaryProjectileQuickSlotIcon(RangedProjectileItem projectileItem)
        {
            if (projectileItem == null)
            {
                Debug.LogError("Projectile Item is null.");
                secondaryProjectileQuickSlotIcon.enabled = false;
                secondaryProjectileQuickSlotIcon.sprite = null;
                secondaryProjectileAmountText.enabled = false;
                return;
            }
            if (projectileItem.itemIcon == null)
            {
                Debug.LogError("Projectile Item does not have an icon.");
                secondaryProjectileQuickSlotIcon.enabled = false;
                secondaryProjectileQuickSlotIcon.sprite = null;
                secondaryProjectileAmountText.enabled = false;
                return;
            }
            secondaryProjectileQuickSlotIcon.sprite = projectileItem.itemIcon;
            secondaryProjectileAmountText.text = projectileItem.currentAmmoAmount.ToString();
            secondaryProjectileQuickSlotIcon.enabled = true;
            secondaryProjectileAmountText.enabled = true;
        }

        public void ToggleProjectileQuickSlotsVisibility(bool status)
        {
            projectileQuickSlotsGameObject.SetActive(status);
        }
    }

}
