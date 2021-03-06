﻿using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public class ObjectPoolManager : MonoBehaviour
    {
        #region Private fields

        /// <summary>
        /// Used to store all the pools into one dictionary so that we can easily retrieve the correct type of objects.
        /// </summary>
        private Dictionary<ObjectPoolType, IObjectPool> poolByType = new Dictionary<ObjectPoolType, IObjectPool>();

        /// <summary>
        /// Pool for fireball prefab objects.
        /// </summary>
        [SerializeField]
        private FireBallPool fireBallPool = null;

        /// <summary>
        /// Pool for shockwave prefab objects.
        /// </summary>
        [SerializeField]
        private ShockwavePool shockwavePool = null;

        /// <summary>
        /// Pool for iceray prefab objects.
        /// </summary>
        [SerializeField]
        private IceRayPool iceRayPool = null;

        /// <summary>
        /// Pool for magic beam prefab objects.
        /// </summary>
        [SerializeField]
        private MagicBeamPool magicBeamPool = null;

        [SerializeField]
        private SkeletonMeleePool skeletonMeleePool = null;

        [SerializeField]
        private PossessedTreePool possessedTreePool = null;

        [SerializeField]
        private MushroomEnemyPool mushroomEnemyPool = null;

        [SerializeField]
        private SkeletonBossPool skeletonBoss1Pool = null;
        [SerializeField]
        private SkeletonBossPool skeletonBoss2Pool = null;
        [SerializeField]
        private SkeletonBossPool skeletonBoss3Pool = null;

        [SerializeField]
        private TreeBossPool treeBossPool = null;

        [SerializeField]
        private SkeletonMacePool skeletonMacePool = null;

        [SerializeField]
        private FinalBossPool finalBossPool = null;

        [SerializeField]
        private PosTreeProjectilePool posTreeProjectilePool = null;

        [SerializeField]
        private MushroomProjectilePool mushroomProjectilePool = null;

        [SerializeField]
        private FinalBossProjectilePool finalBossProjectilePool = null;

        [SerializeField]
        private HealthPickUpPool healthPickUpPool = null;

        [SerializeField]
        private MaxHealthPickUpPool maxHealthPickUpPool = null;

        [SerializeField]
        private MaxHealthIncreasePool maxHealthIncreasePool = null;

        [SerializeField]
        private DamageNumberPool damageNumberPool = null;

        [SerializeField]
        private MeleeHitParticlePool meleeHitParticlePool = null;

        #endregion Private fields

        #region Unity messages

        private void Awake()
        {
            // Initialized from GameMan now.
            /*
            // Add all the individual pools to the dictionary.
            poolByType.Add(ObjectPoolType.Fireball, fireBallPool);
            poolByType.Add(ObjectPoolType.Shockwave, shockwavePool);
            poolByType.Add(ObjectPoolType.IceRay, iceRayPool);
            poolByType.Add(ObjectPoolType.MagicBeam, magicBeamPool);
            poolByType.Add(ObjectPoolType.SkeletonMelee, skeletonMeleePool);
            poolByType.Add(ObjectPoolType.PossessedTree, possessedTreePool);
            poolByType.Add(ObjectPoolType.TreeProjectile, posTreeProjectilePool);
            poolByType.Add(ObjectPoolType.HealthPickUp, healthPickUpPool);
            poolByType.Add(ObjectPoolType.MaxHealthPickUp, maxHealthPickUpPool);
            poolByType.Add(ObjectPoolType.MaxHealthIncrease, maxHealthIncreasePool);
            poolByType.Add(ObjectPoolType.DamageNumber, damageNumberPool);

            // Populate each pool inside the dictionary.
            foreach (var pool in poolByType)
            {
                pool.Value.CreateObjects();
            }
            */
        }

        #endregion Unity messages

        #region Public API

        /// <summary>
        /// Returns correct type of object from the correct pool via wanted type.
        /// </summary>
        /// <param name="objectPoolType">The type of pool that holds the correct objects we want to receive.</param>
        /// <returns>An object of our wanted pool type.</returns>
        public IPoolObject GetObjectFromPool(ObjectPoolType objectPoolType)
        {
            IPoolObject wantedObject = null;
            if (poolByType.ContainsKey(objectPoolType))
            {
                wantedObject = poolByType[objectPoolType].GetObject();
            }

            return wantedObject;
        }

        public void InitializeGraveyardObjectPool()
        {
            ClearPools();

            // Add all the individual pools to the dictionary.
            poolByType.Add(ObjectPoolType.Fireball, fireBallPool);
            poolByType.Add(ObjectPoolType.Shockwave, shockwavePool);
            poolByType.Add(ObjectPoolType.IceRay, iceRayPool);
            poolByType.Add(ObjectPoolType.MagicBeam, magicBeamPool);
            poolByType.Add(ObjectPoolType.SkeletonMelee, skeletonMeleePool);
            poolByType.Add(ObjectPoolType.PossessedTree, possessedTreePool);
            poolByType.Add(ObjectPoolType.MushroomEnemy, mushroomEnemyPool);
            poolByType.Add(ObjectPoolType.SkeletonBoss1, skeletonBoss1Pool);
            poolByType.Add(ObjectPoolType.SkeletonBoss2, skeletonBoss2Pool);
            poolByType.Add(ObjectPoolType.TreeBoss, treeBossPool);
            poolByType.Add(ObjectPoolType.FinalBoss, finalBossPool);
            poolByType.Add(ObjectPoolType.TreeProjectile, posTreeProjectilePool);
            poolByType.Add(ObjectPoolType.MushroomProjectile, mushroomProjectilePool);
            poolByType.Add(ObjectPoolType.FinalBossProjectile, finalBossProjectilePool);
            poolByType.Add(ObjectPoolType.HealthPickUp, healthPickUpPool);
            poolByType.Add(ObjectPoolType.MaxHealthPickUp, maxHealthPickUpPool);
            poolByType.Add(ObjectPoolType.MaxHealthIncrease, maxHealthIncreasePool);
            poolByType.Add(ObjectPoolType.DamageNumber, damageNumberPool);
            poolByType.Add(ObjectPoolType.MeleeHitParticles, meleeHitParticlePool);
            poolByType.Add(ObjectPoolType.SkeletonMace, skeletonMacePool);

            // Populate each pool inside the dictionary.
            foreach (var pool in poolByType)
            {
                pool.Value.CreateObjects();
            }
        }

        public void InitializeCastleObjectPool()
        {
            ClearPools();

            // Add all the individual pools to the dictionary.
            poolByType.Add(ObjectPoolType.Fireball, fireBallPool);
            poolByType.Add(ObjectPoolType.Shockwave, shockwavePool);
            poolByType.Add(ObjectPoolType.IceRay, iceRayPool);
            poolByType.Add(ObjectPoolType.MagicBeam, magicBeamPool);
            poolByType.Add(ObjectPoolType.SkeletonMelee, skeletonMeleePool);
            //poolByType.Add(ObjectPoolType.PossessedTree, possessedTreePool);
            //poolByType.Add(ObjectPoolType.MushroomEnemy, mushroomEnemyPool);
            poolByType.Add(ObjectPoolType.SkeletonMace, skeletonMacePool);
            //poolByType.Add(ObjectPoolType.SkeletonBoss1, skeletonBossPool);
            poolByType.Add(ObjectPoolType.SkeletonBoss3, skeletonBoss3Pool);
            poolByType.Add(ObjectPoolType.FinalBoss, finalBossPool);
            //poolByType.Add(ObjectPoolType.TreeProjectile, posTreeProjectilePool);
            //poolByType.Add(ObjectPoolType.MushroomProjectile, mushroomProjectilePool);
            poolByType.Add(ObjectPoolType.FinalBossProjectile, finalBossProjectilePool);
            poolByType.Add(ObjectPoolType.HealthPickUp, healthPickUpPool);
            poolByType.Add(ObjectPoolType.MaxHealthPickUp, maxHealthPickUpPool);
            poolByType.Add(ObjectPoolType.MaxHealthIncrease, maxHealthIncreasePool);
            poolByType.Add(ObjectPoolType.DamageNumber, damageNumberPool);
            poolByType.Add(ObjectPoolType.MeleeHitParticles, meleeHitParticlePool);

            // Populate each pool inside the dictionary.
            foreach (var pool in poolByType)
            {
                pool.Value.CreateObjects();
            }
        }

        public void InitializeMainMenuObjectPool()
        {
            ClearPools();
        }

        #endregion Public API

        #region Private functionality

        private void ClearPools()
        {
            // Clear pools.
            foreach (var pool in poolByType)
            {
                pool.Value.ClearPool();
            }

            poolByType.Clear();
        }

        #endregion Private functionality
    }
}