using UnityEngine;

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

        #endregion Constants and statics

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
                    instance = Instantiate(Resources.Load<GameMan>(GAME_MANAGER_PATH));
                }

                return instance;
            }
        }

        /// <summary>
        /// Property for easy access to the object pool manager.
        /// </summary>
        public ObjectPoolManager ObjPoolMan
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
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            ObjPoolMan = GetComponent<ObjectPoolManager>();
            DontDestroyOnLoad(gameObject);
        }

        #endregion Unity messages
    }
}