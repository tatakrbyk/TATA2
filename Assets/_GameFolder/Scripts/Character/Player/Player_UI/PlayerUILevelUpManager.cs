using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
namespace XD
{
    public class PlayerUILevelUpManager : PlayerUIMenu
    {
        [Header("Levels0")]
        [SerializeField] private int[] playerLevels = new int[100];
        [SerializeField] private int baseLevelCost = 83;
        [SerializeField] private int totalLevelUpCost = 0;

        [Header("Character Stats")]
        [SerializeField] TextMeshProUGUI characterLevelText;
        [SerializeField] TextMeshProUGUI runesHeldText;
        [SerializeField] TextMeshProUGUI runesNeededText;

        [SerializeField] TextMeshProUGUI vigorLevelText;
        [SerializeField] TextMeshProUGUI mindLevelText;
        [SerializeField] TextMeshProUGUI enduranceLevelText;
        [SerializeField] TextMeshProUGUI strengthLevelText;
        [SerializeField] TextMeshProUGUI dexterityLevelText;
        [SerializeField] TextMeshProUGUI intelligenceLevelText;
        [SerializeField] TextMeshProUGUI faithLevelText;


        [Header("Projected Character Stats")]
        [SerializeField] TextMeshProUGUI projectedCharacterLevelText;
        [SerializeField] TextMeshProUGUI projectedRunesHeldText;

        [SerializeField] TextMeshProUGUI projectedVigorLevelText;
        [SerializeField] TextMeshProUGUI projectedMindLevelText;
        [SerializeField] TextMeshProUGUI projectedEnduranceLevelText;
        [SerializeField] TextMeshProUGUI projectedStrengthLevelText;
        [SerializeField] TextMeshProUGUI projectedDexterityLevelText;
        [SerializeField] TextMeshProUGUI projectedIntelligenceLevelText;
        [SerializeField] TextMeshProUGUI projectedFaithLevelText;


        [Header("Sliders")]
        public CharacterAttribute currentSelectedAttribute;
        public Slider vigorSlider;
        public Slider mindSlider;
        public Slider enduranceSlider;
        public Slider strengthSlider;
        public Slider dexteritySlider;
        public Slider intelligenceSlider;
        public Slider faithSlider;

        [Header("Buttons")]
        [SerializeField] Button confirmLevelsButton;
        private void Awake()
        {
            SetAllLevelsCost();
        }
        public override void OpenMenu()
        {
            base.OpenMenu();

            SetCurrentStats();
        }
        public override void CloseMenu()
        {
            CloseMenuAfterFixedFrame();
        }

        private void SetCurrentStats()
        {
            // Character Level
            characterLevelText.text = PlayerUIManager.Instance.localPlayer.characterStatsManager.CalculateCharacterLevelBasedOnAttributes().ToString();
            projectedCharacterLevelText.text = PlayerUIManager.Instance.localPlayer.characterStatsManager.CalculateCharacterLevelBasedOnAttributes().ToString();

            // Runes
            runesHeldText.text = PlayerUIManager.Instance.localPlayer.playerStatsManager.runes.ToString();
            projectedRunesHeldText.text = PlayerUIManager.Instance.localPlayer.playerStatsManager.runes.ToString();
            runesNeededText.text = "0";

            // Attributes
            vigorLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.vigor.Value.ToString();
            projectedVigorLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.vigor.Value.ToString();
            vigorSlider.minValue = PlayerUIManager.Instance.localPlayer.playerNetworkManager.vigor.Value;

            mindLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.mind.Value.ToString();
            projectedMindLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.mind.Value.ToString();
            mindSlider.minValue = PlayerUIManager.Instance.localPlayer.playerNetworkManager.mind.Value;

            enduranceLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.endurance.Value.ToString();
            projectedEnduranceLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.endurance.Value.ToString();
            enduranceSlider.minValue = PlayerUIManager.Instance.localPlayer.playerNetworkManager.endurance.Value;

            strengthLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.strength.Value.ToString();
            projectedStrengthLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.strength.Value.ToString();
            strengthSlider.minValue = PlayerUIManager.Instance.localPlayer.playerNetworkManager.strength.Value;

            dexterityLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.dexterity.Value.ToString();
            projectedDexterityLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.dexterity.Value.ToString();
            dexteritySlider.minValue = PlayerUIManager.Instance.localPlayer.playerNetworkManager.dexterity.Value;

            intelligenceLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.intelligence.Value.ToString();
            projectedIntelligenceLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.intelligence.Value.ToString();
            intelligenceSlider.minValue = PlayerUIManager.Instance.localPlayer.playerNetworkManager.intelligence.Value;

            faithLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.faith.Value.ToString();
            projectedFaithLevelText.text = PlayerUIManager.Instance.localPlayer.playerNetworkManager.faith.Value.ToString();
            faithSlider.minValue = PlayerUIManager.Instance.localPlayer.playerNetworkManager.faith.Value;

            vigorSlider.OnSelect(null);
            vigorSlider.Select();

        }

        // Called Every time a level by slider OnValueChanged event
        public void UpdateSliderBasedOnCurrentlySelectedAttribute()
        {
            PlayerManager player = PlayerUIManager.Instance.localPlayer;

            switch (currentSelectedAttribute)
            {
                case CharacterAttribute.Vigor:
                    projectedVigorLevelText.text = vigorSlider.value.ToString();
                    break;
                case CharacterAttribute.Mind:
                    projectedMindLevelText.text = mindSlider.value.ToString();
                    break;
                case CharacterAttribute.Endurance:
                    projectedEnduranceLevelText.text = enduranceSlider.value.ToString();
                    break;
                case CharacterAttribute.Strength:
                    projectedStrengthLevelText.text = strengthSlider.value.ToString();
                    break;
                case CharacterAttribute.Dexterity:
                    projectedDexterityLevelText.text = dexteritySlider.value.ToString();
                    break;
                case CharacterAttribute.Intelligence:
                    projectedIntelligenceLevelText.text = intelligenceSlider.value.ToString();
                    break;
                case CharacterAttribute.Faith:
                    projectedFaithLevelText.text = faithSlider.value.ToString();
                    break;
            }

            // Passes our current level and our projected level to set our cost for leveling up
            CalculateLevelCost(
                player.characterStatsManager.CalculateCharacterLevelBasedOnAttributes(),
                player.characterStatsManager.CalculateCharacterLevelBasedOnAttributes(true));

            projectedCharacterLevelText.text = player.characterStatsManager.CalculateCharacterLevelBasedOnAttributes(true).ToString();
            runesNeededText.text = totalLevelUpCost.ToString();

            // Check Cost
            if (totalLevelUpCost > player.playerStatsManager.runes)
            {
                confirmLevelsButton.interactable = false;
            }
            else
            {
                confirmLevelsButton.interactable = true;
            }

            ChangeTextColorsDependingOnCosts();
        }

        public void ConfirmLevels()
        {
            PlayerManager player = PlayerUIManager.Instance.localPlayer;

            // Deduct cost from total runes
            player.playerStatsManager.runes -= totalLevelUpCost;

            // Set new stats
            player.playerNetworkManager.vigor.Value = Mathf.RoundToInt(vigorSlider.value);
            player.playerNetworkManager.mind.Value = Mathf.RoundToInt(mindSlider.value);
            player.playerNetworkManager.endurance.Value = Mathf.RoundToInt(enduranceSlider.value);
            player.playerNetworkManager.strength.Value = Mathf.RoundToInt(strengthSlider.value);
            player.playerNetworkManager.dexterity.Value = Mathf.RoundToInt(dexteritySlider.value);
            player.playerNetworkManager.intelligence.Value = Mathf.RoundToInt(intelligenceSlider.value);
            player.playerNetworkManager.faith.Value = Mathf.RoundToInt(faithSlider.value);

            SetCurrentStats();
            ChangeTextColorsDependingOnCosts();

            // Save Game After setting stats
            WorldSaveGameManager.Instance.SaveGame();
        }

        private void SetAllLevelsCost()
        {
            for(int i = 0; i < playerLevels.Length; i++)
            {
                // Level 0 has no cost
                if (i == 0) continue;

                playerLevels[i] = baseLevelCost + (50 * i);
            }
        }

        private void CalculateLevelCost(int currentLevel, int projectedLevel)
        {
            // We don't  want to chargefor levels we already paid for
            // EX, If you are level 21 we don't add the cost of the first 21 levels
            int totalCost = 0;

            for (int i = 0; i < projectedLevel; i++)
            {
                // Do not charge until we get past our current level 
                if (i < currentLevel) continue;

                // This is a safeguard to stop adding cost if the players level some how exceeds the size of the array we have created
                if (i > playerLevels.Length) continue;
                totalCost += playerLevels[i];
            }

            totalLevelUpCost = totalCost;

            projectedRunesHeldText.text = (PlayerUIManager.Instance.localPlayer.playerStatsManager.runes - totalCost).ToString();

            if(totalCost > PlayerUIManager.Instance.localPlayer.playerStatsManager.runes)
            {
                runesNeededText.color = Color.red;
            }
            else
            {
                runesNeededText.color = Color.white;
            }
        }

        private void ChangeTextColorsDependingOnCosts()
        {
            PlayerManager player = PlayerUIManager.Instance.localPlayer;

            int projectedVigorLevel = Mathf.RoundToInt(vigorSlider.value);
            int projectedMindLevel = Mathf.RoundToInt(mindSlider.value);
            int projectedEnduranceLevel = Mathf.RoundToInt(enduranceSlider.value);
            int projectedStrengthLevel = Mathf.RoundToInt(strengthSlider.value);
            int projectedDexterityLevel = Mathf.RoundToInt(dexteritySlider.value);
            int projectedIntelligenceLevel = Mathf.RoundToInt(intelligenceSlider.value);
            int projectedFaithLevel = Mathf.RoundToInt(faithSlider.value);

            ChangeTextFieldToSpecificColorBasedOnStat(player, projectedVigorLevelText, player.playerNetworkManager.vigor.Value, projectedVigorLevel);
            ChangeTextFieldToSpecificColorBasedOnStat(player, projectedMindLevelText, player.playerNetworkManager.mind.Value, projectedMindLevel);
            ChangeTextFieldToSpecificColorBasedOnStat(player, projectedEnduranceLevelText, player.playerNetworkManager.endurance.Value, projectedEnduranceLevel);
            ChangeTextFieldToSpecificColorBasedOnStat(player, projectedStrengthLevelText, player.playerNetworkManager.strength.Value, projectedStrengthLevel);
            ChangeTextFieldToSpecificColorBasedOnStat(player, projectedDexterityLevelText, player.playerNetworkManager.dexterity.Value, projectedDexterityLevel);
            ChangeTextFieldToSpecificColorBasedOnStat(player, projectedIntelligenceLevelText, player.playerNetworkManager.intelligence.Value, projectedIntelligenceLevel);
            ChangeTextFieldToSpecificColorBasedOnStat(player, projectedFaithLevelText, player.playerNetworkManager.faith.Value, projectedFaithLevel);

            int projctedPlayerLevel = player.characterStatsManager.CalculateCharacterLevelBasedOnAttributes(true);
            int playerLevel = player.characterStatsManager.CalculateCharacterLevelBasedOnAttributes();

            if(projctedPlayerLevel == playerLevel)
            {
                projectedCharacterLevelText.color = Color.white;
                projectedRunesHeldText.color = Color.blue;
                runesNeededText.color = Color.white;
            }

            // we can afford it
            if (totalLevelUpCost <= player.playerStatsManager.runes)
            {
                runesNeededText.color = Color.white;

                if(projctedPlayerLevel > playerLevel)
                {
                    projectedRunesHeldText.color = Color.red;
                    projectedCharacterLevelText.color = Color.blue;
                }
            }
            // we cant afford it
            else
            {
                runesNeededText.color = Color.red;
                if (projctedPlayerLevel > playerLevel)
                {
                    projectedCharacterLevelText.color = Color.red;
                }

            }
        }

        private void ChangeTextFieldToSpecificColorBasedOnStat(PlayerManager player, TextMeshProUGUI textField, int stat, int projectedStat)
        {
            if(projectedStat == stat)
            {
                textField.color = Color.white;
            }
            // we can afford it
            if (totalLevelUpCost <= player.playerStatsManager.runes)
            {
                if (projectedStat > stat)
                {
                    textField.color = Color.blue;
                }
                else
                {
                    // Same stats
                    textField.color = Color.white;
                }
            }
            // we cant afford it
            else
            {
                if (projectedStat > stat)
                {
                    textField.color = Color.red;
                }
                else
                {
                    textField.color = Color.white;
                }
            }
        }
    }
}
