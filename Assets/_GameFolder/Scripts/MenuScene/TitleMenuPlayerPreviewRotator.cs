using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class TitleMenuPlayerPreviewRotator : MonoBehaviour
    {
        PlayerControls playerControls;

        [Header("Camera Input")]
        [SerializeField] private Vector2 cameraInput;
        [SerializeField] private float lookAngle;

        [Header("Rotation")]
        [SerializeField] private float horizontalInput;
        [SerializeField] private float rotationSpeed = 150f;


        private void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            }
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void Update()
        {
            // TODO: Move different place, uptade is for DEBUG
            horizontalInput = cameraInput.x;
            lookAngle += (horizontalInput * rotationSpeed) * Time.deltaTime;
            Vector3 caeeraRotation = Vector3.zero;
            caeeraRotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(caeeraRotation);
            transform.rotation = targetRotation;
        }
    }
}
