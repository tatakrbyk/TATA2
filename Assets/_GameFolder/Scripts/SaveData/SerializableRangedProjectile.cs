using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [System.Serializable]
    public class SerializableRangedProjectile : ISerializationCallbackReceiver
    {

        [SerializeField] public int itemID;
        [SerializeField] public int itemAmount;

        public RangedProjectileItem GetProjectile()
        {
            RangedProjectileItem projectile = WorldItemDatabase.Instance.GetRangedProjectileFromSerializedData(this);
            return projectile;
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
