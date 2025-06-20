using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace XD
{
    public class PlayerCamera : MonoBehaviour
    {
        private static PlayerCamera instance; public static PlayerCamera Instance { get { return instance; } }

        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] private Transform cameraPivotTransform;


        [Header("Locked On")]
        private float lockOnRadius = 20;
        private float minimumViewableAngle = -50;
        private float maximumViewableAngle = 50;
        private float lockOnTargetFollowSpeed = 1;
        private float setCameraHeightSpeed = 0.05f;
        private float unlockedCameraHeight = 1.65f;
        private float lockedCameraHeight = 2.0f;
        private Coroutine cameraLockOnHeightCoroutine;
        private List<CharacterManager> availableTargets = new List<CharacterManager>();        
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockOnTarget;
        public CharacterManager rightLockOnTarget;

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
            // If locked on, Force rotation towards targets
            if(player.playerNetworkManager.isLockedOn.Value)
            {
                // This rotates tigis gameobject
                Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
                rotationDirection.Normalize();
                rotationDirection.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

                // This rotates the pivot gameobject
                rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
                rotationDirection.Normalize();

                targetRotation = Quaternion.LookRotation(rotationDirection);
                cameraPivotTransform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

                // Save our rotations to our look angles, so when we unlock it doesnt snap too far away
                leftAndRightLookAngle = transform.eulerAngles.y;
                upAndDownLookAngle = transform.eulerAngles.x;
            }
            // Else Rotate Regularly
            else
            {
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
        public void HandleLocatingLockOnTargets()
        {
            float shortestDistance = Mathf.Infinity;  // target closest to us
            float shortestDistanceOfRightTarget = Mathf.Infinity; // +x axis
            float shortestDistanceOfLeftTarget  = -Mathf.Infinity; // -x axis

            Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.Instance.GetCharacterLayer());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

                if (lockOnTarget != null)
                {
                    Vector3 lockOnTargetsDirection = lockOnTarget.transform.position - player.transform.position;
                    float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetsDirection, cameraObject.transform.forward);

                    if (lockOnTarget.isDead.Value) continue;
                    if (lockOnTarget.transform.root == player.transform.root) continue; // Ignore self

                    if(viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                        RaycastHit hit;

                        if(Physics.Linecast(player.playerCombatManager.lockOnTransform.position,
                            lockOnTarget.characterCombatManager.lockOnTransform.position,
                            out hit, WorldUtilityManager.Instance.GetEnvironmentLayers()))
                        {
                            continue;
                        }
                        else
                        {
                            availableTargets.Add(lockOnTarget);
                        }
                    }
                }
            }

            for (int k = 0; k < availableTargets.Count; k++)
            {
                if (availableTargets != null)
                {
                    float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);

                    if(distanceFromTarget < shortestDistance)
                    {
                        shortestDistance = distanceFromTarget;
                        nearestLockOnTarget = availableTargets[k];
                    }
                    if(player.playerNetworkManager.isLockedOn.Value)
                    {
                        Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(availableTargets[k].transform.position);

                        var distanceFromLeftTarget = relativeEnemyPosition.x;
                        var distanceFromRightTarget = relativeEnemyPosition.x;

                        if(availableTargets[k] != player.playerCombatManager.currentTarget) { continue; }

                        if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget)
                        {
                            shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                            leftLockOnTarget = availableTargets[k];
                        }
                        else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                        {
                            shortestDistanceOfRightTarget = distanceFromRightTarget;
                            rightLockOnTarget = availableTargets[k];

                        }

                    }
                }
                else
                {
                    ClearLockOnTargets();
                    player.playerNetworkManager.isLockedOn.Value = false;
                }                
            }
        }
        public void SetLockCameraHeight()
        {
            if(cameraLockOnHeightCoroutine != null)
            {
                StopCoroutine(cameraLockOnHeightCoroutine);
            }
            cameraLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
        }
        public void ClearLockOnTargets()
        {
            nearestLockOnTarget = null;
            leftLockOnTarget = null;
            rightLockOnTarget = null;
            availableTargets.Clear();
        }
        public IEnumerator WaitThenFindNewTarget()
        {
            while (player.isPerformingAction)
            {
                yield return null;
            }

            ClearLockOnTargets();
            HandleLocatingLockOnTargets();

            if (nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(nearestLockOnTarget);
                player.playerNetworkManager.isLockedOn.Value = true;
            }
            yield return null;
        }
        private IEnumerator SetCameraHeight()
        {
            float duration = 1;
            float timer = 0;

            Vector3 velocity = Vector3.zero;
            Vector3 newLockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, lockedCameraHeight);
            Vector3 newUnlockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, unlockedCameraHeight);

            while(timer < duration)
            {
                timer += Time.deltaTime;

                if (player != null)
                {
                    if (player.playerCombatManager.currentTarget != null)
                    {
                        cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedCameraHeight, ref velocity, setCameraHeightSpeed);
                        cameraPivotTransform.transform.localRotation = Quaternion.Slerp(cameraPivotTransform.transform.localRotation, Quaternion.Euler(0, 0, 0), setCameraHeightSpeed);
                    }
                    else
                    {
                        cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedCameraHeight, ref velocity, setCameraHeightSpeed);
                    }
                }
                yield return null;
            }
            
            if(player != null)
            {
                if (player.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTransform.transform.localPosition = newLockedCameraHeight;
                    cameraPivotTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    cameraPivotTransform.transform.localPosition = newUnlockedCameraHeight;
                }
            }
            yield return null;
        }
    }
}
