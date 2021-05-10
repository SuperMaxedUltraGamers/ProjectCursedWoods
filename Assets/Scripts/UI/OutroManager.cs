using CursedWoods.Utils;
using UnityEngine;
using UnityEngine.Video;

namespace CursedWoods
{
    public class OutroManager : MonoBehaviour
    {
        [SerializeField]
        private VideoPlayer startVid;
        private VideoPlayer[] chosenEndingVids;
        [SerializeField]
        private VideoPlayer[] goodEndingVids;
        [SerializeField]
        private VideoPlayer[] badEndingVids;
        private VideoPlayer currentVid;
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
            fader.StartFade(FadeType.FadeOut, 1f);
        }

        private void Update()
        {
            if (!fader.IsFading && isEndingChosen)
            {
                if (Input.anyKeyDown)
                {
                    ChangeVid();
                }
            }
        }

        public void GoodEndingButton()
        {
            chosenEndingVids = goodEndingVids;
            isEndingChosen = true;
            ChangeVid();
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MiscSFX.ButtonPress);
        }

        public void BadEndingButton()
        {
            chosenEndingVids = badEndingVids;
            isEndingChosen = true;
            ChangeVid();
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MiscSFX.ButtonPress);
        }

        private void ChangeVid()
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