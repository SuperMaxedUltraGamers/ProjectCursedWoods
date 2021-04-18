using System;
using UnityEngine;
using CursedWoods.Utils;
using CursedWoods.SaveSystem;

namespace CursedWoods
{
    public class AIManager : MonoBehaviour, ISaveable
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

        public void Save(ISave saveSystem, string keyPrefix)
        {
            saveSystem.SetInt(SaveUtils.GetKey(keyPrefix, SaveUtils.AI_MAN_ENEMIES_KILLED_AMOUNT_KEY), enemiesKilledAmount);
        }

        public void Load(ISave saveSystem, string keyPrefix)
        {
            EnemiesKilledAmount = saveSystem.GetInt(SaveUtils.GetKey(keyPrefix, SaveUtils.AI_MAN_ENEMIES_KILLED_AMOUNT_KEY), 0);
        }

        public void Initialize(ISave saveSystem, string keyPrefix)
        {
            fleeAffectorReduceRate = 2f;
            enemiesKilledFleeAffector = 0;
            Load(saveSystem, keyPrefix);
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