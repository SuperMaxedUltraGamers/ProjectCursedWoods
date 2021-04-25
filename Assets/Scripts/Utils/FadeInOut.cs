using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CursedWoods.Utils
{
    public class FadeInOut : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        private float fadeSpeed = 0.005f;
        private float currentFadeSpeed;
        private float currentAlpha = 1f;
        private FadeType currentFadeType;

        private void Awake()
        {
            image.enabled = true;
        }

        public void StartFade(FadeType fadeType)
        {
            currentFadeType = fadeType;
            switch (currentFadeType)
            {
                case FadeType.FadeIn:
                    currentAlpha = 2.2f;
                    currentFadeSpeed = 0f;
                    break;
                case FadeType.FadeOut:
                    currentAlpha = 0f;
                    currentFadeSpeed = 0.05f;
                    break;
            }

            image.enabled = true;
            StartCoroutine(Fading());
        }

        public void StartFade(FadeType fadeType, Level levelToLoadAfterFade)
        {
            currentFadeType = fadeType;
            switch (currentFadeType)
            {
                case FadeType.FadeIn:
                    currentAlpha = 2.2f;
                    currentFadeSpeed = 0f;
                    break;
                case FadeType.FadeOut:
                    currentAlpha = 0f;
                    currentFadeSpeed = 0.05f;
                    break;
            }

            image.enabled = true;
            StartCoroutine(Fading(levelToLoadAfterFade));
        }

        private IEnumerator Fading()
        {
            switch (currentFadeType)
            {
                case FadeType.FadeIn:
                    while (currentAlpha > 0f)
                    {
                        FadeIn();
                        yield return null;
                    }

                    break;
                case FadeType.FadeOut:
                    while (currentAlpha < 1f)
                    {
                        FadeOut();
                        yield return null;
                    }

                    break;
            }
        }

        private IEnumerator Fading(Level levelToLoad)
        {
            switch (currentFadeType)
            {
                case FadeType.FadeIn:
                    while (currentAlpha > 0f)
                    {
                        FadeIn();
                        yield return null;
                    }

                    break;
                case FadeType.FadeOut:
                    while (currentAlpha < 1f)
                    {
                        FadeOut();
                        yield return null;
                    }

                    break;
            }

            string sceneToLoad = "";
            switch (levelToLoad)
            {
                case Level.MainMenu:
                    sceneToLoad = GlobalVariables.MAIN_MENU;
                    break;
                case Level.Intro:
                    sceneToLoad = GlobalVariables.INTRO;
                    break;
                case Level.Graveyard:
                    sceneToLoad = GlobalVariables.GRAVEYARD;
                    break;
                case Level.Castle:
                    sceneToLoad = GlobalVariables.CASTLE;
                    break;
                case Level.Outro:
                    sceneToLoad = GlobalVariables.OUTRO;
                    break;
            }

            SceneManager.LoadScene(sceneToLoad);
        }

        private void FadeIn()
        {
            currentFadeSpeed += fadeSpeed * Time.deltaTime;
            currentAlpha -= currentFadeSpeed;
            Color color = image.color;
            //currentAlpha = color.a - currentFadeSpeed;
            color.a = currentAlpha;
            image.color = color;
            if (currentAlpha <= 0f)
            {
                image.enabled = false;
                currentFadeType = FadeType.None;
            }
        }

        private void FadeOut()
        {
            currentFadeSpeed += fadeSpeed * Time.deltaTime;
            currentAlpha += currentFadeSpeed;
            Color color = image.color;
            //currentAlpha = color.a + currentFadeSpeed;
            color.a = currentAlpha;
            image.color = color;
            if (currentAlpha >= 1f)
            {
                currentFadeType = FadeType.None;
            }
        }
    }
}