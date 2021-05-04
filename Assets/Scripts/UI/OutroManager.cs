using CursedWoods.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CursedWoods
{
    public class OutroManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject startVid;
        private GameObject[] chosenEndingVids;
        [SerializeField]
        private GameObject[] goodEndingVids;
        [SerializeField]
        private GameObject[] badEndingVids;
        private GameObject currentVid;
        private int currentVidID;
        [SerializeField]
        private Level levelToLoad = Level.MainMenu;
        private Fader fader;
        private bool isEndingChosen;
        private AudioSource audioSource;

        private void Awake()
        {
            currentVid = startVid;
            startVid.gameObject.SetActive(true);
            fader = GetComponent<Fader>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            fader.StartFade(FadeType.FadeOut);
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
            chosenEndingVids = goodEndingVids;
            isEndingChosen = true;
            ChangePicture();
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MiscSFX.ButtonPress);
        }

        public void BadEndingButton()
        {
            chosenEndingVids = badEndingVids;
            isEndingChosen = true;
            ChangePicture();
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MiscSFX.ButtonPress);
        }

        private void ChangePicture()
        {
            if (currentVidID < chosenEndingVids.Length - 1)
            {
                if (currentVid != startVid)
                {
                    currentVidID++;
                }

                fader.StartFade(currentVid, chosenEndingVids[currentVidID]);
                currentVid = chosenEndingVids[currentVidID];
            }
            else
            {
                fader.StartFade(FadeType.FadeIn, levelToLoad);
            }
        }
    }
}