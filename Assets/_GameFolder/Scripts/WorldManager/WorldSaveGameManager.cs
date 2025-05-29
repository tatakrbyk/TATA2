using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XD
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        private static WorldSaveGameManager instance;  public static WorldSaveGameManager Instance { get { return instance; } }

        [SerializeField] private int WorldSceneIndex = 1;
        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(WorldSceneIndex);
            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return WorldSceneIndex;
        }
    }
}
