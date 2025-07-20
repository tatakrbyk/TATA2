using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace XD
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("Message Pop Up")]
        [SerializeField] TextMeshProUGUI popUpMessageText;
        [SerializeField] GameObject popUpMessageGameObject;

        [Header("DIED POP UP")]
        [SerializeField] GameObject diedPopUpGameObject;
        [SerializeField] TextMeshProUGUI diedPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI diedPopUpText;
        [SerializeField] CanvasGroup diedPopUpCanvasGroup;

        [Header("Boss Defeated POP UP")]
        [SerializeField] GameObject bossDefeatedPopUpGameObject;
        [SerializeField] TextMeshProUGUI bossDefeatedPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI bossDefeatedPopUpText;
        [SerializeField] CanvasGroup bossDefeatedPopUpCanvasGroup;

        [Header("Grace Restred POP UP")]
        [SerializeField] GameObject graceRestoredPopUpGameObject;
        [SerializeField] TextMeshProUGUI graceRestoredPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI graceRestoredPopUpText;
        [SerializeField] CanvasGroup graceRestoredPopUpCanvasGroup;

        public void CloseAllPopUpWindows()
        {
            popUpMessageGameObject.SetActive(false);
            PlayerUIManager.Instance.popUpWindowIsOpen = false;
        }

        public void SendPlayerMessagePopUp(string messageText)
        {
            PlayerUIManager.Instance.popUpWindowIsOpen = true;
            popUpMessageText.text = messageText;
            popUpMessageGameObject.SetActive(true); 
        }
        public void SendYouDiedPopUp()
        {
            // Activate post processing effects 

            diedPopUpGameObject.SetActive(true);
            diedPopUpBackgroundText.characterSpacing = 0;

            // Pop Up Aniamtion
            StartCoroutine(StretchPopUpTextOverTime(diedPopUpBackgroundText, 8, 19f));
            StartCoroutine(FadeInPopUpOverTime(diedPopUpCanvasGroup, 5));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(diedPopUpCanvasGroup, 2, 5));

        }

        public void SendBossDefeatedPopUp(string bossDefeatedMessage)
        {
            bossDefeatedPopUpText.text = bossDefeatedMessage;
            bossDefeatedPopUpBackgroundText.text = bossDefeatedMessage;

            bossDefeatedPopUpGameObject.SetActive(true);
            bossDefeatedPopUpBackgroundText.characterSpacing = 0;

            // Pop Up Aniamtion
            StartCoroutine(StretchPopUpTextOverTime(bossDefeatedPopUpBackgroundText, 8, 19f));
            StartCoroutine(FadeInPopUpOverTime(bossDefeatedPopUpCanvasGroup, 5));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(bossDefeatedPopUpCanvasGroup, 2, 5));

        }

        public void SendGraceRestoredPopUp(string graceRestoredMessage)
        {
            graceRestoredPopUpText.text = graceRestoredMessage;
            graceRestoredPopUpBackgroundText.text = graceRestoredMessage;

            graceRestoredPopUpGameObject.SetActive(true);
            graceRestoredPopUpBackgroundText.characterSpacing = 0;

            // Pop Up Aniamtion
            StartCoroutine(StretchPopUpTextOverTime(graceRestoredPopUpBackgroundText, 8, 19f));
            StartCoroutine(FadeInPopUpOverTime(graceRestoredPopUpCanvasGroup, 5));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(graceRestoredPopUpCanvasGroup, 2, 5));

        }
        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if(duration > 0f)
            {
                text.characterSpacing = 0;
                float timer = 0;

                yield return null;

                while(timer < duration)
                {
                    timer += Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                    yield return null;
                    
                }
            }
        }

        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvasGroup, float duration)
        {
            if (duration > 0f)
            {
                canvasGroup.alpha = 0f;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, duration * Time.deltaTime);
                    yield return null;
                }
            }
        }

        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvasGroup, float duration, float delay)
        {
            if (duration > 0f)
            {
                while (delay > 0f)
                {
                    delay -= Time.deltaTime;
                    yield return null;
                }

                canvasGroup.alpha = 1f;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvasGroup.alpha = 0f;
            yield return null;
        }
    }

}
