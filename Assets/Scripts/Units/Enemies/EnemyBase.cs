﻿using UnityEngine;

namespace CursedWoods
{
    public abstract class EnemyBase : UnitPoolable
    {
        [SerializeField, Tooltip("Minimum distance from player to start attacking.")]
        protected float attackRange = 0.5f;

        [SerializeField, Tooltip("If we are closer than this to player, the enemy tries to increase the distance.")]
        protected float minComfortRange = 0f;

        //[SerializeField, Tooltip("If we are further than this to player, the enemy tries to decrease the distance.")]
        //protected float maxComfortRange = 0f;

        [SerializeField, Range(0, 100), Tooltip("How easily this enemy flees when other attacking enemy is killed. 0 never flees, 100 always flees.")]
        protected int cowardnessValue = 50;

        protected EnemyBehaviours currentBehaviour = EnemyBehaviours.Idle;

        private void OnEnable()
        {
            GameMan.Instance.AIManager.EnemyGotKilled += CheckFleePossibility;
            HealthChanged += TookDamage;
            Staggered += GotKnockedBack;
        }

        /// <summary>
        /// For changing enemy's state upon taking damage if not in combat states.
        /// This is subscribed to HealthChanged event.
        /// </summary>
        /// <param name="dmg">The amount of damage we took.</param>
        protected abstract void TookDamage(int dmg);

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
    }
}