using CursedWoods.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CursedWoods
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] introVideos;
        [SerializeField]
        private Level levelToLoad = Level.Graveyard;
        private int currentVidID;
        private GameObject currentVid;
        private Fader fader;

        private void Awake()
        {
            currentVid = introVideos[0];
            currentVid.gameObject.SetActive(true);
            fader = GetComponent<Fader>();
        }

        private void Start()
        {
            fader.StartFade(FadeType.FadeOut);
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
            if (currentVidID < introVideos.Length - 1)
            {
                currentVidID++;
                fader.StartFade(currentVid, introVideos[currentVidID]);
                currentVid = introVideos[currentVidID];
            }
            else
            {
                fader.StartFade(FadeType.FadeIn, levelToLoad);
            }
        }
    }
}