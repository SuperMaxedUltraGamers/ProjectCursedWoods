using UnityEngine;

namespace CursedWoods.Utils
{
    public class FinalBossProjectileSpawner : MonoBehaviour
    {
        private int dmgAmount;
        private DamageType dmgType;
        private float spawnInterval = 0.4f;
        private float nextSpawnTime;

        private void Update()
        {
            nextSpawnTime -= Time.deltaTime;
            if (nextSpawnTime <= 0f)
            {
                SpawnProjectile();
            }
        }

        public void Initialize(int damageAmount, DamageType damageType)
        {
            nextSpawnTime = 0f;
            dmgAmount = damageAmount;
            dmgType = damageType;
        }

        private void SpawnProjectile()
        {
            FinalBossProjectile projectile = (FinalBossProjectile)GameMan.Instance.ObjPoolMan.GetObjectFromPool(ObjectPoolType.FinalBossProjectile);
            projectile.InitDamageInfo(dmgAmount, dmgType);
            projectile.Activate(transform.position, transform.rotation);
            nextSpawnTime = spawnInterval;
        }
    }
}