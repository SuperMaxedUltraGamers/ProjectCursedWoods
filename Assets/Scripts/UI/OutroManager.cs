using CursedWoods.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CursedWoods
{
    public class OutroManager : MonoBehaviour
    {
        [SerializeField]
        private Image startImg;
        private Image[] chosenEndingImgs;
        [SerializeField]
        private Image[] goodEndingImgs;
        [SerializeField]
        private Image[] badEndingImgs;
        private Image currentImg;
        private int currentImgID;
        [SerializeField]
        private Level levelToLoad = Level.MainMenu;
        private Fader fader;
        private bool isEndingChosen;
        private AudioSource audioSource;

        private void Awake()
        {
            currentImg = startImg;
            startImg.gameObject.SetActive(true);
            fader = GetComponent<Fader>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            fader.StartFade(FadeType.FadeIn);
        }

        private void Update()
        {
            if (!fader.IsFading && isEndingChosen)
            {
                if (Input.anyKeyDown)
                {
                    ChangePicture();
                }
            }
        }

        public void GoodEndingButton()
        {
            chosenEndingImgs = goodEndingImgs;
            isEndingChosen = true;
            ChangePicture();
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MiscSFX.ButtonPress);
        }

        public void BadEndingButton()
        {
            chosenEndingImgs = badEndingImgs;
            isEndingChosen = true;
            ChangePicture();
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MiscSFX.ButtonPress);
        }

        private void ChangePicture()
        {
            if (currentImgID < chosenEndingImgs.Length - 1)
            {
                if (currentImg != startImg)
                {
                    currentImgID++;
                }

                fader.StartFade(currentImg, chosenEndingImgs[currentImgID]);
                currentImg = chosenEndingImgs[currentImgID];
            }
            else
            {
                fader.StartFade(FadeType.FadeOut, levelToLoad);
            }
        }
    }
}