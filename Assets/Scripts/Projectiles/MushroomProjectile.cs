using CursedWoods.Utils;
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
        private int areaDamage = 5;
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
            hitBox.radius = ogRadius;
            hitBox.enabled = true;
            isHit = false;
            DamageAmount = OgDamageAmount;
            transform.localScale = Vector3.one;
            Launch(pos);
            mesh.SetActive(true);
        }

        private void OnHit()
        {
            hitBox.enabled = false;
            isHit = true;
            mesh.SetActive(false);
            poisonCloud.Play();
            lifeTimeTimer.Run();
        }

        private void Launch(Vector3 pos)
        {
            //Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.PlayerSFX.Fireball);
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
                units[i].GetComponent<IHealth>().DecreaseHealth(areaDamage, areaDamageType);
            }

            areaDamageIntervalTimer.Run();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isHit)
            {
                int otherLayer = other.gameObject.layer;
                if (otherLayer == GlobalVariables.ENEMY_LAYER || otherLayer == GlobalVariables.PLAYER_LAYER)
                {
                    other.GetComponent<IHealth>().DecreaseHealth(DamageAmount, DamageType);
                    OnHit();
                }
                else if (otherLayer == GlobalVariables.PLAYER_MELEE_LAYER)
                {
                    transform.rotation = GameMan.Instance.PlayerT.rotation;
                    projectileVerticalSpeed = projectileLaunchVerticalSpeed;
                    DamageAmount *= 5;
                }
                else
                {
                    OnHit();
                }

                areaDamageIntervalTimer.Run();
            }
        }
    }
}