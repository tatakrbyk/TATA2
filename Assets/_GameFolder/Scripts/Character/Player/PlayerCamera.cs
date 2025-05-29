using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerCamera : MonoBehaviour
    {
        private static PlayerCamera instance; public static PlayerCamera Instance { get { return instance; } }

        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] private Transform cameraPivotTransform;

        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1;
        [SerializeField] private float leftAndRightRotationSpeed = 220;
        [SerializeField] private float upAndDownRotationSpeed = 220;
        [SerializeField] private float minimumPivot = -30; // The lowest point you are able to look down
        [SerializeField] private float maximumPivot = 60;  // The highest point you are able to look up
        [SerializeField] private float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;


        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; // Used for camera collisions (Moves the camera object to this position upon colliding)
        [SerializeField] private float leftAndRightLookAngle;
        [SerializeField] private float upAndDownLookAngle;
        private float cameraZPosition; // For camera collisions , back and forth
        private float targetCameraZPozition; // For camera collisions


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        // Call PlayerManager LateUpdate()
        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                // Follow the player
                HandleFollowTarget();

                // Rotate around the player
                HandleRotation();

                // Collide with objects
                HandleCollisions();

            }
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotation()
        {
            // TODO(t): If locked on, Force rotation towards targets
            // Else Rotate Regularly


            // Rotate left and right based on horizontal movement on the right joystick
            leftAndRightLookAngle += (PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;

            // Rotate up and down based on vertical movement on the right joystick
            upAndDownLookAngle -= (PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;
            // Rotate this gameobject left and right 
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            // Rotate the pivot gameobject Up and Down
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
            
        }
        
        private void HandleCollisions()
        {
            targetCameraZPozition = cameraZPosition;
            
            RaycastHit hit;
            // Chech Collision
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            // Draw Sphere and check is there any object
            if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPozition), collideWithLayers))
            {   // If there is, do it
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPozition = -(distanceFromHitObject - cameraCollisionRadius);
            }
            
            // If our target position is less 
            if(Mathf.Abs(targetCameraZPozition) < cameraCollisionRadius)
            {
                targetCameraZPozition = -cameraCollisionRadius;
            }

            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPozition, cameraCollisionRadius);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }
    }

}
