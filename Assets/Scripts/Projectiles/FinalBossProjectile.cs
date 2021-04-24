using UnityEngine;
using CursedWoods.Utils;
using System.Collections;

namespace CursedWoods
{
    public class FinalBossProjectile : ProjectileBase
    {
        /// <summary>
        /// Used to prevent multiple triggers.
        /// </summary>
        private bool isHit;
        private bool isMoving;

        /// <summary>
        /// How long does the projectile live if no collision happens.
        /// </summary>
        private float lifeTime = 3.5f;
        private Timer lifeTimeTimer;

        [SerializeField]
        private GameObject particles;
        [SerializeField]
        private ParticleSystem hitParticles;
        private float hitParticleSFXLength = 2f;
        private Collider hitBox;
        private MeshRenderer meshRenderer;

        protected override void Awake()
        {
            base.Awake();
            hitBox = GetComponent<Collider>();
            lifeTimeTimer = gameObject.AddComponent<Timer>();
            lifeTimeTimer.Set(lifeTime);
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;
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

        private void Update()
        {
            if (isMoving)
            {
                transform.position += transform.forward * projectileSpeed * Time.deltaTime;
            }
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot);
            hitBox.enabled = true;
            particles.SetActive(true);
            meshRenderer.enabled = true;
            isHit = false;
            lifeTimeTimer.Run();
            Launch();
        }

        /// <summary>
        /// What happens when the projectile hits something.
        /// </summary>
        public override void OnHit()
        {
            isHit = true;
            isMoving = false;
            particles.SetActive(false);
            meshRenderer.enabled = false;
            lifeTimeTimer.Set(hitParticleSFXLength);
            lifeTimeTimer.Run();
            hitParticles.Play();
            //lifeTimeTimer.Stop();
            //Deactivate();
        }

        private void LifeTimeOver()
        {
            // Does not need to get called since should always be finished by the time SFX has played and lifetime will end.
            //hitParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            Deactivate();
        }

        /// <summary>
        /// The way projectile behaves when launching.
        /// </summary>
        private void Launch()
        {
            isMoving = true;
        }

        private IEnumerator DmgChange()
        {
            yield return 0;
            if (!isHit)
            {
                DamageAmount *= 5;
                lifeTimeTimer.Set(lifeTime);
                lifeTimeTimer.Run();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isHit)
            {
                int otherLayer = other.gameObject.layer;
                if (otherLayer == GlobalVariables.PLAYER_LAYER || otherLayer == GlobalVariables.ENEMY_LAYER)
                {
                    IHealth otherHealth = other.GetComponent<IHealth>();
                    if (otherHealth == null)
                    {
                        otherHealth = other.GetComponentInParent<IHealth>();
                    }

                    otherHealth.DecreaseHealth(DamageAmount, DamageType);
                    OnHit();
                    hitBox.enabled = false;
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
                    hitBox.enabled = false;
                }
            }
        }
    }
}