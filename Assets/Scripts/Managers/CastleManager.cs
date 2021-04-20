using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CursedWoods.SaveSystem;

namespace CursedWoods
{
    public class CastleManager : MonoBehaviour, ISaveable
    {
        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Load(ISave saveSystem, string keyPrefix)
        {
        }

        public void Save(ISave saveSystem, string keyPrefix)
        {
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