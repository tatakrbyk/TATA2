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

        public void LoadWeapon(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;

            
        }
    }

}
