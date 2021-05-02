using UnityEngine;
using UnityEngine.Audio;
using CursedWoods.Data;

namespace CursedWoods
{
    public class Settings : MonoBehaviour
    {
        [SerializeField]
        private AudioContainer audioData = null;

        [SerializeField]
        private AudioMixer mixer = null;

        private const string SETTINGS_MANAGER_PATH = "Prefabs/Settings";
        private static Settings instance = null;

        //private static bool isQuitting = false;
        //private static object lockObj = new object();

        private bool useCombatLineRenderer = true;
        private bool cameraRotInvertBool;

        public float MusicInitVol { get; private set; }
        public float SfxInitVol { get; private set; }

        public float CameraRotationSpeed { get; set; } = 100f;
        public float CameraZoomSpeed { get; set; } = 6f;
        public float CameraRotInvert { get; private set; } = 1f;
        public bool CameraRotInvertBool
        {
            get { return cameraRotInvertBool; }
            set
            {
                cameraRotInvertBool = value;
                CameraRotInvert = value ? -1f : 1f;
            }
        }

        public float CombatRotSmoothAmount { get; set; } = 0.25f;


        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    // We don't have the one and only instance of this class created yet.
                    // Let's do that now. Resources.Load loads the asset from Resources folder.

                    // Remember to instantiate prefab before using it!
                    instance = Instantiate(Resources.Load<Settings>(SETTINGS_MANAGER_PATH));
                }

                return instance;
            }
        }

        /*
        public static Settings Instance
        {
            get
            {
                if (isQuitting)
                {
                    return null;
                }

                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<Settings>();

                        if (instance == null)
                        {
                            GameObject singletonGO = new GameObject();
                            instance = singletonGO.AddComponent<Settings>();
                            singletonGO.name = "Settings";
                            DontDestroyOnLoad(singletonGO);
                        }
                    }

                    return instance;
                }
            }
        }
        */

        public AudioManager Audio
        {
            get;
            private set;
        }

        public bool UseCombatLineRenderer
        {
            get { return useCombatLineRenderer; }
            set
            {
                useCombatLineRenderer = value;
                if (CombatLineValueChange != null)
                {
                    CombatLineValueChange(value);
                }
            }
        }

        public static event System.Action<bool> CombatLineValueChange;

        private void Awake()
        {
            if (instance == null)
            {
                // The one and the only instance of this class is not created yet
                instance = this;
            }
            else if (instance != this)
            {
                // The instance already exists and I am not the instance!
                // Destroy this object!
                Destroy(gameObject);
                return;
            }

            AudioSource audioSource = GetComponent<AudioSource>();
            Audio = gameObject.AddComponent<AudioManager>();
            Audio.InitAudioManager(audioSource, mixer, audioData);

            LoadPlayerPrefs();

            DontDestroyOnLoad(gameObject);
        }

        /*
        private void OnValidate()
        {
            if (Audio != null)
            {
                Audio.SetVolume(0.5f, AudioManager.SoundGroup.Effect);
                Audio.SetVolume(0.5f, AudioManager.SoundGroup.Music);
            }
        }
        */

        /*
        private void OnDestroy()
        {
            isQuitting = true;
        }

        private void OnApplicationQuit()
        {
            isQuitting = true;
        }
        */

        public void SavePlayerPrefs()
        {
            float musicVol;
            Audio.GetVolume(AudioManager.SoundGroup.Music, out musicVol);
            PlayerPrefs.SetFloat(GlobalVariables.MUSIC_VOL_KEY, musicVol);
            MusicInitVol = musicVol;

            float sfxVol;
            Audio.GetVolume(AudioManager.SoundGroup.Effect, out sfxVol);
            PlayerPrefs.SetFloat(GlobalVariables.SFX_VOL_KEY, sfxVol);
            SfxInitVol = sfxVol;

            PlayerPrefs.SetFloat(GlobalVariables.CAM_ROT_SPEED_KEY, CameraRotationSpeed);
            PlayerPrefs.SetFloat(GlobalVariables.CAM_ZOOM_SPEED_KEY, CameraZoomSpeed);
            PlayerPrefs.SetInt(GlobalVariables.CAM_INVERT_ROT_KEY, CameraRotInvertBool ? 1 : 0);

            PlayerPrefs.SetFloat(GlobalVariables.COMBAT_ROT_SMOOTH_KEY, CombatRotSmoothAmount);
            PlayerPrefs.SetInt(GlobalVariables.COMBAT_LINE_ENABLE_KEY, UseCombatLineRenderer ? 1 : 0);
        }

        private void LoadPlayerPrefs()
        {
            float musicVol;
            Audio.GetVolume(AudioManager.SoundGroup.Music, out musicVol);
            musicVol = PlayerPrefs.GetFloat(GlobalVariables.MUSIC_VOL_KEY, musicVol);
            Audio.SetVolume(musicVol, AudioManager.SoundGroup.Music);
            MusicInitVol = musicVol;

            float sfxVol;
            Audio.GetVolume(AudioManager.SoundGroup.Effect, out sfxVol);
            sfxVol = PlayerPrefs.GetFloat(GlobalVariables.SFX_VOL_KEY, sfxVol);
            Audio.SetVolume(sfxVol, AudioManager.SoundGroup.Effect);
            SfxInitVol = sfxVol;

            CameraRotationSpeed = PlayerPrefs.GetFloat(GlobalVariables.CAM_ROT_SPEED_KEY, CameraRotationSpeed);
            CameraZoomSpeed = PlayerPrefs.GetFloat(GlobalVariables.CAM_ZOOM_SPEED_KEY, CameraZoomSpeed);
            CameraRotInvertBool = PlayerPrefs.GetInt(GlobalVariables.CAM_INVERT_ROT_KEY, CameraRotInvertBool ? 1 : 0) == 1;

            CombatRotSmoothAmount = PlayerPrefs.GetFloat(GlobalVariables.COMBAT_ROT_SMOOTH_KEY, CombatRotSmoothAmount);
            UseCombatLineRenderer = PlayerPrefs.GetInt(GlobalVariables.COMBAT_LINE_ENABLE_KEY, UseCombatLineRenderer ? 1 : 0) == 1;
        }
    }
}