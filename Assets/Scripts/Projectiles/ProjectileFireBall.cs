using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class ProjectileFireBall : ProjectileBase
    {
        /// <summary>
        /// Used to prevent multiple triggers.
        /// </summary>
        private bool isHit;
        private bool isMoving;

        /// <summary>
        /// How long does the fireball live if no collision happens.
        /// </summary>
        private float lifeTime = 4f;
        private Timer lifeTimeTimer;

        [SerializeField]
        private GameObject particles;
        [SerializeField]
        private ParticleSystem hitParticles;
        private float hitParticleSFXLength = 2f;
        private Collider hitBox;

        private AudioSource audioSource;

        protected override void Awake()
        {
            base.Awake();
            hitBox = GetComponent<Collider>();
            audioSource = GetComponent<AudioSource>();
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
            isHit = false;
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
            particles.SetActive(false);
            lifeTimeTimer.Set(hitParticleSFXLength);
            lifeTimeTimer.Run();
            hitParticles.Play();
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.PlayerSFX.FireballHit);
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
        /// The way fireball behaves when launching.
        /// </summary>
        private void Launch()
        {
            isMoving = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isHit)
            {
                /*
                string otherTag = other.gameObject.tag;
                if (otherTag.Equals(GlobalVariables.ENEMY_TAG))
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
                if (otherLayer == GlobalVariables.ENEMY_LAYER)
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
                /*
                else if (otherLayer == GlobalVariables.PLAYER_MELEE_LAYER)
                {
                    gameObject.layer = GlobalVariables.PLAYER_PROJECTILE_LAYER;
                    transform.rotation = GameMan.Instance.PlayerT.rotation;
                    DamageAmount *= 5;
                }
                */
                else
                {
                    OnHit();
                    hitBox.enabled = false;
                }
            }
        }
    }
}