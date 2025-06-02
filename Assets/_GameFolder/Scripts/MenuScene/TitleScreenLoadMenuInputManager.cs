using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XD;

public class TitleScreenLoadMenuInputManager : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Title Screem Inputs")]
    [SerializeField] bool deleteCharacterSlot = false;

    void Update()
    {
        if(deleteCharacterSlot)
        {
            deleteCharacterSlot = false;
            TitleScreenManager.Instance.AttemptToDeleteCharacterSlot();
        }
    }

    private void OnEnable()
    {
        if (playerControls != null)
        {
            playerControls = new PlayerControls();
            playerControls.UI.X.performed += i => deleteCharacterSlot = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
