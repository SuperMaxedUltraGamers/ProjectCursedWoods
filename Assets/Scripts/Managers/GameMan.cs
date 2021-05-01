using UnityEngine;
using UnityEngine.Audio;
using CursedWoods.UI;
using CursedWoods.Data;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using CursedWoods.SaveSystem;
using System.Collections;

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

        public GraveyardManager GraveyardManager
        {
            get;
            private set;
        }

        public CastleManager CastleManager
        {
            get;
            private set;
        }

        public ISave SaveSystem { get; private set; }

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

            InitializeSaveSystem();
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

        #endregion Unity messages

        public void NewGame()
        {
            // Delete savefile
            SaveSystem.DeleteSaveFile(SaveUtils.AUTOSAVE_SAVE_SLOT);

            // These should be called inside GameMan initialization
            /*
            if (PlayerManager == null)
            {
                PlayerManager = GetComponent<PlayerManager>();
            }

            PlayerManager.Initialize(SaveSystem, SaveUtils.PLAYER_MAN_PREFIX);

            if (AIManager == null)
            {
                AIManager = GetComponent<AIManager>();
            }

            AIManager.Initialize(SaveSystem, SaveUtils.AI_MAN_PREFIX);
            */

            // TODO: reset other progress

            SceneManager.LoadScene(GlobalVariables.INTRO);
        }

        public void LoadGame(string saveSlot)
        {
            SaveSystem.Load(saveSlot);

            string scene = SaveSystem.GetString(SaveUtils.GetKey(SaveUtils.GAME_MAN_PREFIX, SaveUtils.GAMEMAN_SCENE_NAME_KEY), GlobalVariables.GRAVEYARD);
            SceneManager.LoadScene(scene);
        }

        public void AutoSave()
        {
            // Save scene
            SaveSystem.SetString(SaveUtils.GetKey(SaveUtils.GAME_MAN_PREFIX, SaveUtils.GAMEMAN_SCENE_NAME_KEY), SceneManager.GetActiveScene().name);

            // Save player pos/rot
            Vector3 playerPos = PlayerT.position;
            float playerRot = PlayerT.rotation.eulerAngles.y;
            SaveSystem.SetFloat(SaveUtils.GetKey(SaveUtils.PLAYER_TRANS_PREFIX, SaveUtils.PLAYER_X_KEY), playerPos.x);
            SaveSystem.SetFloat(SaveUtils.GetKey(SaveUtils.PLAYER_TRANS_PREFIX, SaveUtils.PLAYER_Y_KEY), playerPos.y);
            SaveSystem.SetFloat(SaveUtils.GetKey(SaveUtils.PLAYER_TRANS_PREFIX, SaveUtils.PLAYER_Z_KEY), playerPos.z);
            SaveSystem.SetFloat(SaveUtils.GetKey(SaveUtils.PLAYER_TRANS_PREFIX, SaveUtils.PLAYER_ROT_KEY), playerRot);

            // Save player health and camera rotation
            CharController.Save(SaveSystem, SaveUtils.CHAR_CONTROLLER_PREFIX);

            // Save player progress
            PlayerManager.Save(SaveSystem, SaveUtils.PLAYER_MAN_PREFIX);

            // Save AI manager
            AIManager.Save(SaveSystem, SaveUtils.AI_MAN_PREFIX);

            // TODO: better way to save levelmanagers.
            // Save GraveyardMan
            if (GraveyardManager != null)
            {
                GraveyardManager.Save(SaveSystem, SaveUtils.GRAVEYARD_MAN_PREFIX);
            }

            // Save CastelMan
            if (CastleManager != null)
            {
                CastleManager.Save(SaveSystem, SaveUtils.CASTLE_MAN_PREFIX);
            }

            SaveSystem.Save(SaveUtils.AUTOSAVE_SAVE_SLOT);
            print("game saved");
        }

        private void InitializeGameMan(Scene scene, LoadSceneMode mode)
        {
            // TODO: load player unlocks from savefile and pass them to PlayerManager and other player progress and stuff.
            string scneneName = scene.name;
            switch (scneneName)
            {
                case GlobalVariables.MAIN_MENU:
                    //print("mainmenu init");
                    MainMenuInit();
                    break;
                case GlobalVariables.GRAVEYARD:
                    //print("graveyard init");

                    //#if (UNITY_EDITOR)
                    // Load PlayerMan, move to GraveyardInit?
                    if (PlayerManager == null)
                    {
                        PlayerManager = GetComponent<PlayerManager>();
                    }

                    PlayerManager.Initialize(SaveSystem, SaveUtils.PLAYER_MAN_PREFIX);

                    // Load GraveyardMan
                    if (GraveyardManager == null)
                    {
                        GraveyardManager = FindObjectOfType<GraveyardManager>();
                    }

                    GraveyardManager.Initialize(SaveSystem, SaveUtils.GRAVEYARD_MAN_PREFIX);

                    // Load AIman, move to GraveyardInit?
                    if (AIManager == null)
                    {
                        AIManager = GetComponent<AIManager>();
                    }

                    AIManager.Initialize(SaveSystem, SaveUtils.AI_MAN_PREFIX);
                    //#endif

                    StartCoroutine(GraveyardInit());
                    break;
                case GlobalVariables.CASTLE:
                    if (PlayerManager == null)
                    {
                        PlayerManager = GetComponent<PlayerManager>();
                    }

                    PlayerManager.Initialize(SaveSystem, SaveUtils.PLAYER_MAN_PREFIX);

                    // Load GraveyardMan
                    if (CastleManager == null)
                    {
                        CastleManager = FindObjectOfType<CastleManager>();
                    }

                    CastleManager.Initialize(SaveSystem, SaveUtils.CASTLE_MAN_PREFIX);

                    // Load AIman, move to GraveyardInit?
                    if (AIManager == null)
                    {
                        AIManager = GetComponent<AIManager>();
                    }

                    AIManager.Initialize(SaveSystem, SaveUtils.AI_MAN_PREFIX);

                    StartCoroutine(CastleInit());
                    break;
#if (UNITY_EDITOR)
                case "SampleScene 1":
                    // Load PlayerMan, move to GraveyardInit?
                    if (PlayerManager == null)
                    {
                        PlayerManager = GetComponent<PlayerManager>();
                    }

                    PlayerManager.Initialize(SaveSystem, SaveUtils.PLAYER_MAN_PREFIX);

                    // Load GraveyardMan
                    if (GraveyardManager == null)
                    {
                        GraveyardManager = FindObjectOfType<GraveyardManager>();
                    }

                    GraveyardManager.Initialize(SaveSystem, SaveUtils.GRAVEYARD_MAN_PREFIX);

                    // Load AIman, move to GraveyardInit?
                    if (AIManager == null)
                    {
                        AIManager = GetComponent<AIManager>();
                    }

                    AIManager.Initialize(SaveSystem, SaveUtils.AI_MAN_PREFIX);

                    StartCoroutine(GraveyardInit());
                    break;
#endif
                case GlobalVariables.OUTRO:
                    Settings.Instance.Audio.ChangeMusic(AudioContainer.Music.Menu);
                    break;
            }
        }

        private void MainMenuInit()
        {
            Time.timeScale = 1f;
            Settings.Instance.Audio.ChangeMusic(AudioContainer.Music.Menu);
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

        private IEnumerator GraveyardInit()
        {
            Settings.Instance.Audio.ChangeMusic(AudioContainer.Music.ForestAmbience);
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
            LevelUIManager.InitializeLevelUIManager(PlayerManager);

            ObjPoolMan.InitializeGraveyardObjectPool();

            PlayerT = CharController.gameObject.transform;

            // Wait for one frame before loading some of the values.
            yield return null;

            // Load player pos/rot
            Vector3 playerPos = PlayerT.position;
            float playerRot = PlayerT.rotation.eulerAngles.y;
            float posX = SaveSystem.GetFloat(SaveUtils.GetKey(SaveUtils.PLAYER_TRANS_PREFIX, SaveUtils.PLAYER_X_KEY), playerPos.x);
            float posY = SaveSystem.GetFloat(SaveUtils.GetKey(SaveUtils.PLAYER_TRANS_PREFIX, SaveUtils.PLAYER_Y_KEY), playerPos.y);
            float posZ = SaveSystem.GetFloat(SaveUtils.GetKey(SaveUtils.PLAYER_TRANS_PREFIX, SaveUtils.PLAYER_Z_KEY), playerPos.z);
            float rotY = SaveSystem.GetFloat(SaveUtils.GetKey(SaveUtils.PLAYER_TRANS_PREFIX, SaveUtils.PLAYER_ROT_KEY), playerRot);

            //print(new Vector3(posX, posY + 0.1f, posZ));
            PlayerT.position = new Vector3(posX, posY + 0.1f, posZ);
            PlayerT.rotation = Quaternion.Euler(0f, rotY, 0f);

            // Load player health and camera rotation
            CharController.Load(SaveSystem, SaveUtils.CHAR_CONTROLLER_PREFIX);



            LevelUIManager.StartFade(FadeType.FadeIn);
            //AudioSource audioSource = GetComponent<AudioSource>();
            //Audio = new AudioManager(audioSource, mixer, audioData);
        }

        private IEnumerator CastleInit()
        {
            Settings.Instance.Audio.ChangeMusic(AudioContainer.Music.CastleAmbience);
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
            LevelUIManager.InitializeLevelUIManager(PlayerManager);

            ObjPoolMan.InitializeCastleObjectPool();

            PlayerT = CharController.gameObject.transform;

            // Wait for one frame before loading some of the values.
            yield return null;

            // Load player pos/rot
            if (!CastleManager.UseStartPos)
            {
                Vector3 playerPos = PlayerT.position;
                float playerRot = PlayerT.rotation.eulerAngles.y;
                float posX = SaveSystem.GetFloat(SaveUtils.GetKey(SaveUtils.PLAYER_TRANS_PREFIX, SaveUtils.PLAYER_X_KEY), playerPos.x);
                float posY = SaveSystem.GetFloat(SaveUtils.GetKey(SaveUtils.PLAYER_TRANS_PREFIX, SaveUtils.PLAYER_Y_KEY), playerPos.y);
                float posZ = SaveSystem.GetFloat(SaveUtils.GetKey(SaveUtils.PLAYER_TRANS_PREFIX, SaveUtils.PLAYER_Z_KEY), playerPos.z);
                float rotY = SaveSystem.GetFloat(SaveUtils.GetKey(SaveUtils.PLAYER_TRANS_PREFIX, SaveUtils.PLAYER_ROT_KEY), playerRot);

                //print(new Vector3(posX, posY + 0.1f, posZ));
                PlayerT.position = new Vector3(posX, posY + 0.1f, posZ);
                PlayerT.rotation = Quaternion.Euler(0f, rotY, 0f);
            }

            // Load player health and camera rotation
            CharController.Load(SaveSystem, SaveUtils.CHAR_CONTROLLER_PREFIX);



            LevelUIManager.StartFade(FadeType.FadeIn);
            //AudioSource audioSource = GetComponent<AudioSource>();
            //Audio = new AudioManager(audioSource, mixer, audioData);
        }

        private void InitializeSaveSystem()
        {
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string gameFolder = "Cursed of The Amulet";
            string saveFolder = Path.Combine(myDocuments, gameFolder);

            SaveSystem = new BinarySaver(saveFolder);
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