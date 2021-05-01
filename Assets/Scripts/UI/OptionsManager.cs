using UnityEngine;
using UnityEngine.UI;

namespace CursedWoods.UI
{

    public class OptionsManager : MonoBehaviour
    {
        [SerializeField]
        protected Slider musicVolumeSlider = null;

        [SerializeField]
        protected Slider SFXVolumeSlider = null;

        [SerializeField]
        protected Slider camRotSpeedSlider = null;

        [SerializeField]
        protected Slider camZoomSpeedSlider = null;

        [SerializeField]
        protected Slider combatRotSmoothSlider = null;

        [SerializeField]
        protected Toggle combatLineToggle = null;

        protected AudioSource audioSource;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            musicVolumeSlider.onValueChanged.AddListener(delegate { MusicVolValCheck(); });
            SFXVolumeSlider.onValueChanged.AddListener(delegate { SFXVolValCheck(); });
            camRotSpeedSlider.onValueChanged.AddListener(delegate { CamRotSpeedValueCheck(); });
            camZoomSpeedSlider.onValueChanged.AddListener(delegate { CamZoomSpeedValueCheck(); });
            combatRotSmoothSlider.onValueChanged.AddListener(delegate { CombatRotSmoothValueCheck(); });
            combatLineToggle.onValueChanged.AddListener(delegate { LineRendererToggle(); });

            InitializeOptions();
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

        private void InitializeVolumeSlider(Slider volumeSlider, AudioManager.SoundGroup soundGroup)
        {
            float volume;
            if (Settings.Instance.Audio.GetVolume(soundGroup, out volume))
            {
                volumeSlider.value = volume;
            }
        }

        private void InitializeOptions()
        {
            combatLineToggle.isOn = Settings.Instance.UseCombatLineRenderer;
            camRotSpeedSlider.value = Settings.Instance.CameraRotationSpeed;
            camZoomSpeedSlider.value = Settings.Instance.CameraZoomSpeed;
            combatRotSmoothSlider.value = Settings.Instance.CombatRotSmoothAmount;

            InitializeVolumeSlider(musicVolumeSlider, AudioManager.SoundGroup.Music);
            InitializeVolumeSlider(SFXVolumeSlider, AudioManager.SoundGroup.Effect);
        }
    }
}