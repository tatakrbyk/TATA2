using System.Collections;
using System.Collections.Generic;
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

        [Header("Camera Movement Input")]
        [SerializeField] Vector2 cameraInput;
        public float cameraHorizontalInput;
        public float cameraVerticalInput;



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
        }
        private void Update()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
        }
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            else
            {
                instance.enabled = false;
            }
        }
        private void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
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

            // Npt locked on
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }
    }

}
