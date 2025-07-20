using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// NOTE(Taha): Using HandR and HandL
namespace XD
{
    public class WeaponModelInstantiationSlot : MonoBehaviour
    {
        
        public WeaponModelSlot weaponSlot;
        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void PlaceWeaponModelIntoSlot(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }

        public void PlaceWeaponModelInUnequippedSlot(GameObject weaponModel, WeaponClass weaponClass, PlayerManager player)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            switch(weaponClass)
            {
                case WeaponClass.StraightSword:
                    weaponModel.transform.localPosition = new Vector3(-0.005759669f, -0.05151821f, -0.1916096f);
                    weaponModel.transform.localRotation = Quaternion.Euler(12.409f, -91.219f, -0.275f);
                    break;
                case WeaponClass.Spear:
                    weaponModel.transform.localPosition = new Vector3(-0.005759669f, -0.05151821f, -0.1916096f);
                    weaponModel.transform.localRotation = Quaternion.Euler(12.409f, -91.219f, -0.275f);
                    break;
                case WeaponClass.MediumShield:
                    weaponModel.transform.localPosition = new Vector3(0.228f, 0.035f, 0.155f);
                    weaponModel.transform.localRotation = Quaternion.Euler(10.146f, 68.984f, -83.77f  );
                    break;
                default:
                    break;
            }
        }
    }

}
