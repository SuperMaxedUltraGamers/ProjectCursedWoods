using UnityEngine;
using CursedWoods.Utils;
using System.Collections;

namespace CursedWoods
{
    public class PosTreeProjectile : ProjectileBase
    {
        /// <summary>
        /// Used to prevent multiple triggers.
        /// </summary>
        private bool isHit;

        private bool isMoving;

        /// <summary>
        /// How long does the projectile live if no collision happens.
        /// </summary>
        private float lifeTime = 6f;

        private Collider hitbox;
        private Timer lifeTimeTimer;
        private MeshRenderer meshRenderer;
        private ParticleSystem hitParticles;
        private float hitParticleSFXLength = 2f;

        protected override void Awake()
        {
            base.Awake();
            hitbox = GetComponent<Collider>();
            lifeTimeTimer = gameObject.AddComponent<Timer>();
            lifeTimeTimer.Set(lifeTime);
            hitParticles = GetComponentInChildren<ParticleSystem>();
            meshRenderer = GetComponentInChildren<MeshRenderer>();
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
            isHit = false;
            hitbox.enabled = true;
            meshRenderer.enabled = true;
            gameObject.layer = GlobalVariables.ENEMY_PROJECTILE_LAYER;
            lifeTimeTimer.Run();
            Launch();
        }

        /// <summary>
        /// What happens when the fireball hits something.
        /// </summary>
        public override void OnHit()
        {
            isHit = true;
            isMoving = false;
            hitbox.enabled = false;
            meshRenderer.enabled = false;
            lifeTimeTimer.Stop();
            lifeTimeTimer.Set(hitParticleSFXLength);
            lifeTimeTimer.Run();
            hitParticles.Play();
            //Deactivate();
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
            if (!isHit)
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
                    gameObject.layer = GlobalVariables.PLAYER_PROJECTILE_LAYER;
                    transform.rotation = GameMan.Instance.PlayerT.rotation;
                    StartCoroutine(DmgChange());
                }
                else
                {
                    OnHit();
                }
            }
        }

        private IEnumerator DmgChange()
        {
            yield return 0;
            if (!isHit)
            {
                DamageAmount *= 5;
            }
        }
    }
}