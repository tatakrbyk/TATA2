using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class Item : ScriptableObject
    {
        [Header("Item Info")]
        public int itemID;
        public string itemName;
        public Sprite itemIcon;
        [TextArea] public string itemDescription;

    }
}
