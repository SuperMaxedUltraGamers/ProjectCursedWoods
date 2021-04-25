using CursedWoods.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CursedWoods
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField]
        private Image[] introImages;
        private int currentImgID;
        private Image currentImg;
        private Fader fader;

        private void Awake()
        {
            currentImg = introImages[0];
            currentImg.gameObject.SetActive(true);
            fader = GetComponent<Fader>();
        }

        private void Start()
        {
            fader.StartFade(FadeType.FadeIn);
        }

        private void Update()
        {
            if (!fader.IsFading)
            {
                if (Input.anyKeyDown)
                {
                    ChangePicture();
                }
            }
        }

        private void ChangePicture()
        {
            if (currentImgID < introImages.Length - 1)
            {
                currentImgID++;
                fader.StartFade(currentImg, introImages[currentImgID]);
                //ToggleFadeDir();
                currentImg = introImages[currentImgID];
            }
            else
            {
                fader.StartFade(FadeType.FadeOut, Level.Graveyard);
            }
        }

        /*
        private void ToggleFadeDir()
        {
            if (fadeDir == FadeType.FadeIn)
            {
                fadeDir = FadeType.FadeOut;
            }
            else
            {
                fadeDir = FadeType.FadeIn;
            }
        }
        */
    }
}