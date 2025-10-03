using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace XD
{   
    public class AICharacterNetworkManager : CharacterNetworkManager
    {
        AICharacterManager a›character;

        protected override void Awake()
        {
            base.Awake();
            a›character = GetComponent<AICharacterManager>();
        }

        public override void OnIsDeadChanged(bool oldStatus, bool newStatus)
        {
            base.OnIsDeadChanged(oldStatus, newStatus);

            a›character.aiCharacterInventoryManager.DropItem();
        }

    }
}
