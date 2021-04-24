using UnityEngine;

namespace CursedWoods.Utils
{
    public class EnemySpawnerOnTrigger : EnemySpawnerBase
    {
        private Collider trigger;

        private void Awake()
        {
            trigger = GetComponentInChildren<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckSpawnSpot();
            isSpawning = true;
            trigger.enabled = false;
        }
    }
}