using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CursedWoods.SaveSystem;

namespace CursedWoods
{
    public class CastleManager : MonoBehaviour, ISaveable
    {
        public bool UseStartPos { get; set; }

        public void Load(ISave saveSystem, string keyPrefix)
        {
            UseStartPos = saveSystem.GetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.CASTLE_USE_LEVEL_START_POS_KEY), true);
        }

        public void Save(ISave saveSystem, string keyPrefix)
        {
            saveSystem.SetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.CASTLE_USE_LEVEL_START_POS_KEY), UseStartPos);
        }

        public void Initialize(ISave saveSystem, string keyPrefix)
        {
            Load(saveSystem, keyPrefix);

            /*
            if (!GetGateOpenStatus(GraveyardGateType.GraveyardMiddleAreaNorthGate))
            {
                for (int i = 0; i < arenaEnemySpawners.Length; i++)
                {
                    arenaEnemySpawners[i].SetActive(false);
                }
            }
            */
        }
    }
}