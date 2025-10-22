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
        [SerializeField] bool dodge_Input = false;
        [SerializeField] bool sprint_Input = false;
        [SerializeField] bool jump_Input = false;
        [SerializeField] bool switchRightWeapon_Input = false;
        [SerializeField] bool switchLeftWeapon_Input = false;
        [SerializeField] bool interaction_Input = false;

        [Header("Bumper INPUTS")]
        [SerializeField] bool RB_Input = false;
        [SerializeField] bool Hold_RB_Input = false;
        [SerializeField] bool LB_Input = false;
        [SerializeField] bool Hold_LB_Input = false;

        [Header("Two Hand Inputs")]
        [SerializeField] bool two_Hand_Input = false;
        [SerializeField] bool two_Hand_Right_Weapon_Input = false;
        [SerializeField] bool two_Hand_Left_Weapon_Input = false;

        [Header("Qued Inputs")]
        [SerializeField] private bool input_Que_Is_Active = false; 
        [SerializeField] float default_Que_Input_Time = 0.35f;
        [SerializeField] float que_Input_Timer = 0f;
        [SerializeField] bool que_RB_Input = false;
        [SerializeField] bool que_RT_Input = false;

        [Header("Trigger INPUTS")]
        [SerializeField] bool RT_Input = false; 
        [SerializeField] bool Hold_RT_Input = false;
        [SerializeField] bool LT_Input = false;

        [Header("UI Inputs")]
        [SerializeField] bool openCharacterMenuInput = false;
        [SerializeField] bool closeMenuInput = false;
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
                playerControls.PlayerActions.Dodge.performed += i => dodge_Input = true;
                playerControls.PlayerActions.Jump.performed += i => jump_Input = true;

                // Sprint 
                playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;

                playerControls.PlayerActions.Interact.performed += i => interaction_Input = true;

                // Mouse Left Click (Attack)
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;
                playerControls.PlayerActions.Hold_RB.performed += i => Hold_RB_Input = true; // For Charge Attacks/Spell
                playerControls.PlayerActions.Hold_RB.canceled += i => Hold_RB_Input = false;

                // Left Alt?
                playerControls.PlayerActions.LB.performed += i => LB_Input = true;
                playerControls.PlayerActions.LB.canceled += i => player.playerNetworkManager.isBlocking.Value = false;
                playerControls.PlayerActions.LB.canceled += i => player.playerNetworkManager.isAiming.Value = false;
                playerControls.PlayerActions.Hold_LB.performed += i => Hold_LB_Input = true;
                playerControls.PlayerActions.Hold_LB.canceled += i => Hold_LB_Input = false;

                playerControls.PlayerActions.RT.performed += i => RT_Input = true;
                playerControls.PlayerActions.HoldRT.performed += i => Hold_RT_Input = true;
                playerControls.PlayerActions.HoldRT.canceled += i => Hold_RT_Input = false;
                playerControls.PlayerActions.LT.performed += i => LT_Input = true;


                // Twp Hand Inputs
                playerControls.PlayerActions.TwoHandWeapon.performed += i => two_Hand_Input = true;
                playerControls.PlayerActions.TwoHandWeapon.canceled += i => two_Hand_Input = false;

                playerControls.PlayerActions.TwoHandRightWeapon.performed += i => two_Hand_Right_Weapon_Input = true;
                playerControls.PlayerActions.TwoHandRightWeapon.canceled += i => two_Hand_Right_Weapon_Input = false;

                playerControls.PlayerActions.TwoHandLeftWeapon.performed += i => two_Hand_Left_Weapon_Input = true;
                playerControls.PlayerActions.TwoHandLeftWeapon.canceled += i => two_Hand_Left_Weapon_Input = false;


                // Switch Weapons
                playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switchLeftWeapon_Input = true;
                playerControls.PlayerActions.SwitchRightWeapon.performed += i => switchRightWeapon_Input = true;

                // Lock On
                playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_Left_Input = true;
                playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOn_Right_Input = true;

                playerControls.PlayerActions.QueRB.performed += i => QueInput(ref que_RB_Input);
                playerControls.PlayerActions.QueRT.performed += i => QueInput(ref que_RT_Input);

                // UI Inputs
                playerControls.PlayerActions.Dodge.performed += i => closeMenuInput = true;
                playerControls.PlayerActions.OpenCharacterMenu.performed += i => openCharacterMenuInput = true;


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
            HandleTwoHandInput();
            HandleLockOnInput();
            HandleLockOnSwitchTargetInput(); 
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleRBInput();
            HandleHoldRBInput();
            HandleLBInput();
            HandleHoldLBInput();
            HandleRTInput();
            HandleChargeRTInput();
            HandleLTInput();
            HandleSwitchRightWeaponInput();
            HandleSwitchLeftWeaponInput();
            HandleQuedInputs();
            HandleInteractionInput();
            HandleCloseUIInputs();
            HandleOpenCharacterMenuInput();
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

            if(!player.playerLocomotionManager.canRun)
            {
                if(moveAmount > 0.5f)
                {
                    moveAmount = 0.5f;
                }
                if(verticalInput > 0.5f)
                {
                    verticalInput = 0.5f;
                }
                if(horizontalInput > 0.5f)
                {
                    horizontalInput = 0.5f;
                }
            }
                
            if (player.playerNetworkManager.isLockedOn.Value && !player.playerNetworkManager.isSprinting.Value)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);
                return;
            }
            if(player.playerNetworkManager.isAiming.Value)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);
                return;
            }
            
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);       
            
            
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
            if (dodge_Input)
            {
                dodge_Input = false;

                //  TODO: If any UI or menu is open, return and don't dodge
                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintInput()
        {
            if(sprint_Input)
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
            if (jump_Input)
            {
                jump_Input = false;
                
                if(PlayerUIManager.Instance.menuWindowIsOpen) { return; }


                // TODO: If we have a uý window open, simply return without doing anything

                player.playerLocomotionManager.AttemptToPerformJump();   
            }
        }

        private void HandleRBInput()
        {
            if(two_Hand_Input) { return; }
            if(RB_Input)
            {
                RB_Input = false;

                // TODO: If we have a uý window open, simply return without doing anything
                player.playerNetworkManager.SetCharacterActionHand(true);

                // TODO: If we are two handing the weapon, use the two handed action
                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon); 
            }
        }

        private void HandleHoldRBInput()
        {
            if(Hold_RB_Input)
            {
                player.playerNetworkManager.isChargingRightSpell.Value = true;
                player.playerNetworkManager.isHoldingArrow.Value = true;
            }
            else
            {
                player.playerNetworkManager.isChargingRightSpell.Value = false;
                player.playerNetworkManager.isHoldingArrow.Value = false;
            }
        }
        private void HandleLBInput()
        {
            if (two_Hand_Input) { return; }

            if (LB_Input)
            {
                LB_Input = false;

                // TODO: If we have a uý window open, simply return without doing anything
                player.playerNetworkManager.SetCharacterActionHand(false);

                if(player.playerNetworkManager.isUsingRightHand.Value)
                {
                    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_LB_Action, player.playerInventoryManager.currentRightHandWeapon);

                }
                else
                {
                    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentLeftHandWeapon.oh_LB_Action, player.playerInventoryManager.currentLeftHandWeapon);
                }
            }
        }

        private void HandleHoldLBInput()
        {
            if(Hold_LB_Input)
            {
                player.playerNetworkManager.isChargingLeftSpell.Value = true;
            }
            else
            {
                player.playerNetworkManager.isChargingLeftSpell.Value = false;
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

        private void HandleLTInput()
        {
            if (LT_Input)
            {
                LT_Input = false;
                
                WeaponItem weaponPerformingAshOfWar = player.playerCombatManager.SelectWeaponToPerformAshOfWar();
                weaponPerformingAshOfWar.ashOfWarAction.AttemptToPerformAction(player);
            }
        }

        private void HandleTwoHandInput()
        {
            if(!two_Hand_Input) { return; }

            if(two_Hand_Right_Weapon_Input)
            {
                RB_Input = false;
                two_Hand_Right_Weapon_Input = false;
                player.playerNetworkManager.isBlocking.Value = false;

                if(player.playerNetworkManager.IsTwoHandingWeapon.Value)
                {
                    // If we are two handing a weapon already, change the is twohaning weapon bool to false whic triggers an " OnValueChanged" func
                    // which UnTwohands current weapon
                    player.playerNetworkManager.IsTwoHandingWeapon.Value = false;
                    return;
                }
                else
                {
                    // If we are not already two handing, change the right two hand bool to true which triggers an "OnValueChanged" func
                    // This function two hands the right weapon
                    player.playerNetworkManager.IsTwoHandingRightWeapon.Value = true;
                    return;
                }
            }

            if (two_Hand_Left_Weapon_Input)
            {
                LB_Input = false;
                two_Hand_Left_Weapon_Input = false;
                player.playerNetworkManager.isBlocking.Value = false;

                if (player.playerNetworkManager.IsTwoHandingWeapon.Value)
                {
                    // If we are two handing a weapon already, change the is twohaning weapon bool to false whic triggers an " OnValueChanged" func
                    // which UnTwohands current weapon
                    player.playerNetworkManager.IsTwoHandingWeapon.Value = false;
                    return;
                }
                else
                {
                    // If we are not already two handing, change the left two hand bool to true which triggers an "OnValueChanged" func
                    // This function two hands the left weapon
                    player.playerNetworkManager.IsTwoHandingLeftWeapon.Value = true;
                    return;
                }
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
            if(switchRightWeapon_Input)
            {
                switchRightWeapon_Input = false;

                if (PlayerUIManager.Instance.menuWindowIsOpen) { return; }
                player.playerEquipmentManager.SwitchRightWeapon();
            }
        }

        private void HandleSwitchLeftWeaponInput()
        {
            if (switchLeftWeapon_Input)
            {
                switchLeftWeapon_Input = false;
                if (PlayerUIManager.Instance.menuWindowIsOpen) { return; }   
                player.playerEquipmentManager.SwitchLeftWeapon();
            }
        }

        private void HandleInteractionInput()
        {
            if(interaction_Input)
            {
                interaction_Input = false;

                player.playerInteractionManager.Interact();                
            }
        }
        #endregion

        private void QueInput(ref bool quedInput) // Passing a reference means we pass a specific bool, and not the value of that bool (True or False)
        {
            que_RB_Input = false;
            que_RT_Input = false;
            //que_LB_Input = false;
            //que_LT_Input = false;

            // TODO: Check for uý window being open, if its open return

            if(player.isPerformingAction || player.playerNetworkManager.isJumping.Value)
            {
                quedInput = true;
                que_Input_Timer = default_Que_Input_Time;
                input_Que_Is_Active = true;
            }
        }

        private void ProcessQuedInput()
        {
            if(player.isDead.Value) { return; }

            if (que_RB_Input) { RB_Input = true; }
            if (que_RT_Input) { RT_Input = true; }

        }

        private void HandleQuedInputs()
        {
            if(input_Que_Is_Active)
            {
                if (que_Input_Timer > 0f)
                {
                    que_Input_Timer -= Time.deltaTime;
                    ProcessQuedInput();
                }
                else
                {
                    // Reset All Qued Inputs
                    que_RB_Input = false;
                    que_RT_Input = false;

                    input_Que_Is_Active = false;
                    que_Input_Timer = 0;
                }
            }
        }

        private void HandleOpenCharacterMenuInput()
        {
            if(openCharacterMenuInput)
            {
                openCharacterMenuInput = false;

                PlayerUIManager.Instance.playerUIPopUpManager.CloseAllPopUpWindows();
                PlayerUIManager.Instance.CloseAllMenuWindows();
                PlayerUIManager.Instance.playerUICharacterMenuManager.OpenCharacterMenu();
            }
        }
        private void HandleCloseUIInputs()
        {
            if(closeMenuInput)
            {
                closeMenuInput = false;
              
                if (PlayerUIManager.Instance.menuWindowIsOpen)
                {
                    PlayerUIManager.Instance.CloseAllMenuWindows();
                }
            }
        }
    }

}
 