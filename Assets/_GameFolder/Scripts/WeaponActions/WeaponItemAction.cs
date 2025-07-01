using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    // DEBUG SCRIPT
    [CreateAssetMenu(menuName = "Character Actions/ Weapon Actions/ TestAction")]
    public class WeaponItemAction : ScriptableObject
    {
        public int actionID;

        public virtual void AttemptToPerformAction(PlayerManager playerPerformAction, WeaponItem weaponPerformAction)
        {
            if(playerPerformAction.IsOwner)
            { 
                playerPerformAction.playerNetworkManager.currentWeaponBeingUsed.Value = weaponPerformAction.itemID;  
            }
            
        }
    }
}