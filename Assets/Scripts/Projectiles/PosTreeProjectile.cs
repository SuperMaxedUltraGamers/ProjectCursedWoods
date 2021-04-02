using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class PosTreeProjectile : ProjectileBase
    {
        /// <summary>
        /// Used to prevent multiple triggers.
        /// </summary>
        private bool hasTriggered;

        private bool isMoving;

        /// <summary>
        /// How long does the projectile live if no collision happens.
        /// </summary>
        private float lifeTime = 6f;

        private Timer lifeTimeTimer;

        protected override void Awake()
        {
            base.Awake();
            lifeTimeTimer = gameObject.AddComponent<Timer>();
            lifeTimeTimer.Set(lifeTime);
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
            hasTriggered = true;
            isMoving = false;
            lifeTimeTimer.Stop();
            Deactivate();
        }

        private void LifeTimeOver()
        {
            Deactivate();
        }

        /// <summary>
        /// The way fireball behaves when launching.
        /// </summary>
        private void Launch()
        {
            isMoving = true;
        }

        private void Update()
        {
            if (isMoving)
            {
                transform.position += transform.forward * projectileSpeed * Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!hasTriggered)
            {
                /*
                string otherTag = other.gameObject.tag;
                if (otherTag.Equals(GlobalVariables.ENEMY_TAG) || otherTag.Equals(GlobalVariables.PLAYER_TAG))
                {
                    other.GetComponent<IHealth>().DecreaseHealth(DamageAmount, DamageType);
                    OnHit();
                }
                else if (otherTag.Equals(GlobalVariables.MELEE_WEAPON_TAG))
                {
                    transform.rotation = other.gameObject.transform.rotation;
                }
                else
                {
                    OnHit();
                }
                */

                int otherLayer = other.gameObject.layer;
                if (otherLayer == GlobalVariables.ENEMY_LAYER || otherLayer == GlobalVariables.PLAYER_LAYER)
                {
                    other.GetComponent<IHealth>().DecreaseHealth(DamageAmount, DamageType);
                    OnHit();
                }
                else if (otherLayer == GlobalVariables.PLAYER_MELEE_LAYER)
                {
                    transform.rotation = GameMan.Instance.PlayerT.rotation;
                    DamageAmount *= 5;
                }
                else
                {
                    OnHit();
                }
            }
        }
    }
}