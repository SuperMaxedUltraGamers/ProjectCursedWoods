using System;
using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    [Serializable]
    public struct DamageResistanceInfo
    {
        /// <summary>
        /// The type of damage we want to set resistance against.
        /// </summary>
        [SerializeField]
        private DamageType type;

        /// <summary>
        /// The amount of resistance we have against this DamageType in percents.
        /// </summary>
        [SerializeField, Range(0, 100), Tooltip("Resistance we have against this damage type in percents. 100 means no damage will be taken")]
        private int resistance;

        /// <summary>
        /// Property for for damage type.
        /// </summary>
        public DamageType Type { get { return type; } }

        /// <summary>
        /// Property for the resistance amount.
        /// </summary>
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

        /// <summary>
        /// The health amount the unit starts with.
        /// </summary>
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

        /// <summary>
        /// The minimum damage amount we have to take to cause knockback/stagger.
        /// </summary>
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

        /// <summary>
        /// Event for notifying UI when CurrentHealth or MaxHealth is changed and informing enemies if they took damage.
        /// </summary>
        public event Action<int, int> HealthChanged;

        /// <summary>
        /// Event for firing knockback/stagger behviour.
        /// </summary>
        public event Action Staggered;

        #endregion Events

        #region Unity messages

        protected virtual void Awake()
        {
            // Go through the damage resitance info that is set in the inspector and assign those values to dictionary.
            foreach (DamageResistanceInfo info in dmgResInfo)
            {
                dmgResistances.Add(info.Type, info.Resistance);
            }

            // Make sure that startingHealth does not exceed maxHealth.
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
            // Make sure CurrentHealth does not exceed MaxHealth.
            if (CurrentHealth + amount > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
            else
            {
                CurrentHealth += amount;
            }

            // Invoke HealtChanged event if it has subscribers.
            InvokeHealthChangedEvent();
        }

        public void DecreaseHealth(int amount, DamageType damageType)
        {
            // Check if unit is immortal and no damage should be taken.
            if (!IsImmortal)
            {
                print($"health before hit: {CurrentHealth}");

                // Check if we have resistance value set against the taken DamageType.
                if (!dmgResistances.TryGetValue(damageType, out int resistance))
                {
                    print($"Unit had no resistance set against damagetype: {damageType} resistance left at 0%");
                }

                // Calculate the real damage amount we take after resitance is taken into account.
                int dmgAmount = amount - amount * resistance / 100;

                // Check if we die from the taken damage or just reduce health.
                if (CurrentHealth - dmgAmount < MinHealth)
                {
                    CurrentHealth = MinHealth;

                    // Invoke HealtChanged event if it has subscribers.
                    InvokeHealthChangedEvent();

                    // Call the abstract Die method.
                    Die();
                    print("dead");
                }
                else
                {
                    CurrentHealth -= dmgAmount;

                    // Check if the taken damage is large enough to cause the unit to stagger/knockback.
                    if (dmgAmount > MinCauseStagger)
                    {
                        // Invoke Staggered event if it has subscribers.
                        if (Staggered != null)
                        {
                            Staggered();
                        }
                    }

                    // Invoke HealtChanged event if it has subscribers.
                    InvokeHealthChangedEvent();
                    print($"health after hit: {CurrentHealth}");
                }
            }
        }

        public void IncreaseMaxHealth(int amount)
        {
            MaxHealth += amount;

            // Invoke HealtChanged event if it has subscribers.
            InvokeHealthChangedEvent();
        }

        public void DecreaseMaxHealth(int amount)
        {
            MaxHealth -= amount;

            // Make sure that CurrentHealth does not exceed MaxHealth.
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

            // Invoke HealtChanged event if it has subscribers.
            InvokeHealthChangedEvent();
        }

        public virtual void ResetValues()
        {
            CurrentHealth = startingHealth;
            MinCauseStagger = startingMinCauseStagger;
            MaxHealth = startingMaxHealth;
        }

        #endregion Public API

        #region Protected functionality

        /// <summary>
        /// What happens when unit runs out of HP.
        /// </summary>
        protected abstract void Die();

        #endregion Protected functionality

        #region Private functionality

        /// <summary>
        /// Hard-coded invoke for HealthChanged event with correct parameters.
        /// </summary>
        private void InvokeHealthChangedEvent()
        {
            if (HealthChanged != null)
            {
                HealthChanged(CurrentHealth, MaxHealth);
            }
        }

        #endregion Private functionality
    }
}