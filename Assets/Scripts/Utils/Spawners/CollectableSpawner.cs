using UnityEngine;

namespace CursedWoods.Utils
{
    // TODO: rename to MaxHealthIncreaseSpawner since this is only spawning them now.
    public class CollectableSpawner : MonoBehaviour
    {
        //[SerializeField]
        //private ObjectPoolType collectable;
        [SerializeField]
        private bool spawnOnStart;
        [SerializeField]
        private int spawnerID = 0;

        private void Start()
        {
            if (spawnOnStart)
            {
                SpawnCollectable();
            }
        }

        public void SpawnCollectable()
        {
            if (GameMan.Instance.GraveyardManager.GetMaxHealthIncreaseSpawnerStatus(spawnerID))
            {
                //IPoolObject maxHealthIncrease = GameMan.Instance.ObjPoolMan.GetObjectFromPool(collectable);
                MaxHealthIncrease maxHealthIncrease = (MaxHealthIncrease)GameMan.Instance.ObjPoolMan.GetObjectFromPool(ObjectPoolType.MaxHealthIncrease);
                maxHealthIncrease.Activate(transform.position, transform.rotation);
                maxHealthIncrease.SetSpawnerID(spawnerID);
            }
        }
    }
}