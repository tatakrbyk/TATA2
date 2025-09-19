using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class AshOfWar : Item
    {
        [Header("Ash of War Info")]
        public WeaponClass[] usableWeaponClasses;

        [Header("Costs")]
        public int focusPointCost = 20;
        public int staminaCost = 20;

        // The Functiom Attempting To Perform The Ash of War 

        public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction)
        {
            Debug.Log("Performend");
        }


        // A Helper Function used to determine if we can in this moment use this ash of war
        public virtual bool CanIUseThisAbility(PlayerManager playerPerformingAction)
        {
            return false;
        }

        protected virtual void DeductStaminaCost(PlayerManager playerPerformingAction)
        {
            playerPerformingAction.playerNetworkManager.currentStamina.Value -= staminaCost;
        }

        protected virtual void DeductFocusPointCost(PlayerManager playerPerformingAction)
        {
            //playerPerformingAction.playerNetworkManager.currentFocusPoints.Value -= focusPointCost;
        }
    }

}
