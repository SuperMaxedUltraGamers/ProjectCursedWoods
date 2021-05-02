using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CursedWoods.UI
{

    public class OptionsManager : MonoBehaviour
    {
        [SerializeField]
        protected EventSystem eventSystem;

        [SerializeField]
        protected Button[] optionsMenuButtons;
        [SerializeField]
        protected GameObject optionsMenu;
        [SerializeField]
        protected GameObject cameraOptions;
        [SerializeField]
        protected GameObject combatOptions;
        [SerializeField]
        protected GameObject audioOptions;

        [SerializeField]
        protected Slider musicVolumeSlider = null;

        [SerializeField]
        protected Slider SFXVolumeSlider = null;

        [SerializeField]
        protected Slider camRotSpeedSlider = null;

        [SerializeField]
        protected Slider camZoomSpeedSlider = null;

        [SerializeField]
        protected Toggle camRotInvert = null;

        [SerializeField]
        protected Slider combatRotSmoothSlider = null;

        [SerializeField]
        protected Toggle combatLineToggle = null;

        protected AudioSource audioSource;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        protected virtual void Start()
        {
            musicVolumeSlider.onValueChanged.AddListener(delegate { MusicVolValCheck(); });
            SFXVolumeSlider.onValueChanged.AddListener(delegate { SFXVolValCheck(); });

            camRotSpeedSlider.onValueChanged.AddListener(delegate { CamRotSpeedValueCheck(); });
            camZoomSpeedSlider.onValueChanged.AddListener(delegate { CamZoomSpeedValueCheck(); });
            camRotInvert.onValueChanged.AddListener(delegate { CamInvertToggle(); });

            combatRotSmoothSlider.onValueChanged.AddListener(delegate { CombatRotSmoothValueCheck(); });
            combatLineToggle.onValueChanged.AddListener(delegate { LineRendererToggle(); });

            InitializeOptions();
        }

        public void BackOutToOptionsMenu()
        {
            eventSystem.SetSelectedGameObject(null);
            ToggleOptionsMenuButtons(true);
            cameraOptions.SetActive(false);
            combatOptions.SetActive(false);
            audioOptions.SetActive(false);
            PlayButtonSFX();
        }

        public void CameraOptionsButton()
        {
            eventSystem.SetSelectedGameObject(null);
            cameraOptions.SetActive(true);
            ToggleOptionsMenuButtons(false);
            PlayButtonSFX();
        }

        public void CombatOptionsButton()
        {
            eventSystem.SetSelectedGameObject(null);
            combatOptions.SetActive(true);
            ToggleOptionsMenuButtons(false);
            PlayButtonSFX();
        }

        public void AudioOptionsButton()
        {
            eventSystem.SetSelectedGameObject(null);
            audioOptions.SetActive(true);
            ToggleOptionsMenuButtons(false);
            PlayButtonSFX();
        }

        protected void LineRendererToggle()
        {
            Settings.Instance.UseCombatLineRenderer = combatLineToggle.isOn;
        }

        protected void MusicVolValCheck()
        {
            Settings.Instance.Audio.SetVolume(musicVolumeSlider.value, AudioManager.SoundGroup.Music);
        }

        protected void SFXVolValCheck()
        {
            Settings.Instance.Audio.SetVolume(SFXVolumeSlider.value, AudioManager.SoundGroup.Effect);
        }

        protected void CamRotSpeedValueCheck()
        {
            Settings.Instance.CameraRotationSpeed = camRotSpeedSlider.value;
        }

        protected void CamInvertToggle()
        {
            Settings.Instance.CameraRotInvertBool = camRotInvert.isOn;
        }

        protected void CamZoomSpeedValueCheck()
        {
            Settings.Instance.CameraZoomSpeed = camZoomSpeedSlider.value;
        }

        protected void CombatRotSmoothValueCheck()
        {
            Settings.Instance.CombatRotSmoothAmount = combatRotSmoothSlider.value;
        }

        protected void PlayButtonSFX()
        {
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MiscSFX.ButtonPress);
        }

        //private void InitializeVolumeSlider(Slider volumeSlider, AudioManager.SoundGroup soundGroup)
        //{
        //    float volume;
        //    if (Settings.Instance.Audio.GetVolume(soundGroup, out volume))
        //    {
        //        volumeSlider.value = volume;
        //    }
        //}

        private void InitializeOptions()
        {
            combatLineToggle.isOn = Settings.Instance.UseCombatLineRenderer;
            camRotSpeedSlider.value = Settings.Instance.CameraRotationSpeed;
            camZoomSpeedSlider.value = Settings.Instance.CameraZoomSpeed;
            camRotInvert.isOn = Settings.Instance.CameraRotInvertBool;
            combatRotSmoothSlider.value = Settings.Instance.CombatRotSmoothAmount;

            InitVolSliders();
        }

        private void ToggleOptionsMenuButtons(bool isOn)
        {
            foreach (Button button in optionsMenuButtons)
            {
                button.gameObject.SetActive(isOn);
            }
        }

        private void InitVolSliders()
        {
            musicVolumeSlider.value = Settings.Instance.MusicInitVol;
            SFXVolumeSlider.value = Settings.Instance.SfxInitVol;
            //InitializeVolumeSlider(musicVolumeSlider, AudioManager.SoundGroup.Music);
            //InitializeVolumeSlider(SFXVolumeSlider, AudioManager.SoundGroup.Effect);
        }
    }
}