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

        private static Settings instance = null;

        private static bool isQuitting = false;
        private static object lockObj = new object();

        private bool useCombatLineRenderer = true;

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

        public AudioManager Audio
        {
            get;
            private set;
        }

        public float CameraRotationSpeed { get; set; } = 30f;
        public float CameraZoomSpeed { get; set; } = 6f;

        public float CombatRotSmoothAmount { get; set; } = 0.3f;
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
            AudioSource audioSource = GetComponent<AudioSource>();
            Audio = new AudioManager(audioSource, mixer, audioData);
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
    }
}