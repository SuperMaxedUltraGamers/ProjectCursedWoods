using System;
using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    [Serializable]
    public struct DamageResistanceInfo
    {
        [SerializeField]
        private DamageType type;
        [SerializeField, Range(0, 100), Tooltip("Resistance we have against this damage type in percents. 100 means no damage will be taken")]
        private int resistance;

        public DamageType Type { get { return type; } }
        public int Resistance { get { return resistance; } }
    }

    public abstract class UnitBase : MonoBehaviour, IHealth
    {
        [SerializeField]
        private DamageResistanceInfo[] dmgResInfo;
        private Dictionary<DamageType, int> dmgResistances = new Dictionary<DamageType, int>();

        [SerializeField, Tooltip("The default/starting health, should never be larger than Max Health.")]
        private int startingHealth = 100;

        [SerializeField]
        private int maxHealth;

        private int startingMaxHealth;

        [SerializeField]
        private int minHealth;

        [SerializeField, Tooltip("If health is decreased more than this value then this character gets knocked back/staggered.")]
        private int minCauseStagger;

        private int startingMinCauseStagger;

        public int CurrentHealth { get; private set; }

        public int MaxHealth { get { return maxHealth; } private set { maxHealth = value; } }

        public int MinHealth { get { return minHealth; } private set { minHealth = value; } }

        public int MinCauseStagger { get { return minCauseStagger; } set { minCauseStagger = value; } }

        public bool IsImmortal { get; set; }

        // This is for notifying UI
        public event Action<int> HealthChanged;
        // This is to fire knockback/stagger.
        public event Action Staggered;

        protected virtual void Awake()
        {
            foreach (DamageResistanceInfo info in dmgResInfo)
            {
                dmgResistances.Add(info.Type, info.Resistance);
            }

            if (startingHealth > maxHealth)
            {
                startingHealth = maxHealth;
            }

            CurrentHealth = startingHealth;
            startingMinCauseStagger = minCauseStagger;
            startingMaxHealth = maxHealth;
        }

        public void IncreaseHealth(int amount)
        {
            if (CurrentHealth + amount > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
            else
            {
                CurrentHealth += amount;
            }

            HealthChanged?.Invoke(CurrentHealth);
        }

        public void DecreaseHealth(int amount, DamageType damageType)
        {
            if (!IsImmortal)
            {
                print("health before hit:" + CurrentHealth);
                if (!dmgResistances.TryGetValue(damageType, out int resistance))
                {
                    print("Unit had no resistance set against damagetype: " + damageType + " resistance left 0");
                }

                int dmgAmount = amount - amount * resistance / 100;
                if (CurrentHealth - dmgAmount < MinHealth)
                {
                    CurrentHealth = MinHealth;
                    HealthChanged?.Invoke(CurrentHealth);
                    Die();
                    print("dead");
                }
                else 
                {
                    CurrentHealth -= dmgAmount;
                    if (dmgAmount > MinCauseStagger)
                    {
                        Staggered?.Invoke();
                    }

                    HealthChanged?.Invoke(CurrentHealth);
                    print("health after hit:" + CurrentHealth);
                }
            }
        }

        public void IncreaseMaxHealth(int amount)
        {
            MaxHealth += amount;
        }

        public void DecreaseMaxHealth(int amount)
        {
            MaxHealth -= amount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        public void ResetValues()
        {
            CurrentHealth = startingHealth;
            MinCauseStagger = startingMinCauseStagger;
            MaxHealth = startingMaxHealth;
        }

        protected abstract void Die();
    }
}