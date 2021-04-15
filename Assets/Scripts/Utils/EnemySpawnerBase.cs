using UnityEngine;

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

        protected virtual void Start()
        {
            int layerMask = 1 << 0;
            spawnPos = spawnPosT.position;

            RaycastHit hit;
            switch (enemyType)
            {
                case EnemyType.SkeletonMelee:
                    objectPoolType = ObjectPoolType.SkeletonMelee;
                    if (Physics.Raycast(spawnPos, -Vector3.up, out hit, maxLinecastDistance, layerMask))
                    {
                        ySpawnOffset = hit.distance - 0.01f;
                    }
                    else
                    {
                        ySpawnOffset = maxLinecastDistance;
                    }

                    spawnSpaceRadius = 0.5f;
                    break;
                case EnemyType.PossessedTree:
                    objectPoolType = ObjectPoolType.PossessedTree;
                    if (Physics.Raycast(spawnPos, -Vector3.up, out hit, maxLinecastDistance, layerMask))
                    {
                        ySpawnOffset = hit.distance;
                    }
                    else
                    {
                        ySpawnOffset = maxLinecastDistance;
                    }

                    spawnSpaceRadius = 0.6f;
                    break;
                case EnemyType.MushroomEnemy:
                    objectPoolType = ObjectPoolType.MushroomEnemy;
                    if (Physics.Raycast(spawnPos, -Vector3.up, out hit, maxLinecastDistance, layerMask))
                    {
                        ySpawnOffset = hit.distance;
                    }
                    else
                    {
                        ySpawnOffset = maxLinecastDistance;
                    }

                    spawnSpaceRadius = 0.6f;
                    break;
                case EnemyType.SkeletonBoss:
                    objectPoolType = ObjectPoolType.SkeletonBoss;
                    if (Physics.Raycast(spawnPos, -Vector3.up, out hit, maxLinecastDistance, layerMask))
                    {
                        ySpawnOffset = hit.distance - 0.01f;
                    }
                    else
                    {
                        ySpawnOffset = maxLinecastDistance;
                    }

                    spawnSpaceRadius = 0.75f;
                    break;
                case EnemyType.TreeBoss:
                    objectPoolType = ObjectPoolType.TreeBoss;
                    if (Physics.Raycast(spawnPos, -Vector3.up, out hit, maxLinecastDistance, layerMask))
                    {
                        ySpawnOffset = hit.distance - 0.01f;
                    }
                    else
                    {
                        ySpawnOffset = maxLinecastDistance;
                    }

                    spawnSpaceRadius = 2f;
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
    }
}