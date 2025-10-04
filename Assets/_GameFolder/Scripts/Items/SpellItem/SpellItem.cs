using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace XD
{
    public class SpellItem : Item
    {
        [Header("Spell Class")]
        public SpellClass spellClass;

        [Header("Spell Modifiers")]
        public int fullChargeEffectMultiplier = 2;

        [Header("Spell Costs")]
        public int spellSlotUsed = 1;
        public int staminaCost = 25;
        public int focusPointCost = 25;

        [Header("Spell FX")]
        [SerializeField] protected GameObject spellCastWarmUpFX;
        [SerializeField] protected GameObject spellCastReleaseFX;
        [SerializeField] protected GameObject spellChargeFX;
        [SerializeField] protected GameObject spellCastReleaseFXFullCharge;
        // TODO. Full Charge Version Of FX

        [Header("Animations")]
        [SerializeField] protected string mainHandSpellAnimation;
        [SerializeField] protected string offHandSpellAnimation;

        [Header("Sound FX")]
        public AudioClip warmUpSoundFX;
        public AudioClip releaseSoundFX;

        // This is where you play the "Warm Up" Animation
        public virtual void AttemptToCastSpell(PlayerManager player)
        {

        }


        // Spell FX that are instantiated when attempting to cast the spell
        public virtual void InstantiateWarmUpSpellFX(PlayerManager player)
        {

        }

        // This is where you play the "Throw" or "Cast" Animation
        public virtual void SuccessfullyCastSpell(PlayerManager player)
        {
            if (player.IsOwner)
            {
                player.playerNetworkManager.currentFocusPoints.Value -= focusPointCost;
                player.playerNetworkManager.currentStamina.Value -= staminaCost;
            }
        }

        public virtual void SuccessfullyChargeSpell(PlayerManager player)
        {

        }
        // This is where you play the "Throw" or "Cast" Animation
        public virtual void SuccessfullyCastSpellFullCharge(PlayerManager player)
        {
            if (player.IsOwner)
            {
                player.playerNetworkManager.currentFocusPoints.Value -= Mathf.RoundToInt(focusPointCost * fullChargeEffectMultiplier);
                player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaCost * fullChargeEffectMultiplier);
            }
        }


        // Spell FX that are instantiated when when spell has been successfully cast
        public virtual void InstantiateReleaseFX(PlayerManager player)
        {
        }

        // Helper function to check weather or not we are able to use the spell when attempting to cast 
        public virtual bool CanICastThisSpell(PlayerManager player)
        {
            if(player.playerNetworkManager.currentFocusPoints.Value <= focusPointCost) { return false; }   
            if (player.playerNetworkManager.currentStamina.Value <= staminaCost) { return false; }
            if (player.isPerformingAction) { return false; }
            if (player.playerNetworkManager.isJumping.Value) { return false; }
            return true;
        }

   


    }

}
