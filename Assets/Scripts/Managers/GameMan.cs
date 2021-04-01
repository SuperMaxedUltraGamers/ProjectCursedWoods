using UnityEngine;
using UnityEngine.Audio;
using CursedWoods.UI;
using CursedWoods.Data;
using UnityEngine.SceneManagement;

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

        //private static bool isQuitting = false;
        //private static object lockObj = new object();

        #endregion Constants and statics

        #region Private fields

        /*
        [SerializeField]
        private AudioContainer audioData = null;

        [SerializeField]
        private AudioMixer mixer = null;
        */

        #endregion Private fields

        #region Properties

        /// <summary>
        /// Property for the only game manager.
        /// </summary>
        public static GameMan Instance
        {
            get
            {
                if (instance == null)
                {
                    // We don't have the one and only instance of this class created yet.
                    // Let's do that now. Resources.Load loads the asset from Resources folder.

                    // Remember to instantiate prefab before using it!
                    instance = Instantiate(Resources.Load<GameMan>(GAME_MANAGER_PATH));
                }

                return instance;
            }
        }

        /*
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
        */

        /*
        public AudioManager Audio
        {
            get;
            private set;
        }
        */

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

            /*
            ObjPoolMan = GetComponent<ObjectPoolManager>();
            AIManager = GetComponent<AIManager>();
            PlayerManager = GetComponent<PlayerManager>();
            CharController = FindObjectOfType<CharController>();
            LevelUIManager = FindObjectOfType<LevelUIManager>();
            PlayerT = CharController.gameObject.transform;

            //AudioSource audioSource = GetComponent<AudioSource>();
            //Audio = new AudioManager(audioSource, mixer, audioData);
            PlayerManager.Initialize();
            //PlayerManager.IsAttackUnlocked = true;
            //PlayerManager.IsSpellCastUnlocked = true;
            // TODO: load player unlocks from savefile and pass them to PlayerManager.
            */

            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += InitializeGameMan;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= InitializeGameMan;
        }

        public void NewGame()
        {
            if (PlayerManager == null)
            {
                PlayerManager = GetComponent<PlayerManager>();
            }

            PlayerManager.Initialize();

            if (AIManager == null)
            {
                AIManager = GetComponent<AIManager>();
            }

            AIManager.ResetProgress();

            // TODO: reset other progress

            SceneManager.LoadScene(GlobalVariables.GRAVEYARD);
        }

        public void LoadGame()
        {
            // TODO: Load progress values from file.
            // TODO: load correct scene.
        }

        private void InitializeGameMan(Scene scene, LoadSceneMode mode)
        {
            // TODO: load player unlocks from savefile and pass them to PlayerManager and other player progress and stuff.
            string scneneName = scene.name;
            switch(scneneName)
            {
                case GlobalVariables.MAIN_MENU:
                    //print("mainmenu init");
                    MainMenuInit();
                    break;
                case GlobalVariables.GRAVEYARD:
                    //print("graveyard init");

#if (UNITY_EDITOR)
                    if (PlayerManager == null)
                    {
                        PlayerManager = GetComponent<PlayerManager>();
                    }

                    PlayerManager.Initialize();

                    if (AIManager == null)
                    {
                        AIManager = GetComponent<AIManager>();
                    }

                    AIManager.ResetProgress();
#endif

                    GraveyardInit();
                    break;
#if (UNITY_EDITOR)
                case "SampleScene 1":
                    if (PlayerManager == null)
                    {
                        PlayerManager = GetComponent<PlayerManager>();
                    }

                    PlayerManager.Initialize();

                    if (AIManager == null)
                    {
                        AIManager = GetComponent<AIManager>();
                    }

                    AIManager.ResetProgress();

                    GraveyardInit();
                    break;
#endif
            }
        }

        private void MainMenuInit()
        {
            if (CharController != null)
            {
                //Destroy(CharController.transform.root);
                Destroy(PlayerT.root);
                // Kinda useless to set these null.
                PlayerT = null;
                CharController = null;
            }

            // TODO: Reset or nullify AIManager or do nothing?
            /*
            if (AIManager != null)
            {
                
            }
            */

            // TODO: Reset PlayerManager progress etc.??

            if (LevelUIManager != null)
            {
                Destroy(LevelUIManager.transform.root);
                // Kinda useless to set this null.
                LevelUIManager = null;
            }
        }

        private void GraveyardInit()
        {
            if (ObjPoolMan == null)
            {
                ObjPoolMan = GetComponent<ObjectPoolManager>();
            }

            /*
            if (AIManager == null)
            {
                AIManager = GetComponent<AIManager>();
            }
            */

            /*
            if (PlayerManager == null)
            {
                PlayerManager = GetComponent<PlayerManager>();
                PlayerManager.Initialize();
            }
            */

            CharController = FindObjectOfType<CharController>();
            LevelUIManager = FindObjectOfType<LevelUIManager>();
            PlayerT = CharController.gameObject.transform;

            //AudioSource audioSource = GetComponent<AudioSource>();
            //Audio = new AudioManager(audioSource, mixer, audioData);
            ObjPoolMan.InitializeGraveyardObjectPool();
            //PlayerManager.IsAttackUnlocked = true;
            //PlayerManager.IsSpellCastUnlocked = true;
            // TODO: load player unlocks from savefile and pass them to PlayerManager.
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

#endregion Unity messages
    }
}