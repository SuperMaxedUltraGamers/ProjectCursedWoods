using UnityEngine;

namespace CursedWoods.Utils
{
    public class CollectableSpawner : MonoBehaviour
    {
        [SerializeField]
        private ObjectPoolType collectable;
        [SerializeField]
        private bool spawnOnStart;

        private void Start()
        {
            if (spawnOnStart)
            {
                SpawnCollectable();
            }
        }

        public void SpawnCollectable()
        {
            IPoolObject maxHealthIncrease = GameMan.Instance.ObjPoolMan.GetObjectFromPool(collectable);
            maxHealthIncrease.Activate(transform.position, transform.rotation);
        }
    }
}