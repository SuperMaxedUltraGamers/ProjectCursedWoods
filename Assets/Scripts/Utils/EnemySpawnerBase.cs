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
        protected Transform spawnPos;

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
        }

        protected void CheckSpawnSpot()
        {
            if (!Physics.CheckSphere(spawnPos.position, 0.5f))
            {
                Spawn(spawnPos.position, spawnPos.rotation);
            }
            else if (!Physics.CheckSphere(spawnPos.position + spawnPos.right * 1.2f, 0.5f))
            {
                Spawn(spawnPos.position + spawnPos.right, spawnPos.rotation);
            }
            else if (!Physics.CheckSphere(spawnPos.position - spawnPos.right * 1.2f, 0.5f))
            {
                Spawn(spawnPos.position - spawnPos.right, spawnPos.rotation);
            }
            else
            {
                currentTime = 0f;
            }
        }

        protected void Spawn(Vector3 pos, Quaternion rot)
        {
            switch (enemyType)
            {
                case EnemyType.SkeletonMelee:
                    objectPoolType = ObjectPoolType.SkeletonMelee;
                    break;
                case EnemyType.PossessedTree:
                    objectPoolType = ObjectPoolType.PossessedTree;
                    break;
            }

            IPoolObject enemy = GameMan.Instance.ObjPoolMan.GetObjectFromPool(objectPoolType);
            enemy.Activate(pos, rot);

            spawnAmount--;
            currentTime = 0f;
        }
    }
}