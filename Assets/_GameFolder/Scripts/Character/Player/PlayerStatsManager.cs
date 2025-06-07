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
        }

    }

}


    
