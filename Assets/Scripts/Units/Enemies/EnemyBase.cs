using UnityEngine;

namespace CursedWoods
{
    public abstract class EnemyBase : UnitPoolable
    {
        #region Protected fields

        /// <summary>
        /// Minimum distance from player when to start attacking, also determinates the reach the of attack.
        /// </summary>
        [SerializeField, Tooltip("Minimum distance from player when to start attacking, also determinates the reach the of attack.")]
        protected float attackRange = 0.5f;

        /// <summary>
        /// Determinates how close the enemy wants to be to player at minimum.
        /// </summary>
        [SerializeField, Tooltip("If we are closer than this to player, the enemy tries to increase the distance.")]
        protected float minComfortRange = 0f;

        /// <summary>
        /// Determinates how easily enemy can flee from combat if other enemies dies on combat.
        /// </summary>
        [SerializeField, Range(0, 100), Tooltip("How easily this enemy flees when other attacking enemy is killed. 0 never flees, 100 always flees.")]
        protected int cowardnessValue = 50;

        /// <summary>
        /// Determinates how enemy currently behaves. 
        /// </summary>
        protected EnemyBehaviours currentBehaviour = EnemyBehaviours.Idle;

        #endregion Protected fields

        #region Unity messages

        private void OnEnable()
        {
            GameMan.Instance.AIManager.EnemyGotKilled += CheckFleePossibility;
            HealthChanged += TookDamage;
            Staggered += GotKnockedBack;
        }

        #endregion Unity messages

        #region Protected API

        /// <summary>
        /// For changing enemy's state upon taking damage if not in combat states.
        /// This is subscribed to HealthChanged event.
        /// </summary>
        /// <param name="currentHealth">The new/current health after taking damage.</param>
        /// <param name="maxHealth">The current maximum health of the unit.</param>
        protected abstract void TookDamage(int currentHealth, int maxHealth);

        /// <summary>
        /// For changing enemy's state when getting knocked back.
        /// This is subscribed to Staggered event.
        protected abstract void GotKnockedBack();

        /// <summary>
        /// Used to check if this unit's state should be set to flee state.
        /// </summary>
        /// <param name="fleeAffectorValue"></param>
        protected abstract void CheckFleePossibility(int fleeAffectorValue);

        protected override void Die()
        {
            currentBehaviour = EnemyBehaviours.Dead;
            GameMan.Instance.AIManager.EnemyGotKilled -= CheckFleePossibility;
            HealthChanged -= TookDamage;
            Staggered -= GotKnockedBack;

            GameMan.Instance.AIManager.EnemiesKilledFleeAffector++;
        }

        #endregion Protected API
    }
}