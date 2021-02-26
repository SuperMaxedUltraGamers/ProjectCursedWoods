using UnityEngine;

namespace CursedWoods
{
    public class ProjectileBase : PoolObjectBase
    {
        protected Rigidbody rb;
        [SerializeField]
        protected float projectileSpeed = 10f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            gameObject.SetActive(false);
        }
    }
}