using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XD
{
    public class PlayerInputManager : MonoBehaviour
    {
        private static PlayerInputManager instance; public static PlayerInputManager Instance { get { return instance; } }

        public PlayerManager player;

        PlayerControls playerControls;

        [Header("Player Movement Input")]
        [SerializeField] Vector2 movementInput;
        public float horizontalInput;
        public float verticalInput;
        public float moveAmount;
        
        [Header("Lock On Input")]
        [SerializeField] bool lockOn_Input = false;
        [SerializeField] bool lockOn_Left_Input = false;
        [SerializeField] bool lockOn_Right_Input = false;
        [SerializeField] float lockOn_Change_Input;
        private Coroutine lockOnCoroutine;

        [Header("Camera Movement Input")]
        [SerializeField] Vector2 cameraInput;
        public float cameraHorizontalInput;
        public float cameraVerticalInput;

        [Header("Player ACTIONS INPUT")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;
        [SerializeField] bool switchRightWeaponInput = false;
        [SerializeField] bool switchLeftWeaponInput = false;

        [Header("Bumper INPUTS")]
        [SerializeField] bool RB_Input = false;

        [Header("Trigger INPUTS")]
        [SerializeField] bool RT_Input = false; 
        [SerializeField] bool Hold_RT_Input = false;
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
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false ;
            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }
        private void Update()
        {
            HandleAllInputs();
        }
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
            {
                instance.enabled = true;

                if (playerControls != null)
                {
                    playerControls.Enable();
                }
            }
            else
            {
                instance.enabled = false;
                if (playerControls != null)
                {
                    playerControls.Disable();
                }
            }
        }
        private void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                
                // Rool & Backstep
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;

                // Sprint 
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

                // Mouse Left Click (Attack)
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;
               
                playerControls.PlayerActions.RT.performed += i => RT_Input = true;
                playerControls.PlayerActions.HoldRT.performed += i => Hold_RT_Input = true;
                playerControls.PlayerActions.HoldRT.canceled += i => Hold_RT_Input = false;
                 
                // Switch Weapons
                playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switchLeftWeaponInput = true;
                playerControls.PlayerActions.SwitchRightWeapon.performed += i => switchRightWeaponInput = true;

                // Lock On
                playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_Left_Input = true;
                playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOn_Right_Input = true;


            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;

        }

        // If we minimmize or lower the window, stop adjusting inputs
        private void OnApplicationFocus(bool focus)
        {
            if(enabled)
            {
                if(focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();   
                }
            }
        }

        private void HandleAllInputs()
        {
            HandleLockOnInput();
            HandleLockOnSwitchTargetInput(); 
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleRBInput();
            HandleRTInput();
            HandleChargeRTInput();
            HandleSwitchRightWeaponInput();
            HandleSwitchLeftWeaponInput();
        }
        #region Movements
        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));


            // Clamp The Values
            if(moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f; // Walk
            }
            if(moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1; // Run
            }

            if (player == null) return;
            // Why do i pass 0 on the horizontal? Because we only want non-strafing movement 
            // We Use the horizontal when we are strafing or locked on

            if (moveAmount != 0)
            {
                player.playerNetworkManager.isMoving.Value = true;
            }
            else
            {
                player.playerNetworkManager.isMoving.Value = false;
            }

                // Not locked on
            if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);       
            }
            else
            { 
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);
            }
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }
        #endregion

        #region ACTIONS
        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;

                //  TODO: If any UI or menu is open, return and don't dodge
                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintInput()
        {
            if(sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;

                // TODO: If we have a uý window open, simply return without doing anything

                player.playerLocomotionManager.AttemptToPerformJump();   
            }
        }

        private void HandleRBInput()
        {
            if(RB_Input)
            {
                RB_Input = false;

                // TODO: If we have a uý window open, simply return without doing anything
                player.playerNetworkManager.SetCharacterActionHand(true);

                // TODO: If we are two handing the weapon, use the two handed action
                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon); 
            }
        }

        private void HandleRTInput()
        {
            if(RT_Input)
            {
                RT_Input = false;

                // TODO: If we have a uý window open, simply return without doing anything
                player.playerNetworkManager.SetCharacterActionHand(true);

                // TODO: If we are two handing the weapon, use the two handed action
                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        private void HandleChargeRTInput()
        {
            // Check for a charge
            if(player.isPerformingAction)
            {
                if(player.playerNetworkManager.isUsingRightHand.Value)
                {
                    player.playerNetworkManager.isChargingAttack.Value = Hold_RT_Input;
                }
            }
        }
        private void HandleLockOnInput()
        {
            if(player.playerNetworkManager.isLockedOn.Value)
            {
                Debug.Log("lockOn_Input: " + lockOn_Input);
                if (player.playerCombatManager.currentTarget == null) {  return; }
                
                if(player.playerCombatManager.currentTarget.isDead.Value)
                {
                    player.playerNetworkManager.isLockedOn.Value = false;
                }

                // Attempt to find new target
                if (lockOnCoroutine != null)
                {
                    StopCoroutine(lockOnCoroutine);
                }
                lockOnCoroutine = StartCoroutine(PlayerCamera.Instance.WaitThenFindNewTarget());
            }

            if(lockOn_Input && player.playerNetworkManager.isLockedOn.Value)
            {
                lockOn_Input = false;
                PlayerCamera.Instance.ClearLockOnTargets();
                player.playerNetworkManager.isLockedOn.Value = false;
                return;
            }

            if(lockOn_Input && !player.playerNetworkManager.isLockedOn.Value)
            {
                lockOn_Input = false;

                // TODO(): IF we are aiming used ranged  weapons return ( don't lock on)

                PlayerCamera.Instance.HandleLocatingLockOnTargets();
                if(PlayerCamera.Instance.nearestLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.Instance.nearestLockOnTarget);
                    player.playerNetworkManager.isLockedOn.Value = true;
                }
            }
        }

        private void HandleLockOnSwitchTargetInput()
        {
            if (lockOn_Left_Input)
            {
                lockOn_Left_Input = false;

                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    PlayerCamera.Instance.HandleLocatingLockOnTargets();
                    if(PlayerCamera.Instance.leftLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.Instance.leftLockOnTarget);
                    }
                }
            }
            if (lockOn_Right_Input)
            {
                lockOn_Right_Input = false;
                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    PlayerCamera.Instance.HandleLocatingLockOnTargets();
                    if (PlayerCamera.Instance.rightLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.Instance.rightLockOnTarget);
                    }
                }
            }
     
        }
        private void HandleSwitchRightWeaponInput()
        {
            if(switchRightWeaponInput)
            {
                switchRightWeaponInput = false;
                player.playerEquipmentManager.SwitchRightWeapon();
            }
        }

        private void HandleSwitchLeftWeaponInput()
        {
            if (switchLeftWeaponInput)
            {
                switchLeftWeaponInput = false;
                player.playerEquipmentManager.SwitchLeftWeapon();
            }
        }
        #endregion
    }

}
 