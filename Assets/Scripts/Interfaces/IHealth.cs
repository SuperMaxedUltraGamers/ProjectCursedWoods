namespace CursedWoods
{
    public interface IHealth
    {
        /// <summary>
        /// This event is triggered every time the CurrentHealth changes.
        /// </summary>
        event System.Action<int, int> HealthChanged;

        /// <summary>
        /// If character takes massive damage, this causes them to knockback/staggger.
        /// </summary>
        event System.Action Staggered;

        /// <summary>
        /// Returns character's current health.
        /// </summary>
        int CurrentHealth { get; }

        /// <summary>
        /// Health's maximum amount. CurrentHealth can never exceed this.
        /// </summary>
        int MaxHealth { get; }

        /// <summary>
        /// Health's minimum amount. CurrentHealth can never go beneath this.
        /// </summary>
        int MinHealth { get; }

        /// <summary>
        /// If health is decreased more than this value then invoke Staggered event.
        /// </summary>
        int MinCauseStagger { get; set; }

        /// <summary>
        /// Indicates weather the character is immortal atm. or not. Is the character
        /// is immortal, CurrentHealth can't be reduced even though the character
        /// is damaged.
        /// </summary>
        bool IsImmortal { get; set; }

        /// <summary>
        /// Increases the CurrentHealth by the amount. CurrentHealth can never exceed MaxHealth.
        /// </summary>
        /// <param name="amount">The amount CurrentHealth is increased by.</param>
        void IncreaseHealth(int amount);

        /// <summary>
        /// Decreases the CurrentHealth by the amount. CurrentHealth can never go beneath
        /// MinHealth.
        /// </summary>
        /// <param name="amount">The amount CurrentHealth is decreased by.</param>
        /// <param name="damageType">The type of damage we took.</param>
        void DecreaseHealth(int amount, DamageType damageType);

        /// <summary>
        /// Increases the maximum health with the given amount.
        /// </summary>
        /// <param name="amount">The amount maximum health is increased.</param>
        void IncreaseMaxHealth(int amount);

        /// <summary>
        /// Decreases the maximum health with the given amount.
        /// If CurrentHealth is larger than the new decreased MaxHealth then it is also decreased to the new MaxHealth amount.
        /// </summary>
        /// <param name="amount">The amount maximum health is decreased.</param>
        void DecreaseMaxHealth(int amount);

        /// <summary>
        /// Resets component's values to their original state.
        /// </summary>
        void ResetValues();
    }
}