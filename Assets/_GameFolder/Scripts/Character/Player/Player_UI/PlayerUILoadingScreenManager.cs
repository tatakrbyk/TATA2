using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XD
{
    public class PlayerUILoadingScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private CanvasGroup canvasGroup;
        private Coroutine fadeLoadingScreenCoroutine;


        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnSceneChanged(Scene arg0, Scene arg1)
        {
            DeActivateLoadingScreen();
        }

        public void ActivateLoadingScreen()
        {
            // If The loading screen is already active, do nothing
            if(loadingScreen.activeSelf) return;

            // TODO: If the loading screen is in the process of deactivating, cancel it


            canvasGroup.alpha = 1;
            loadingScreen.SetActive(true);
        }

        public void DeActivateLoadingScreen(float delay = 1)
        {
            // If The loading screen is already inactive, do nothing
            if (!loadingScreen.activeSelf) return;

            // If we are already fading away the loading screen return
            if (fadeLoadingScreenCoroutine != null) return;

            // The Duration is how long the fade will take, the delat is the wait in seconds before the fade begins
            fadeLoadingScreenCoroutine = StartCoroutine(FadeLoadingScreen(1f, delay));

        }   

        private IEnumerator FadeLoadingScreen(float duration, float delay)
        {
            while (WorldAIManager.Instance.isPerformingLoadingOperation)
            {
                yield return null;
            }
            loadingScreen.SetActive(true);

            if(duration > 0)
            {
                while(delay > 0)
                {
                    delay -= Time.deltaTime;
                    yield return null;
                }

                canvasGroup.alpha = 1;
                float elapsedTime = 0;

                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
                    yield return null;
                }

                canvasGroup.alpha = 0;
                loadingScreen.SetActive(false);
                fadeLoadingScreenCoroutine = null;
                yield return null;
            }
        }
    }
}
