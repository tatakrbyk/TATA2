using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerBodyManager : MonoBehaviour
    {
        PlayerManager player;

        [Header("Hair")]
        [SerializeField] public GameObject hair;
        [SerializeField] public GameObject facialHair;

        [Header("Male")]
        [SerializeField] public GameObject maleObject;      // The Master Male GameObject Parent
        [SerializeField] public GameObject maleHead;        // Default Head Model When UnEquipping Armor
        [SerializeField] public GameObject[] maleBody;      // Default UpperBody Models When UnEquipping Armor (Chest, Upper Right Arm, Upper Left Arm)
        [SerializeField] public GameObject[] maleArms;      // Default UpperBody Models When UnEquipping Armor (Lower Right Arm, Right Hand, Lower Left Arm, Left Hand)
        [SerializeField] public GameObject[] maleLegs;      // Default UpperBody Models When UnEquipping Armor (Hips, Right Leg,  Left Leg)
        [SerializeField] public GameObject maleEyebrows;    // Facial Feature
        [SerializeField] public GameObject maleFacialHair;  // Facial Feature


        [Header("Female")]
        [SerializeField] public GameObject femaleObject;    // The Master Female GameObject Parent
        [SerializeField] public GameObject femaleHead;      // Default Head Model When UnEquipping Armor
        [SerializeField] public GameObject[] femaleBody;    // Default UpperBody Models When UnEquipping Armor (Chest, Upper Right Arm, Upper Left Arm)
        [SerializeField] public GameObject[] femaleArms;    // Default UpperBody Models When UnEquipping Armor (Lower Right Arm, Right Hand, Lower Left Arm, Left Hand)
        [SerializeField] public GameObject[] femaleLegs;    // Default UpperBody Models When UnEquipping Armor (Hips, Right Leg,  Left Leg)
        [SerializeField] public GameObject femaleEyebrows;  // Facial Feature
        [SerializeField] public GameObject femaleFacialHair; // Facial Feature

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }
        #region Enable/Disable Body Features
        public void EnableHead()
        {
            //Enable Head Object
            maleHead.SetActive(true);
            femaleHead.SetActive(true);

            //Enable Any Facial Object
            maleEyebrows.SetActive(true);
            femaleEyebrows.SetActive(true);

        }
        public void DisableHead()
        {
            // Disable Head Object
            maleHead.SetActive(false);
            femaleHead.SetActive(false);

            // Disable Any Facial Object
            maleEyebrows .SetActive(false);
            femaleEyebrows.SetActive(false);

        }
        public void EnableHair()
        {
            hair.SetActive(true);
        }
        public void DisableHair()
        {
            hair.SetActive(false);
        }

        public void EnableFacialHair()
        {
            facialHair.SetActive(true);
        }
        public void DisableFacialHair()
        {
            facialHair.SetActive(false);
        }

        public void EnableBody()
        {
            foreach (var model in maleBody)
            {
                model.SetActive(true);
            }
            foreach (var model in femaleBody)
            {
                model.SetActive(true);
            }
        }
        public void DisableBody()
        {
            foreach (var model in maleBody)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleBody)
            {
                model.SetActive(false);
            }
        }

        public void EnableLowerBody()
        {
            foreach (var model in maleLegs)
            {
                model.SetActive(true);
            }
            foreach (var model in femaleLegs)
            {
                model.SetActive(true);
            }
            
        }
        public void DisableLowerBody()
        {
            foreach (var model in maleLegs)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleLegs)
            {
                model.SetActive(false);
            }
        }

        public void EnableArms()
        {
            foreach (var model in maleArms)
            {
                model.SetActive(true);
            }
            foreach (var model in femaleArms)
            {
                model.SetActive(true);
            }
        }
        public void DisableArms()
        {
            foreach (var model in maleArms)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleArms)
            {
                model.SetActive(false);
            }
        }
        #endregion

        public void ToggleBodyType(bool isMale)
        {
            if (isMale)
            {
                maleObject.SetActive(true);
                femaleObject.SetActive(false);
            }
            else
            {
                maleObject.SetActive(false);
                femaleObject.SetActive(true);
            }

            player.playerEquipmentManager.EquipArmors();
        }
    }

}
