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
        private int enemiesKilledFleeAffector;

        public event Action<int> EnemyGotKilled;

        public int EnemiesAttackingAmount { get; set; }
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
                    if (EnemyGotKilled != null)
                    {
                        EnemyGotKilled(value);
                    }
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

        private void ReduceFleeAffector()
        {
            if (EnemiesKilledFleeAffector > 0)
            {
                EnemiesKilledFleeAffector--;
            }
        }
    }
}