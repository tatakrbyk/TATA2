using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [System.Serializable]
    public class SerializableWeapon : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;
        [SerializeField] public int ashOfWarID;

        public WeaponItem GetWeapon()
        {
            WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponFromSerializedData(this);
            return weapon;
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
