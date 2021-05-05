using CursedWoods.Utils;
using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class MushroomProjectile : ProjectileBase
    {
        private float gravity = 0.1f;
        private float playerDistance;
        private float projectileVerticalSpeed;
        private float projectileLaunchVerticalSpeed;
        private bool isHit;
        private float ogRadius;
        private float scaleSpeed = 1f;
        private float lifeTimeAfterHit = 6f;
        private int areaDamage = 7;
        private DamageType areaDamageType = DamageType.Poison;
        private SphereCollider hitBox;
        private Timer lifeTimeTimer;
        private Timer areaDamageIntervalTimer;
        private float areaDamageInterval = 0.5f;
        private int areaDmgLayerMask;

        [SerializeField]
        private GameObject mesh;
        private ParticleSystem poisonCloud;
        private ParticleSystem.ShapeModule poisonCloudShape;

        private AudioSource audioSource;

        protected override void Awake()
        {
            base.Awake();
            hitBox = GetComponent<SphereCollider>();
            ogRadius = hitBox.radius;
            lifeTimeTimer = gameObject.AddComponent<Timer>();
            lifeTimeTimer.Set(lifeTimeAfterHit);
            areaDamageIntervalTimer = gameObject.AddComponent<Timer>();
            areaDamageIntervalTimer.Set(areaDamageInterval);
            areaDmgLayerMask |= 1 << 8;
            areaDmgLayerMask |= 1 << 10;
            poisonCloud = GetComponentInChildren<ParticleSystem>();
            poisonCloudShape = poisonCloud.shape;
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            lifeTimeTimer.TimerCompleted += LifeTimeOver;
            areaDamageIntervalTimer.TimerCompleted += AreaDamageTick;
        }

        private void OnDisable()
        {
            if (lifeTimeTimer != null)
            {
                lifeTimeTimer.TimerCompleted -= LifeTimeOver;
            }

            if (areaDamageIntervalTimer != null)
            {
                areaDamageIntervalTimer.TimerCompleted -= AreaDamageTick;
            }
        }

        private void Update()
        {
            if (isHit)
            {
                hitBox.radius += scaleSpeed * Time.deltaTime;
                poisonCloudShape.scale = Vector3.one * hitBox.radius;
            }
            else
            {
                projectileVerticalSpeed -= gravity * Time.deltaTime;
                float speedH = projectileSpeed * Time.deltaTime;
                transform.position += transform.forward * speedH + Vector3.up * projectileVerticalSpeed;
            }
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot);
            gameObject.layer = GlobalVariables.ENEMY_PROJECTILE_LAYER;
            hitBox.radius = ogRadius;
            hitBox.enabled = true;
            isHit = false;
            transform.localScale = Vector3.one;
            Launch(pos);
            mesh.SetActive(true);
        }

        public override void OnHit()
        {
            hitBox.enabled = false;
            isHit = true;
            mesh.SetActive(false);
            areaDamageIntervalTimer.Run();
            poisonCloud.Play();
            lifeTimeTimer.Run();
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MushroomSFX.PoisonCloud, 4f);
        }

        private void Launch(Vector3 pos)
        {
            playerDistance = MathUtils.GetDistanceToPlayer(pos);
            //projectileSpeed = playerDistance / 100f + 5f;
            //projectileLaunchVerticalSpeed = playerDistance / 1000f;
            projectileLaunchVerticalSpeed = playerDistance / 1500f;
            projectileVerticalSpeed = projectileLaunchVerticalSpeed;
        }

        private void LifeTimeOver()
        {
            // Does not need to get called since should always be finished by the time SFX has played and lifetime will end.
            //poisonCloud.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            areaDamageIntervalTimer.Reset();
            Deactivate();
        }


        private void AreaDamageTick()
        {
            Collider[] units = Physics.OverlapSphere(transform.position, hitBox.radius, areaDmgLayerMask);
            for (int i = 0; i<units.Length; i++)
            {
                IHealth otherHealth = units[i].GetComponent<IHealth>();
                if (otherHealth == null)
                {
                    otherHealth = units[i].GetComponentInParent<IHealth>();
                }

                otherHealth.DecreaseHealth(areaDamage, areaDamageType);
            }

            areaDamageIntervalTimer.Run();
        }

        private IEnumerator DmgChange()
        {
            yield return 0;
            if (!isHit)
            {
                DamageAmount *= 5;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isHit)
            {
                int otherLayer = other.gameObject.layer;
                if (otherLayer == GlobalVariables.ENEMY_LAYER || otherLayer == GlobalVariables.PLAYER_LAYER)
                {
                    IHealth otherHealth = other.GetComponent<IHealth>();
                    if (otherHealth == null)
                    {
                        otherHealth = other.GetComponentInParent<IHealth>();
                    }

                    otherHealth.DecreaseHealth(DamageAmount, DamageType);
                    OnHit();
                }
                else if (otherLayer == GlobalVariables.PLAYER_MELEE_LAYER)
                {
                    gameObject.layer = GlobalVariables.PLAYER_PROJECTILE_LAYER;
                    transform.rotation = GameMan.Instance.PlayerT.rotation;
                    projectileVerticalSpeed = projectileLaunchVerticalSpeed;
                    StartCoroutine(DmgChange());
                }
                else
                {
                    OnHit();
                }
            }
        }
    }
}