using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class WorldGameSessionManager : MonoBehaviour
    {
        private static WorldGameSessionManager instance; public static WorldGameSessionManager Instance { get { return instance; } }

        [Header("Active Players In Session")]
        public List<PlayerManager> activePlayers = new List<PlayerManager>();
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
