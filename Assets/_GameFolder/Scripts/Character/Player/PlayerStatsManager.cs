using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;

        protected override void Awake()
        { 
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
            CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
            CalculateFocusPointsBasedOnMindLevel(player.playerNetworkManager.mind.Value);
        }

        public void CalculateTotalArmorAbsorption()
        {
            armorPhysicalDamageAbsorption = 0;
            armorMagicDamageAbsorption = 0;
            armorFireDamageAbsorption = 0;
            armorLightningDamageAbsorption = 0;
            armorHolyDamageAbsorption = 0;

            armorRobustness = 0;
            armorVitality = 0;
            armorImmunity = 0;
            armorFocus = 0;

            basePoiseDefense = 0;

            if(player.playerInventoryManager.headEquipment != null)
            {
                // Damage Resistance
                armorPhysicalDamageAbsorption += player.playerInventoryManager.headEquipment.physicalDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.headEquipment.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.headEquipment.fireDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.headEquipment.lightningDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.headEquipment.holyDamageAbsorption;

                // Status Effect Resistances
                armorRobustness += player.playerInventoryManager.headEquipment.robustness;
                armorVitality += player.playerInventoryManager.headEquipment.vitality;
                armorImmunity += player.playerInventoryManager.headEquipment.immunity;
                armorFocus += player.playerInventoryManager.headEquipment.focus;
                
                // Poise
                basePoiseDefense += player.playerInventoryManager.headEquipment.poise;
            }

            if (player.playerInventoryManager.bodyEquipment != null)
            {
                // Damage Resistance
                armorPhysicalDamageAbsorption += player.playerInventoryManager.bodyEquipment.physicalDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.bodyEquipment.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.bodyEquipment.fireDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.bodyEquipment.lightningDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.bodyEquipment.holyDamageAbsorption;
                
                // Status Effect Resistances
                armorRobustness += player.playerInventoryManager.bodyEquipment.robustness;
                armorVitality += player.playerInventoryManager.bodyEquipment.vitality;
                armorImmunity += player.playerInventoryManager.bodyEquipment.immunity;
                armorFocus += player.playerInventoryManager.bodyEquipment.focus;
                
                // Poise
                basePoiseDefense += player.playerInventoryManager.bodyEquipment.poise;
            }
            
            if (player.playerInventoryManager.legEquipment != null)
            {
                // Damage Resistance
                armorPhysicalDamageAbsorption += player.playerInventoryManager.legEquipment.physicalDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.legEquipment.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.legEquipment.fireDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.legEquipment.lightningDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.legEquipment.holyDamageAbsorption;

                // Status Effect Resistances
                armorRobustness += player.playerInventoryManager.legEquipment.robustness;
                armorVitality += player.playerInventoryManager.legEquipment.vitality;
                armorImmunity += player.playerInventoryManager.legEquipment.immunity;
                armorFocus += player.playerInventoryManager.legEquipment.focus;

                // Poise
                basePoiseDefense += player.playerInventoryManager.legEquipment.poise;
            }

            if(player.playerInventoryManager.handEquipment != null)
            {
                // Damage Resistance
                armorPhysicalDamageAbsorption += player.playerInventoryManager.handEquipment.physicalDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.handEquipment.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.handEquipment.fireDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.handEquipment.lightningDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.handEquipment.holyDamageAbsorption;

                // Status Effect Resistances
                armorRobustness += player.playerInventoryManager.handEquipment.robustness;
                armorVitality += player.playerInventoryManager.handEquipment.vitality;
                armorImmunity += player.playerInventoryManager.handEquipment.immunity;
                armorFocus += player.playerInventoryManager.handEquipment.focus;
                
                // Poise
                basePoiseDefense += player.playerInventoryManager.handEquipment.poise;  
            }
        }

    }

}


    
