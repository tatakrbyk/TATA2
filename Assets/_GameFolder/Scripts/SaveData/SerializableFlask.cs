using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [System.Serializable]
    public class SerializableFlask : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;
        //[SerializeField] public int maxFlaskCharges;
        //[SerializeField] public int flaskHealAmount;


        public FlaskItem GetFlask()
        {
            FlaskItem flask = WorldItemDatabase.Instance.GetFlaskFromSerializedData(this);
            return flask;
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
