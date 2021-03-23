using UnityEngine;

namespace CursedWoods
{
    public class ProjectileBase : PoolObjectBase, ICauseDamage
    {
        [SerializeField]
        protected float projectileSpeed = 10f;
        protected AudioSource audioSource;

        public DamageType DamageType { get; protected set; }

        public int DamageAmount { get; protected set; }

        protected virtual void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            gameObject.SetActive(false);
        }

        public void InitDamageInfo(int damageAmount, DamageType damageType)
        {
            DamageAmount = damageAmount;
            DamageType = damageType;
        }
    }
}