﻿using UnityEngine;

namespace CursedWoods.Utils
{
    public class EnemySpawnerBase : MonoBehaviour
    {
        protected ObjectPoolType objectPoolType = ObjectPoolType.SkeletonMelee;

        [SerializeField]
        protected EnemyType enemyType = EnemyType.SkeletonMelee;

        [SerializeField]
        protected int spawnAmount;

        [SerializeField]
        protected float spawnInterval;
        protected float currentTime;
        protected bool isSpawning;

        [SerializeField]
        protected Transform spawnPosT;
        protected Vector3 spawnPos;

        protected float spawnSpaceRadius = 0.5f;
        private float ySpawnOffset;
        private float maxLinecastDistance = 5f;
        private int layerMask;

        protected virtual void Start()
        {
            layerMask = 1 << 0;
            spawnPos = spawnPosT.position;

            switch (enemyType)
            {
                case EnemyType.SkeletonMelee:
                    InitializeSpawning(ObjectPoolType.SkeletonMelee, 0.01f, 0.5f);
                    break;
                case EnemyType.PossessedTree:
                    InitializeSpawning(ObjectPoolType.PossessedTree, 0f, 0.6f);
                    break;
                case EnemyType.MushroomEnemy:
                    InitializeSpawning(ObjectPoolType.MushroomEnemy, 0f, 0.6f);
                    break;
                case EnemyType.SkeletonBoss1:
                    InitializeSpawning(ObjectPoolType.SkeletonBoss1, 0.01f, 0.75f);
                    break;
                case EnemyType.SkeletonBoss2:
                    InitializeSpawning(ObjectPoolType.SkeletonBoss2, 0.01f, 0.75f);
                    break;
                case EnemyType.SkeletonBoss3:
                    InitializeSpawning(ObjectPoolType.SkeletonBoss3, 0.01f, 0.75f);
                    break;
                case EnemyType.TreeBoss:
                    InitializeSpawning(ObjectPoolType.TreeBoss, 0.01f, 2f);
                    break;
                case EnemyType.SkeletonMace:
                    InitializeSpawning(ObjectPoolType.SkeletonMace, 0.01f, 0.5f);
                    break;
                case EnemyType.FinalBoss:
                    InitializeSpawning(ObjectPoolType.FinalBoss, 0.01f, 0.75f);
                    break;
            }
        }

        private void Update()
        {
            if (spawnAmount > 0)
            {
                if (isSpawning)
                {
                    currentTime += Time.deltaTime;
                    if (currentTime >= spawnInterval)
                    {
                        CheckSpawnSpot();
                    }
                }
            }
            else
            {
                this.enabled = false;
            }
        }

        protected void CheckSpawnSpot()
        {
            float horizontalSpawnOffset = spawnSpaceRadius * 2f;
            float horizontalCheckOffset = horizontalSpawnOffset + 0.2f;
            if (!Physics.CheckSphere(spawnPos, spawnSpaceRadius))
            {
                Spawn(spawnPos + -Vector3.up * ySpawnOffset, spawnPosT.rotation);
            }
            else if (!Physics.CheckSphere(spawnPos + spawnPosT.right * horizontalCheckOffset, spawnSpaceRadius))
            {
                Spawn(spawnPos + spawnPosT.right * horizontalSpawnOffset + -Vector3.up * ySpawnOffset, spawnPosT.rotation);
            }
            else if (!Physics.CheckSphere(spawnPos + -spawnPosT.right * horizontalCheckOffset, spawnSpaceRadius))
            {
                Spawn(spawnPos - spawnPosT.right * horizontalSpawnOffset + -Vector3.up * ySpawnOffset, spawnPosT.rotation);
            }
            else
            {
                currentTime = 0f;
            }
        }

        protected void Spawn(Vector3 pos, Quaternion rot)
        {
            IPoolObject enemy = GameMan.Instance.ObjPoolMan.GetObjectFromPool(objectPoolType);
            enemy.Activate(pos, rot);

            spawnAmount--;
            currentTime = 0f;
        }

        private void InitializeSpawning(ObjectPoolType objectPool, float offset, float spawnRadius)
        {
            objectPoolType = objectPool;
            RaycastHit hit;
            if (Physics.Raycast(spawnPos, -Vector3.up, out hit, maxLinecastDistance, layerMask))
            {
                ySpawnOffset = hit.distance - offset;
            }
            else
            {
                ySpawnOffset = maxLinecastDistance;
            }

            spawnSpaceRadius = spawnRadius;
        }
    }
}