using UnityEngine;

namespace CursedWoods
{
    public class ProjectileBase : PoolObjectBase, ICauseDamage
    {
        [SerializeField]
        protected float projectileSpeed = 10f;

        [SerializeField]
        protected AudioSource audioSource;

        public DamageType DamageType { get; protected set; }

        public int DamageAmount { get; protected set; }

        public int OgDamageAmount { get; private set; }

        protected virtual void Awake()
        {
            gameObject.SetActive(false);
        }

        public void InitDamageInfo(int damageAmount, DamageType damageType)
        {
            DamageAmount = damageAmount;
            DamageType = damageType;
            OgDamageAmount = DamageAmount;
        }
    }
}