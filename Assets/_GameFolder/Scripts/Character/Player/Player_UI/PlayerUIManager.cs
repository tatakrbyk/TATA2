using Unity.Netcode;
using UnityEngine;

namespace XD
{
    public class PlayerUIManager : MonoBehaviour
    {
        private static PlayerUIManager instance; public static PlayerUIManager Instance { get { return instance; } }

        [Header("Network Join")]
        [SerializeField] bool startGameAsClient;

        [HideInInspector] public PlayerManager localPlayer;
        [HideInInspector] public PlayerUIHUDManager playerUIHUDManager;
        [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;
        [HideInInspector] public PlayerUICharacterMenuManager playerUICharacterMenuManager;
        [HideInInspector] public PlayerUIEquipmentManager playerUIEquipmentManager;
        [HideInInspector] public PlayerUISiteOfGraceManager playerUISiteOfGraceManager;
        [HideInInspector] public PlayerUITeleportLocationManager playerUITeleportLocationManager;
        [HideInInspector] public PlayerUILoadingScreenManager playerUILoadingScreenManager;
        [HideInInspector] public PlayerUILevelUpManager playerUILevelUpManager;

        [Header("UI Flags")]
        public bool menuWindowIsOpen = false;  // Inventory screen, Equipment menu, Blacksmith menu, etc.
        public bool popUpWindowIsOpen = false; // Item Pick up, Dialog pop up, etc.

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

            playerUIHUDManager = GetComponentInChildren<PlayerUIHUDManager>();
            playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
            playerUICharacterMenuManager = GetComponentInChildren<PlayerUICharacterMenuManager>();
            playerUIEquipmentManager = GetComponentInChildren<PlayerUIEquipmentManager>();
            playerUISiteOfGraceManager = GetComponentInChildren<PlayerUISiteOfGraceManager>();
            playerUITeleportLocationManager = GetComponentInChildren<PlayerUITeleportLocationManager>();
            playerUILoadingScreenManager = GetComponentInChildren<PlayerUILoadingScreenManager>();
            playerUILevelUpManager = GetComponentInChildren<PlayerUILevelUpManager>();

        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;
                // we must first shutdown, because we have started as a host during the title screen
                NetworkManager.Singleton.Shutdown();
                // We then restart, as a client
                NetworkManager.Singleton.StartClient();
            }
        }

        public void CloseAllMenuWindows()
        { 
            playerUICharacterMenuManager.CloseMenuAfterFixedFrame();
            playerUIEquipmentManager.CloseMenuAfterFixedFrame();
            playerUISiteOfGraceManager.CloseMenuAfterFixedFrame();
            playerUITeleportLocationManager.CloseMenuAfterFixedFrame();
            playerUILevelUpManager.CloseMenuAfterFixedFrame();

        }
    }
}
