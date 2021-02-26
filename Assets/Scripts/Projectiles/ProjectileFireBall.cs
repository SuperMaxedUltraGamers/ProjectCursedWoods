using UnityEngine;

namespace CursedWoods
{
    public class ProjectileFireBall : ProjectileBase
    {
        /// <summary>
        /// Used to prevent multiple triggers.
        /// </summary>
        private bool hasTriggered;

        /// <summary>
        /// What happens when the fireball hits something.
        /// </summary>
        private void OnHit()
        {
            rb.velocity = Vector3.zero;
            hasTriggered = true;
            Deactivate();
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot);
            hasTriggered = false;
            Launch();
        }

        /// <summary>
        /// The way fireball behaves when launching.
        /// </summary>
        private void Launch()
        {
            rb.velocity = transform.forward * projectileSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!hasTriggered)
            {
                // TODO: affect enemies
                OnHit();
            }
        }
    }
}