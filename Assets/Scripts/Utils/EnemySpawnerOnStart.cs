namespace CursedWoods.Utils
{
    public class EnemySpawnerOnStart : EnemySpawnerBase
    {
        protected override void Start()
        {
            base.Start();
            isSpawning = true;
        }
    }
}