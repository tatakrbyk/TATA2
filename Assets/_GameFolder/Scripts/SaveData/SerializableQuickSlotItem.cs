using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace XD
{
    [System.Serializable]
    public class SerializableQuickSlotItem : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;
        [SerializeField] public int itemAmount;


        public QuickSlotItem GetQuickSlotItem()
        {
            QuickSlotItem quickSlotItem = WorldItemDatabase.Instance.GetQuickSlotItemSerializedData(this);
            return quickSlotItem;
        }

        #region ISerializationCallbackReceiver Implementation
        public void OnAfterDeserialize()
        {
        }
        public void OnBeforeSerialize()
        {
        }
        #endregion

    }
}
