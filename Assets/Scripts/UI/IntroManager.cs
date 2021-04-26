using CursedWoods.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CursedWoods
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField]
        private Image[] introImages;
        [SerializeField]
        private Level levelToLoad = Level.Graveyard;
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
                currentImg = introImages[currentImgID];
            }
            else
            {
                fader.StartFade(FadeType.FadeOut, levelToLoad);
            }
        }
    }
}