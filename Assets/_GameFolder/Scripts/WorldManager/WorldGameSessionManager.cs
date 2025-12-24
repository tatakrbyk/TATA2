using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XD
{
    public class WorldGameSessionManager : MonoBehaviour
    {
        private static WorldGameSessionManager instance; public static WorldGameSessionManager Instance { get { return instance; } }

        [Header("Active Players In Session")]
        public List<PlayerManager> activePlayers = new List<PlayerManager>();

        private Coroutine revivalCoroutine;
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
        }

        private void OnDestroy()
        {
            
        }

        private void OnApplicationQuit()
        {
            
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

        }

        private void OnSceneLoaded(Scene newScene, LoadSceneMode loadMode)
        {

        }
        // Face Punch
        public void ToggleLobbyIsJoinable(bool status)
        {

        }

        // Called when a lobby is created
        private void OnLobbyCreated(/*Result result, Lobby lobby*/)
        {
        }
        // Called when you enter a lobby
        private void OnGameLobbyJoinRequested(/*Lobby lobby, SteamId steamID*/)
        {

        }
        private void OnLobbyEntered(/*Lobby lobby*/)
        {
        }

        public async void StartGameAsHost()
        {

        }

        public void StartGameAsClient(/*SteamId ID*/)
        {

        }

        private IEnumerator AttemptToJoinAsClient(/*SteamId ID*/)
        {
            yield return null;
        }
        public void DisconnectFromLobby()
        {

        }

        public void WaitThenReviveHost()
        {
            if (revivalCoroutine != null)
            {
                StopCoroutine(revivalCoroutine);
            }

            revivalCoroutine = StartCoroutine(ReviveHostCoroutine(5f));
        }

        private IEnumerator ReviveHostCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            PlayerUIManager.Instance.playerUILoadingScreenManager.ActivateLoadingScreen();
            PlayerUIManager.Instance.localPlayer.ReviveCharacter();

            for (int i = 0; i < WorldObjectManager.Instance.sitesOfGrace.Count; i++)
            {
                if (WorldObjectManager.Instance.sitesOfGrace[i].siteOfGraceID == WorldSaveGameManager.Instance.currentCharacterData.lastSiteOfGraveRestedAt)
                {
                    WorldObjectManager.Instance.sitesOfGrace[i].TeleportToSiteOfGrace();
                    break;
                }
            }
        }
        public void AddPlayerToActivePlayersList(PlayerManager player)
        {

            if(!activePlayers.Contains(player))
            {
                activePlayers.Add(player);
            }
            RemoveNullPlayer();

        }

        public void RemovePlayerFromActivePlayersList(PlayerManager player)
        {
            if(!activePlayers.Contains(player))
            {
               activePlayers.Remove(player);
            }
            RemoveNullPlayer();
        }

        private void RemoveNullPlayer()
        {
            for (int i = activePlayers.Count - 1; i > -1; i--)
            {
                if (activePlayers[i] == null)
                {
                    activePlayers.RemoveAt(i);
                }
            }
        }

        
    }

}
