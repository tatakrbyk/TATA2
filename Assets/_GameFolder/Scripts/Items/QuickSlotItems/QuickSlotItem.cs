using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class QuickSlotItem : Item
    {
        [Header("ItemModel")]
        [SerializeField] protected GameObject ItemModel;

        [Header("Animation")]
        [SerializeField] protected string useItemAnimation;

        // Not all quick slot items are consumables
        [Header("Consumable")]
        public bool isConsumable;
        public int itemAmount = 1;
        public virtual void AttemptToUseItem(PlayerManager player)
        {
            if(!CanIUseThisItem(player)) { return; }

            player.playerAnimatorManager.PlayActionAnimation(useItemAnimation, true);
        }

        public virtual void SuccessfullyUseItem(PlayerManager player)
        {

        }

        public virtual bool CanIUseThisItem(PlayerManager player)
        {
            return true;
        }

        public virtual int GetCurrentAmount(PlayerManager player)
        {
            return 0;
        }
    }
}
