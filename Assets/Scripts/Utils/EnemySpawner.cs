using UnityEngine;

namespace CursedWoods.Utils
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField, Tooltip("IMPORTANT! Only choose enemies even though the list contains more items.")]
        private ObjectPoolType enemyType = ObjectPoolType.SkeletonMelee;

        [SerializeField]
        private int spawnAmount;

        [SerializeField]
        private float spawnInterval;
        private float currentTime;
        private bool isSpawning;

        [SerializeField]
        private bool spawningAtStart;

        [SerializeField]
        private Transform spawnPos;

        private void Awake()
        {
            isSpawning = spawningAtStart;
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
        }

        public void StartSpawning()
        {
            isSpawning = true;
        }

        private void CheckSpawnSpot()
        {
            if (!Physics.CheckSphere(spawnPos.position, 0.5f))
            {
                Spawn(spawnPos.position, transform.rotation);
            }
            else if (!Physics.CheckSphere(spawnPos.position + spawnPos.right * 1.2f, 0.5f))
            {
                Spawn(spawnPos.position + spawnPos.right, transform.rotation);
            }
            else if (!Physics.CheckSphere(spawnPos.position - spawnPos.right * 1.2f, 0.5f))
            {
                Spawn(spawnPos.position - spawnPos.right, transform.rotation);
            }
            else
            {
                currentTime = 0f;
            }
        }

        private void Spawn(Vector3 pos, Quaternion rot)
        {
            IPoolObject enemy = GameMan.Instance.ObjPoolMan.GetObjectFromPool(enemyType);
            enemy.Activate(pos, rot);

            spawnAmount--;
            currentTime = 0f;
        }
    }
}