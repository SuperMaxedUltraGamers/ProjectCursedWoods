using System;
using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class AIManager : MonoBehaviour
    {
        [SerializeField]
        private float fleeAffectorReduceRate = 2f;
        private Timer fleeAffectorReduceTimer;
        private int enemiesKilledAmount;
        private int enemiesKilledFleeAffector;

        public static event Action<int> EnemyGotKilled;
        public static event Action<int> EnemyFleeAffectorChange;

        public int EnemiesAttackingAmount { get; set; }

        public int EnemiesKilledAmount 
        { 
            get
            {
                return enemiesKilledAmount;
            }
            private set 
            {
                enemiesKilledAmount = value;
                if (EnemyGotKilled != null)
                {
                    EnemyGotKilled(enemiesKilledAmount);
                }
            } 
        }

        public int EnemiesKilledFleeAffector
        {
            get
            {
                return enemiesKilledFleeAffector;
            }
            set
            {
                if (value > enemiesKilledFleeAffector)
                {
                    if (EnemyFleeAffectorChange != null)
                    {
                        EnemyFleeAffectorChange(value);
                    }

                    // Add one to enemieskilled.
                    EnemiesKilledAmount++;
                }

                enemiesKilledFleeAffector = value;
                fleeAffectorReduceTimer.Stop();
                fleeAffectorReduceTimer.Run();
            }
        }

        private void Awake()
        {
            fleeAffectorReduceTimer = gameObject.AddComponent<Timer>();
            fleeAffectorReduceTimer.Set(fleeAffectorReduceRate);
        }

        private void OnEnable()
        {
            fleeAffectorReduceTimer.TimerCompleted += ReduceFleeAffector;
        }

        private void OnDisable()
        {
            fleeAffectorReduceTimer.TimerCompleted -= ReduceFleeAffector;
        }

        public void ResetProgress()
        {
            fleeAffectorReduceRate = 2f;
            enemiesKilledFleeAffector = 0;
            EnemiesKilledAmount = 0;
        }

        private void ReduceFleeAffector()
        {
            if (EnemiesKilledFleeAffector > 0)
            {
                EnemiesKilledFleeAffector--;
            }
        }
    }
}