using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XD;

namespace XD
{
    public class PlayerSoundFXManager : CharacterSoundFXManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }
        public override void PlayBlockSoundFX()
        {
            PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(player.playerCombatManager.currentWeaponBeingUsed.blocking));
        }
    }

}
