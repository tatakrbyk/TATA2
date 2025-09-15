using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerUIEquipmentManagerInputManager : MonoBehaviour
    {
        PlayerControls playerControls;

        PlayerUIEquipmentManager playerUIEquipmentManager;

        [Header("Input")]
        [SerializeField] private bool unequipItemInput;

        private void Awake()
        {
            playerUIEquipmentManager = GetComponentInParent<PlayerUIEquipmentManager>();
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                // Unequip Item
                playerControls.PlayerActions.X.performed += i => unequipItemInput = true;
            }
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void Update()
        {
            
        }

        private void HandlePlayerUIEquipmentManagerInputs()
        {
            if(unequipItemInput)
            {
                unequipItemInput = false;
                playerUIEquipmentManager.UnEquipSelectedItem();
            }
        }
    }

}
