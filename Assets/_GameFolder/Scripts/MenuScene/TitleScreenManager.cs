using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace XD
{
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNetworkAsHost() // Oress Start
        {
            NetworkManager.Singleton.StartHost();   
        }

        public void StartNewGame()
        {
            StartCoroutine(WorldSaveGameManager.Instance.LoadNewGame());
        }
    }

}
