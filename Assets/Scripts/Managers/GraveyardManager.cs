using System;
using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class GraveyardManager : MonoBehaviour
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

        private bool isSwordGateOpened;
        private bool isBookGateOpened;
        private bool isMiddleAreaGateOpened;

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

        private void CheckKills(int killAmount)
        {
            //int killAmount = GameMan.Instance.AIManager.EnemiesKilledAmount;
            if (killAmount >= KillsToSpawnSwordGateKey)
            {
                if (!isSwordGateOpened)
                {
                    /*
                    if (SwordGateEvent != null)
                    {
                        SwordGateEvent();
                    }
                    */

                    bookGateKey.gameObject.SetActive(true);

                    isSwordGateOpened = true;
                }
            }

            if (killAmount >= KillsToSpawnGraveyardKey)
            {
                if (!isBookGateOpened)
                {
                    /*
                    if (SpellbookGateEvent != null)
                    {
                        SpellbookGateEvent();
                    }
                    */

                    graveyardSouthKey.gameObject.SetActive(true);

                    isBookGateOpened = true;
                }
            }

            if (killAmount >= KillsToSpawnSkeletonBoss)
            {
                if (!isMiddleAreaGateOpened)
                {
                    /*
                    if (MiddleAreaGateEvent != null)
                    {
                        MiddleAreaGateEvent();
                    }
                    */

                    skeletonBossSpawner.enabled = true;
                    isMiddleAreaGateOpened = true;
                }
            }

            //checkKillsTimer.Run();
        }
    }
}