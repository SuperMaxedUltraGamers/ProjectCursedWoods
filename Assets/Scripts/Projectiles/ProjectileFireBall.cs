﻿using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class ProjectileFireBall : ProjectileBase
    {
        /// <summary>
        /// Used to prevent multiple triggers.
        /// </summary>
        private bool hasTriggered;
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
            particles.SetActive(true);
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
            particles.SetActive(false);
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
        /// The way fireball behaves when launching.
        /// </summary>
        private void Launch()
        {
            isMoving = true;
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.PlayerSFX.Fireball);
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
                    other.GetComponent<IHealth>().DecreaseHealth(DamageAmount, DamageType);
                    OnHit();
                }
                else if (otherLayer == GlobalVariables.PLAYER_MELEE_LAYER)
                {
                    transform.rotation = GameMan.Instance.PlayerT.rotation;
                }
                else
                {
                    OnHit();
                }
            }
        }
    }
}