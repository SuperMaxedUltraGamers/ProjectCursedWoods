using UnityEngine;

namespace CursedWoods.Utils
{
    public class CollectableSpawner : MonoBehaviour
    {
        private void Start()
        {
            IPoolObject maxHealthIncrease = GameMan.Instance.ObjPoolMan.GetObjectFromPool(ObjectPoolType.MaxHealthIncrease);
            maxHealthIncrease.Activate(transform.position, transform.rotation);
        }
    }
}