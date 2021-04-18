using System;
using UnityEngine;
using CursedWoods.Utils;
using CursedWoods.SaveSystem;
using System.Collections.Generic;

namespace CursedWoods
{
    public class GraveyardManager : MonoBehaviour, ISaveable
    {
        //private Timer checkKillsTimer;
        //private float killsTimerInterval = 0.5f;

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
        private EnemySpawnerOnStart skeletonBossSpawner = null;
        [SerializeField]
        private GameObject[] arenaEnemySpawners;

        //private bool hasBookGateKeySpawned;
        //private bool hasGraveyardSouthKeySpawned;
        private bool spawnSkeletonBoss;

        private Dictionary<GraveyardGateType, bool> gateLockedInfo = new Dictionary<GraveyardGateType, bool>();

        //Events that are invoked depending on how many enemies are killed.
        //public event Action SwordGateEvent;
        //public event Action SpellbookGateEvent;
        //public event Action MiddleAreaGateEvent;

        private void Awake()
        {
            //<checkKillsTimer = gameObject.AddComponent<Timer>();
            //checkKillsTimer.Set(killsTimerInterval);
        }

        private void Start()
        {
            //checkKillsTimer.Run();
        }

        private void OnEnable()
        {
            //checkKillsTimer.TimerCompleted += CheckKills;
            AIManager.EnemyGotKilled += CheckKills;
        }
        private void OnDisable()
        {
            //checkKillsTimer.TimerCompleted -= CheckKills;
            AIManager.EnemyGotKilled -= CheckKills;
        }

        public void Save(ISave saveSystem, string keyPrefix)
        {
            saveSystem.SetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.GRAVEYARD_SPAWN_SKELETON_BOSS_KEY), spawnSkeletonBoss);

            for (int i = 0; i < gateLockedInfo.Count; i++)
            {
                bool isOpen = gateLockedInfo[(GraveyardGateType)i];
                saveSystem.SetBool(SaveUtils.GetKey(keyPrefix, string.Format(SaveUtils.GRAVEYARD_GATE_OPEN_KEY, i)), isOpen);
            }
        }

        public void Load(ISave saveSystem, string keyPrefix)
        {
            spawnSkeletonBoss = saveSystem.GetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.GRAVEYARD_SPAWN_SKELETON_BOSS_KEY), true);

            gateLockedInfo.Clear();

            for (int i = 0; i < Enum.GetNames(typeof(GraveyardGateType)).Length; i++)
            {
                bool isOpen = saveSystem.GetBool(SaveUtils.GetKey(keyPrefix, string.Format(SaveUtils.GRAVEYARD_GATE_OPEN_KEY, i)), true);
                gateLockedInfo.Add((GraveyardGateType)i, isOpen);
            }
        }

        public void Initialize(ISave saveSystem, string keyPrefix)
        {
            Load(saveSystem, keyPrefix);

            if (!GetGateOpenStatus(GraveyardGateType.GraveyardMiddleAreaNorthGate))
            {
                for (int i=0; i<arenaEnemySpawners.Length; i++)
                {
                    arenaEnemySpawners[i].SetActive(false);
                }
            }
        }

        public void SetGateToOpenStatus(GraveyardGateType gate)
        {
            gateLockedInfo[gate] = false;
        }

        public bool GetGateOpenStatus(GraveyardGateType gate)
        {
            return gateLockedInfo[gate];
        }

        private void CheckKills(int killAmount)
        {
            //int killAmount = GameMan.Instance.AIManager.EnemiesKilledAmount;
            if (killAmount >= KillsToSpawnSwordGateKey)
            {
                if (gateLockedInfo[GraveyardGateType.GraveyardBookGate])
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
                if (gateLockedInfo[GraveyardGateType.GraveyardMiddleAreaSouthGate])
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
                if (spawnSkeletonBoss)
                {
                    /*
                    if (MiddleAreaGateEvent != null)
                    {
                        MiddleAreaGateEvent();
                    }
                    */

                    skeletonBossSpawner.enabled = true;
                    spawnSkeletonBoss = false;
                }
            }

            //checkKillsTimer.Run();
        }
    }
}