using UnityEngine;

namespace CursedWoods.Utils
{
    public class EnemySpawnerOnInteraction : EnemySpawnerBase
    {
        [SerializeField]
        private InteractionBase interaction;

        private void OnEnable()
        {
            interaction.Interacted += StartSpawning;
        }

        private void OnDisable()
        {
            interaction.Interacted -= StartSpawning;
        }

        private void StartSpawning()
        {
            isSpawning = true;
            CheckSpawnSpot();
        }
    }
}