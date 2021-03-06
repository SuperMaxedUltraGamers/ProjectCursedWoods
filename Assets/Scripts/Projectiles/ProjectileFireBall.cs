using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class ProjectileFireBall : ProjectileBase
    {
        /// <summary>
        /// Used to prevent multiple triggers.
        /// </summary>
        private bool hasTriggered;

        /// <summary>
        /// How long does the fireball live if no collision happens.
        /// </summary>
        private float lifeTime = 3f;

        private Timer lifeTimeTimer;

        protected override void Awake()
        {
            base.Awake();
            lifeTimeTimer = gameObject.AddComponent<Timer>();
            lifeTimeTimer.Set(lifeTime);
            //print("asdasd");
        }

        private void OnEnable()
        {
            lifeTimeTimer.TimerCompleted += LifeTimeOver;
        }

        private void OnDisable()
        {
            if (lifeTimeTimer != null)
            {
                lifeTimeTimer.TimerCompleted -= LifeTimeOver;
            }
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot);
            hasTriggered = false;
            lifeTimeTimer.Run();
            Launch();
        }

        /// <summary>
        /// What happens when the fireball hits something.
        /// </summary>
        private void OnHit()
        {
            rb.velocity = Vector3.zero;
            hasTriggered = true;
            lifeTimeTimer.Stop();
            Deactivate();
        }

        private void LifeTimeOver()
        {
            rb.velocity = Vector3.zero;
            Deactivate();
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
                string otherTag = other.gameObject.tag;
                if (otherTag.Equals(GlobalVariables.ENEMY_TAG))
                {
                    other.GetComponent<IHealth>().DecreaseHealth(DamageAmount, DamageType);
                    OnHit();
                }
                else if (otherTag.Equals(GlobalVariables.MELEE_WEAPON_TAG))
                {
                    transform.rotation = other.gameObject.transform.rotation;
                    rb.velocity = transform.forward * projectileSpeed;
                }
                else
                {
                    OnHit();
                }
            }
        }
    }
}