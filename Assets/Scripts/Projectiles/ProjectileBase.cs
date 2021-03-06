using UnityEngine;

namespace CursedWoods
{
    public class ProjectileBase : PoolObjectBase, ICauseDamage
    {
        protected Rigidbody rb;
        [SerializeField]
        protected float projectileSpeed = 10f;

        public DamageType DamageType { get; protected set; }

        public int DamageAmount { get; protected set; }

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            gameObject.SetActive(false);
        }

        public void InitDamageInfo(int damageAmount, DamageType damageType)
        {
            DamageAmount = damageAmount;
            DamageType = damageType;
        }
    }
}