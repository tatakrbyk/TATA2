using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace XD
{
    public class UI_Boss_HP_Bar : UI_StatBar
    {
        [SerializeField] AIBossCharacterManager bossCharacter;

        
        public void EnableBossBar(AIBossCharacterManager boss)
        {
            bossCharacter = boss;
            bossCharacter.aiCharacterNetworkManager.currentHealth.OnValueChanged += OnBossHPChanged;
            SetMaxStat(bossCharacter.characterNetworkManager.maxHealth.Value);
            SetStat(bossCharacter.aiCharacterNetworkManager.currentHealth.Value);
            GetComponentInChildren<TextMeshProUGUI>().text = bossCharacter.characterName;

        }

        private void OnDestroy()
        {
            bossCharacter.aiCharacterNetworkManager.currentHealth.OnValueChanged -= OnBossHPChanged;
        }

        private void OnBossHPChanged(int oldValue, int newValue)
        {
            SetStat(newValue);

            if(newValue <= 0)
            {
                RemoveHPBar(2.5f);
            }
        }

        public void RemoveHPBar(float time)
        {
            Destroy(gameObject, time);
        }
    }

}
