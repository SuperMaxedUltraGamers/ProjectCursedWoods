using UnityEngine;
using UnityEngine.Audio;
using CursedWoods.UI;
using CursedWoods.Data;

namespace CursedWoods
{
    public class GameMan : MonoBehaviour
    {
        #region Constants and statics

        /// <summary>
        /// Game manager prefab file path.
        /// </summary>
        private const string GAME_MANAGER_PATH = "Prefabs/GameManager";

        /// <summary>
        /// Only instance of the game manager.
        /// </summary>
        private static GameMan instance = null;

        private static bool isQuitting = false;
        private static object lockObj = new object();

        #endregion Constants and statics

        #region Private fields

        [SerializeField]
        private AudioContainer audioData = null;

        [SerializeField]
        private AudioMixer mixer = null;

        #endregion Private fields

        #region Properties

        /// <summary>
        /// Property for the only game manager.
        /// </summary>
        public static GameMan Instance
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
                        instance = FindObjectOfType<GameMan>();

                        if (instance == null)
                        {
                            //instance = Instantiate(Resources.Load<GameMan>(GAME_MANAGER_PATH));
                            GameObject singletonGO = new GameObject();
                            instance = singletonGO.AddComponent<GameMan>();
                            singletonGO.name = "GameManager";
                            DontDestroyOnLoad(singletonGO);
                            //DontDestroyOnLoad(instance);
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

        /// <summary>
        /// Property for easy access to the object pool manager.
        /// </summary>
        public ObjectPoolManager ObjPoolMan
        {
            get;
            private set;
        }

        public AIManager AIManager
        {
            get;
            private set;
        }

        public PlayerManager PlayerManager
        {
            get;
            private set;
        }

        public Transform PlayerT
        {
            get;
            private set;
        }

        public CharController CharController
        {
            get;
            private set;
        }
        public LevelUIManager LevelUIManager
        {
            get;
            private set;
        }

        #endregion Properties

        #region Unity messages

        private void Awake()
        {
            /*
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
            */

            ObjPoolMan = GetComponent<ObjectPoolManager>();
            AIManager = GetComponent<AIManager>();
            PlayerManager = GetComponent<PlayerManager>();
            CharController = FindObjectOfType<CharController>();
            LevelUIManager = FindObjectOfType<LevelUIManager>();
            PlayerT = CharController.gameObject.transform;

            AudioSource audioSource = GetComponent<AudioSource>();
            Audio = new AudioManager(audioSource, mixer, audioData);
            PlayerManager.Initialize();
            //PlayerManager.IsAttackUnlocked = true;
            //PlayerManager.IsSpellCastUnlocked = true;
            // TODO: load player unlocks from savefile and pass them to PlayerManager.

            DontDestroyOnLoad(gameObject);
        }
        private void OnValidate()
        {
            if (Audio != null)
            {
                Audio.SetVolume(0.5f, AudioManager.SoundGroup.Effect);
                Audio.SetVolume(0.5f, AudioManager.SoundGroup.Music);
            }
        }

        private void OnDestroy()
        {
            isQuitting = true;
        }

        private void OnApplicationQuit()
        {
            isQuitting = true;
        }

        #endregion Unity messages
    }
}