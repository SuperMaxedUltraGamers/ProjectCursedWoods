using System;
using UnityEngine;
using CursedWoods.Utils;
using CursedWoods.SaveSystem;
using System.Collections.Generic;

namespace CursedWoods
{
    public class GraveyardManager : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private int KillsToSpawnSwordGateKey = 3;
        [SerializeField]
        private int KillsToSpawnGraveyardKey = 6;
        [SerializeField]
        private int KillsToSpawnSkeletonBoss = 15;
        [SerializeField]
        private KeyPickUp bookGateKey = null;
        [SerializeField]
        private KeyPickUp graveyardSouthKey = null;
        [SerializeField]
        private EnemySpawnerOnStart skeletonBoss1Spawner = null;
        [SerializeField]
        private EnemySpawnerOnTrigger skeletonBoss2Spawner = null;
        [SerializeField]
        private GameObject[] arenaEnemySpawners;

        [SerializeField]
        private CollectableSpawner[] maxHealthIncreases;
        private bool[] spawnMaxHealtIncreases;

        //private bool hasBookGateKeySpawned;
        //private bool hasGraveyardSouthKeySpawned;
        private bool spawnSkeletonBoss1;

        private Dictionary<GateType, bool> gateLockedInfo = new Dictionary<GateType, bool>();

        [SerializeField]
        private GameObject[] barriers;

        [SerializeField]
        private EnableBarrierTrigger middleAreaBarrierTrigger;

        //Events that are invoked depending on how many enemies are killed.
        //public event Action SwordGateEvent;
        //public event Action SpellbookGateEvent;
        //public event Action MiddleAreaGateEvent;

        private void Awake()
        {
            spawnMaxHealtIncreases = new bool[maxHealthIncreases.Length];
        }

        private void OnEnable()
        {
            AIManager.EnemyGotKilled += CheckKills;
        }
        private void OnDisable()
        {
            AIManager.EnemyGotKilled -= CheckKills;
        }

        public void Save(ISave saveSystem, string keyPrefix)
        {
            saveSystem.SetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.GRAVEYARD_SPAWN_SKELETON_BOSS_1_KEY), spawnSkeletonBoss1);

            for (int i = 0; i < maxHealthIncreases.Length; i++)
            {
                saveSystem.SetBool(SaveUtils.GetKey(keyPrefix, string.Format(SaveUtils.GRAVEYARD_SPAWN_MAX_HEALTH_INCREASE_KEY, i)), spawnMaxHealtIncreases[i]);
            }

            for (int i = 0; i < gateLockedInfo.Count; i++)
            {
                bool isOpen = gateLockedInfo[(GateType)i];
                saveSystem.SetBool(SaveUtils.GetKey(keyPrefix, string.Format(SaveUtils.GRAVEYARD_GATE_OPEN_KEY, i)), isOpen);
            }
        }

        public void Load(ISave saveSystem, string keyPrefix)
        {
            spawnSkeletonBoss1 = saveSystem.GetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.GRAVEYARD_SPAWN_SKELETON_BOSS_1_KEY), true);

            for (int i = 0; i < maxHealthIncreases.Length; i++)
            {
                spawnMaxHealtIncreases[i] = saveSystem.GetBool(SaveUtils.GetKey(keyPrefix, string.Format(SaveUtils.GRAVEYARD_SPAWN_MAX_HEALTH_INCREASE_KEY, i)), true);
            }

            gateLockedInfo.Clear();

            for (int i = 0; i < Enum.GetNames(typeof(GateType)).Length; i++)
            {
                bool isOpen = saveSystem.GetBool(SaveUtils.GetKey(keyPrefix, string.Format(SaveUtils.GRAVEYARD_GATE_OPEN_KEY, i)), true);
                gateLockedInfo.Add((GateType)i, isOpen);
            }
        }

        public void Initialize(ISave saveSystem, string keyPrefix)
        {
            Load(saveSystem, keyPrefix);

            if (!GetGateOpenStatus(GateType.GraveyardMiddleAreaNorthGate))
            {
                for (int i = 0; i < arenaEnemySpawners.Length; i++)
                {
                    arenaEnemySpawners[i].SetActive(false);
                }
            }

            if (!GetGateOpenStatus(GateType.GraveyardGardenGate))
            {
                skeletonBoss2Spawner.gameObject.SetActive(false);
            }

            if (!GetGateOpenStatus(GateType.GraveyardMiddleAreaNorthGate))
            {
                middleAreaBarrierTrigger.gameObject.SetActive(false);
            }

            for (int i = 0; i < maxHealthIncreases.Length; i++)
            {
                if (!spawnMaxHealtIncreases[i])
                {
                    maxHealthIncreases[i].gameObject.SetActive(false);
                }
            }
        }

        public void SetGateToOpenStatus(GateType gate)
        {
            gateLockedInfo[gate] = false;
        }

        public bool GetGateOpenStatus(GateType gate)
        {
            return gateLockedInfo[gate];
        }

        public void DisableSpawMaxHealthIncrease(int collectibleId)
        {
            spawnMaxHealtIncreases[collectibleId] = false;
        }

        public bool GetMaxHealthIncreaseSpawnerStatus(int spawnerId)
        {
            return spawnMaxHealtIncreases[spawnerId];
        }

        public void EnableBarrier(int index)
        {
            barriers[index].SetActive(true);
        }

        public void DisableBarrier(int index)
        {
            barriers[index].SetActive(false);
        }

        private void CheckKills(int killAmount)
        {
            //int killAmount = GameMan.Instance.AIManager.EnemiesKilledAmount;
            if (killAmount >= KillsToSpawnSwordGateKey)
            {
                if (gateLockedInfo[GateType.GraveyardBookGate])
                {
                    /*
                    if (SwordGateEvent != null)
                    {
                        SwordGateEvent();
                    }
                    */

                    bookGateKey.gameObject.SetActive(true);
                    //hasBookGateKeySpawned = true;
                }
            }

            if (killAmount >= KillsToSpawnGraveyardKey)
            {
                if (gateLockedInfo[GateType.GraveyardMiddleAreaSouthGate])
                {
                    /*
                    if (SpellbookGateEvent != null)
                    {
                        SpellbookGateEvent();
                    }
                    */

                    graveyardSouthKey.gameObject.SetActive(true);
                    //hasGraveyardSouthKeySpawned = true;
                }
            }

            if (killAmount >= KillsToSpawnSkeletonBoss)
            {
                if (spawnSkeletonBoss1)
                {
                    /*
                    if (MiddleAreaGateEvent != null)
                    {
                        MiddleAreaGateEvent();
                    }
                    */

                    skeletonBoss1Spawner.enabled = true;
                    spawnSkeletonBoss1 = false;
                }
            }
        }
    }
}