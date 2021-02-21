using UnityEngine;

namespace CursedWoods
{
    public class ProjectileBase : MonoBehaviour, IProjectile
    {
        protected Rigidbody rb;
        [SerializeField]
        private float projectileSpeed = 10f;
        protected bool isDestroyed = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public virtual void OnHit()
        {
            Destroy(gameObject);
        }

        public virtual void Launch()
        {
            rb.velocity = transform.forward * projectileSpeed;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!isDestroyed)
            {
                isDestroyed = true;
                OnHit();
            }
        }
    }
}