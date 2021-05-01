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

        public float CameraRotationSpeed { get; set; } = 100f;
        public float CameraZoomSpeed { get; set; } = 6f;

        public float CombatRotSmoothAmount { get; set; } = 0.25f;
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
    }
}