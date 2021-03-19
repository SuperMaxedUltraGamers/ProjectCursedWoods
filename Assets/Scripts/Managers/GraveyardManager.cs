using System;
using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class GraveyardManager : MonoBehaviour
    {
        private Timer checkKillsTimer;
        private float killsTimerInterval = 0.5f;

        [SerializeField]
        private int KillsToOpenSwordGate = 3;
        [SerializeField]
        private int KillsToOpenBookGate = 8;
        [SerializeField]
        private int KillsToOpenMiddleAreaGate = 20;

        private bool isSwordGateOpened;
        private bool isBookGateOpened;
        private bool isMiddleAreaGateOpened;

        //Events that are invoked depending on how many enemies are killed.
        public event Action SwordGateEvent;
        public event Action SpellbookGateEvent;
        public event Action MiddleAreaGateEvent;

        private void Awake()
        {
            checkKillsTimer = gameObject.AddComponent<Timer>();
            checkKillsTimer.Set(killsTimerInterval);
            checkKillsTimer.Run();
        }

        private void OnEnable()
        {
            checkKillsTimer.TimerCompleted += CheckKills;
        }
        private void OnDisable()
        {
            checkKillsTimer.TimerCompleted -= CheckKills;
        }

        private void CheckKills()
        {
            int killAmount = GameMan.Instance.AIManager.EnemiesKilledAmount;

            if (killAmount == KillsToOpenSwordGate)
            {
                if (!isSwordGateOpened)
                {
                    if (SwordGateEvent != null)
                    {
                        SwordGateEvent();
                    }

                    isSwordGateOpened = true;
                }

            }
            else if (killAmount == KillsToOpenBookGate)
            {
                if (!isBookGateOpened)
                {
                    if (SpellbookGateEvent != null)
                    {
                        SpellbookGateEvent();
                    }

                    isBookGateOpened = true;
                }
            }
            else if (killAmount == KillsToOpenMiddleAreaGate)
            {
                if (!isMiddleAreaGateOpened)
                {
                    if (MiddleAreaGateEvent != null)
                    {
                        MiddleAreaGateEvent();
                    }

                    isMiddleAreaGateOpened = true;
                }
            }

            checkKillsTimer.Run();
        }
    }
}