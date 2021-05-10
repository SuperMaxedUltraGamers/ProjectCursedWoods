using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace CursedWoods.Utils
{
    public class Fader : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        private float fadeSpeed = 0.8f;  //0.005f if deltaTime multiplier in a different spot
        private float currentFadeSpeed;
        private float currentAlpha = 1f;
        private FadeType currentFadeType;

        public bool IsFading { get; private set; }

        private void Awake()
        {
            image.enabled = true;
        }

        public void StartFade(FadeType fadeType)
        {
            currentFadeType = fadeType;
            switch (currentFadeType)
            {
                case FadeType.FadeOut:
                    currentAlpha = 4.5f;
                    currentFadeSpeed = 0f;
                    break;
                case FadeType.FadeIn:
                    currentAlpha = 0f;
                    currentFadeSpeed = 0f;
                    break;
            }

            image.gameObject.SetActive(true);
            IsFading = true;
            StartCoroutine(Fading());
        }

        public void StartFade(FadeType fadeType, float startAlpha)
        {
            currentFadeType = fadeType;
            switch (currentFadeType)
            {
                case FadeType.FadeOut:
                    currentAlpha = startAlpha;
                    currentFadeSpeed = 0f;
                    break;
                case FadeType.FadeIn:
                    currentAlpha = 0f;
                    currentFadeSpeed = 0f;
                    break;
            }

            image.gameObject.SetActive(true);
            IsFading = true;
            StartCoroutine(Fading());
        }

        public void StartFade(FadeType fadeType, Level levelToLoadAfterFade)
        {
            currentFadeType = fadeType;
            switch (currentFadeType)
            {
                case FadeType.FadeOut:
                    currentAlpha = 4.5f;
                    currentFadeSpeed = 0f;
                    break;
                case FadeType.FadeIn:
                    currentAlpha = 0f;
                    currentFadeSpeed = 0f;
                    break;
            }

            image.gameObject.SetActive(true);
            IsFading = true;
            StartCoroutine(Fading(levelToLoadAfterFade));
        }

        public void StartFade(GameObject disableImg, GameObject enableImg)
        {
            currentFadeType = FadeType.FadeOut;
            currentAlpha = 1f;
            currentFadeSpeed = 0f;

            image.gameObject.SetActive(true);
            IsFading = true;
            StartCoroutine(Fading(disableImg, enableImg));
        }

        public void StartFade(VideoPlayer disableVid, VideoPlayer enableVid)
        {
            currentFadeType = FadeType.FadeOut;
            currentAlpha = 1f;
            currentFadeSpeed = 0f;

            image.gameObject.SetActive(true);
            IsFading = true;
            StartCoroutine(Fading(disableVid, enableVid));
        }

        private IEnumerator Fading()
        {
            switch (currentFadeType)
            {
                case FadeType.FadeOut:
                    while (currentAlpha > 0f)
                    {
                        FadeOut(true);
                        yield return null;
                    }

                    break;
                case FadeType.FadeIn:
                    while (currentAlpha < 1f)
                    {
                        FadeIn(true);
                        yield return null;
                    }

                    break;
            }

            IsFading = false;
        }

        private IEnumerator Fading(Level levelToLoad)
        {
            switch (currentFadeType)
            {
                case FadeType.FadeOut:
                    while (currentAlpha > 0f)
                    {
                        FadeOut(true);
                        yield return null;
                    }

                    break;
                case FadeType.FadeIn:
                    while (currentAlpha < 1f)
                    {
                        FadeIn(true);
                        yield return null;
                    }

                    break;
            }

            IsFading = false;

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

        private IEnumerator Fading(GameObject fromImg, GameObject toImg)
        {
            currentFadeType = FadeType.FadeOut;
            currentAlpha = 0f;

            // Fade from black
            while (currentAlpha < 1f)
            {
                FadeIn(true);
                yield return null;
            }

            // Disable and enable videos/pictures
            fromImg.SetActive(false);
            toImg.SetActive(true);

            // Now fade back
            while (currentAlpha > 0f)
            {
                FadeOut(true);
                yield return null;
            }

            IsFading = false;
        }

        private IEnumerator Fading(VideoPlayer fromVid, VideoPlayer toVid)
        {
            currentFadeType = FadeType.FadeOut;
            currentAlpha = 0f;

            // Fade from black
            while (currentAlpha < 1f)
            {
                FadeIn(true);
                yield return null;
            }

            // Disable and enable videos
            fromVid.gameObject.SetActive(false);
            toVid.gameObject.SetActive(true);
            //toVid.targetTexture.Release();

            // Now fade back
            while (currentAlpha > 0f)
            {
                FadeOut(true);
                yield return null;
            }

            IsFading = false;
        }

        private void FadeOut(bool useDeltaTime)
        {
            currentFadeSpeed += fadeSpeed;
            if (useDeltaTime)
            {
                currentFadeSpeed *= Time.deltaTime;
            }

            currentAlpha -= currentFadeSpeed;
            Color color = image.color;
            color.a = currentAlpha;
            image.color = color;
            if (currentAlpha <= 0f)
            {
                image.gameObject.SetActive(false);
                currentFadeType = FadeType.None;
            }
        }

        private void FadeIn(bool useDeltaTime)
        {
            currentFadeSpeed += fadeSpeed;
            if (useDeltaTime)
            {
                currentFadeSpeed *= Time.deltaTime;
            }

            currentAlpha += currentFadeSpeed;
            Color color = image.color;
            color.a = currentAlpha;
            image.color = color;
            if (currentAlpha >= 1f)
            {
                currentFadeType = FadeType.None;
            }
        }
    }
}