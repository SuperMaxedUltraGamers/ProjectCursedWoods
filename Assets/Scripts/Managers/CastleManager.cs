using UnityEngine;
using CursedWoods.SaveSystem;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class CastleManager : MonoBehaviour, ISaveable
    {
        public bool UseStartPos { get; set; }
        public bool FinalBossDoorOpen { get; set; }

        [SerializeField]
        private EnemySpawnerOnStart skeletonBoss3Spawner = null;

        public void Load(ISave saveSystem, string keyPrefix)
        {
            UseStartPos = saveSystem.GetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.CASTLE_USE_LEVEL_START_POS_KEY), true);
            FinalBossDoorOpen = saveSystem.GetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.CASTLE_FINAL_BOSS_DOOR_OPEN_KEY), false);
        }

        public void Save(ISave saveSystem, string keyPrefix)
        {
            saveSystem.SetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.CASTLE_USE_LEVEL_START_POS_KEY), UseStartPos);
            saveSystem.SetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.CASTLE_FINAL_BOSS_DOOR_OPEN_KEY), FinalBossDoorOpen);
        }

        public void Initialize(ISave saveSystem, string keyPrefix)
        {
            Load(saveSystem, keyPrefix);

            if (FinalBossDoorOpen)
            {
                skeletonBoss3Spawner.gameObject.SetActive(false);
            }
        }
    }
}