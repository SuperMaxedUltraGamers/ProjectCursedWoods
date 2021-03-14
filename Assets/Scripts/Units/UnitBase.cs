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
        #region Private fields

        /// <summary>
        /// Damage resistance values against different damage types.
        /// </summary>
        [SerializeField]
        private DamageResistanceInfo[] dmgResInfo;

        /// <summary>
        /// Table for damage types and their resistances.
        /// </summary>
        private Dictionary<DamageType, int> dmgResistances = new Dictionary<DamageType, int>();

        [SerializeField, Tooltip("The default/starting health, set to Max Health value if larger.")]
        private int startingHealth = 100;

        /// <summary>
        /// Unit's current maximum health.
        /// </summary>
        [SerializeField]
        private int maxHealth;

        /// <summary>
        /// Unit's default maximum health, used if reseting the unit back to it's default values.
        /// </summary>
        private int startingMaxHealth;

        /// <summary>
        /// If unit's health is reduced to this or to a lower value, the unit dies.
        /// </summary>
        [SerializeField]
        private int minHealth;

        [SerializeField, Tooltip("If health is decreased more than this value then this character gets knocked back/staggered.")]
        private int minCauseStagger;

        /// <summary>
        /// Unit's default minimmum Cause Stagger value, used if reseting the unit back to it's default values.
        /// </summary>
        private int startingMinCauseStagger;

        #endregion Private fields

        #region Properties

        public int CurrentHealth { get; private set; }

        public int MaxHealth { get { return maxHealth; } private set { maxHealth = value; } }

        public int MinHealth { get { return minHealth; } private set { minHealth = value; } }

        public int MinCauseStagger { get { return minCauseStagger; } set { minCauseStagger = value; } }

        public bool IsImmortal { get; set; }

        #endregion Properties

        #region Events

        // This is for notifying UI and informing enemies if they took damage.
        public event Action<int> HealthChanged;

        // This is to fire knockback/stagger.
        public event Action Staggered;

        #endregion Events

        #region Unity messages

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

        #endregion Unity messages

        #region Public API

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

            if (HealthChanged != null)
            {
                HealthChanged(CurrentHealth);
            }
        }

        public void DecreaseHealth(int amount, DamageType damageType)
        {
            if (!IsImmortal)
            {
                print($"health before hit: {CurrentHealth}");
                if (!dmgResistances.TryGetValue(damageType, out int resistance))
                {
                    print($"Unit had no resistance set against damagetype: {damageType} resistance left at 0%");
                }

                int dmgAmount = amount - amount * resistance / 100;
                if (CurrentHealth - dmgAmount < MinHealth)
                {
                    CurrentHealth = MinHealth;
                    if (HealthChanged != null)
                    {
                        HealthChanged(CurrentHealth);
                    }

                    Die();
                    print("dead");
                }
                else
                {
                    CurrentHealth -= dmgAmount;
                    if (dmgAmount > MinCauseStagger)
                    {
                        if (Staggered != null)
                        {
                            Staggered();
                        }
                    }

                    if (HealthChanged != null)
                    {
                        HealthChanged(CurrentHealth);
                    }

                    print($"health after hit: {CurrentHealth}");
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

        public virtual void ResetValues()
        {
            CurrentHealth = startingHealth;
            MinCauseStagger = startingMinCauseStagger;
            MaxHealth = startingMaxHealth;
        }

        #endregion Public API

        #region Protected API

        /// <summary>
        /// What happens when unit runs out of HP.
        /// </summary>
        protected abstract void Die();

        #endregion Protected API
    }
}