using UnityEngine;
using CursedWoods.Utils;
using CursedWoods.Data;

namespace CursedWoods
{
    public class FinalBoss : UnitPoolable
    {
        public static bool hasMeleeHit;

        private const float MIN_IDLE_TIME = 0.5f;
        private const float MAX_IDLE_TIME = 1.5f;

        private Animator animator;
        private Collider hitbox;

        private Transform playerT;

        private FinalBossBehaviours currentBehaviour = FinalBossBehaviours.Sleep;

        private float idleTime;
        private float elapsedIdleTime;

        private bool hasTransitionedIn;

        [SerializeField]
        private int scanLaserDamageAmount = 215;
        [SerializeField]
        private int projectileDamageAmount = 155;
        [SerializeField]
        private int magicScytheDamageAmount = 315;

        private float idleTrackingSpeed = 4f;
        private float walkTrackingSpeed = 4f;
        private float projectileAttackTrackingSpeed = 3.5f;
        private float laserSetupRotSpeed = 1.6f;
        private float laserScanRotSpeed = 1.6f;
        private float currentLaserRotSpeed;

        [SerializeField]
        private FinalBossLaser laser = null;
        [SerializeField]
        private FinalBossProjectileSpawner[] projectileSpawners;
        [SerializeField]
        private FinalBossMagicScythe magicScythe = null;

        private Quaternion targetRot;

        [SerializeField]
        private string bossName = "";

        [SerializeField]
        private Renderer meshRenderer;
        private Material eyeMat;
        private Color sleepEyeColor = new Color(0, 0, 0);
        private Color awakeEyeColor = new Color(8, 0, 0);
        private Color eyeColor;
        private float eyeColorChangeTime = 2f;
        private float elapsedEyeColorChangeTime = 0f;

        private float scanLaserStartAngle;
        private float scanLaserEndAngle;

        [SerializeField]
        private ParticleSystem deathSprayParticles = null;
        [SerializeField]
        private ParticleSystem deathStaticParticle = null;
        private ParticleSystem.ShapeModule deathStaticParticleShape;
        [SerializeField]
        private ParticleSystem deathExplosionParticles = null;

        private delegate void TransitionDel();

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            hitbox = GetComponent<Collider>();
            eyeMat = meshRenderer.materials[11];
            eyeMat.SetColor("_EmissionColor", sleepEyeColor);
            deathStaticParticleShape = deathStaticParticle.shape;
            gameObject.SetActive(false);
        }

        private void Start()
        {
            playerT = GameMan.Instance.PlayerT;
        }

        private void OnEnable()
        {
            //HealthChanged += TookDamage;
            AwakeFinalBossTrigger.AwakeFinalBossEvent += AwakeFromSleep;
        }

        private void Update()
        {
            YAxisKillCheck();

            switch (currentBehaviour)
            {
                case FinalBossBehaviours.Awaking:
                    Awaking();
                    break;
                case FinalBossBehaviours.Idle:
                    Idle();
                    break;
                case FinalBossBehaviours.Walk:
                    Walk();
                    break;
                case FinalBossBehaviours.Dash:
                    Dash();
                    break;
                case FinalBossBehaviours.ScanLaser:
                    ScanLaser();
                    break;
                case FinalBossBehaviours.Projectile:
                    MagicScythe();
                    break;
                case FinalBossBehaviours.MagicScythe:
                    Projectile();
                    break;
                case FinalBossBehaviours.Dead:
                    Dead();
                    break;
            }
        }

        // Could be used to aggro if lots of damage or health below half
        /*
        private void TookDamage(int currentHealth, int maxHealth)
        {

        }
        */

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot);
            ResetValues();
            IsImmortal = true;
            currentBehaviour = FinalBossBehaviours.Sleep;
            hasTransitionedIn = false;
            DisableAttacks();
        }

        public override void DecreaseHealth(int amount, DamageType damageType)
        {
            base.DecreaseHealth(amount, damageType);
            if (CurrentHealth > 0)
            {
                Settings.Instance.Audio.PlayEffect(audioSource, AudioContainer.FinalBossSFX.TakeDamage);
            }
        }

        public void AwakeFromSleep()
        {
            SetNextBehaviour(FinalBossBehaviours.Awaking);
            GameMan.Instance.CastleManager.EnableBarrier(GlobalVariables.CASTLE_FINAL_BOSS_BARRIER);
            elapsedEyeColorChangeTime = 0f;
            GameMan.Instance.LevelUIManager.ConfigureBossHealthBar(this, bossName, CurrentHealth, MaxHealth);
        }

        protected override void Die()
        {
            if (currentBehaviour != FinalBossBehaviours.Dead)
            {
                //HealthChanged -= TookDamage;
                AwakeFinalBossTrigger.AwakeFinalBossEvent -= AwakeFromSleep;
                DisableAttacks();
                GameMan.Instance.AIManager.EnemiesKilledFleeAffector++;
                GameMan.Instance.LevelUIManager.DisableBossHealthBar();
                animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.FINALBOSS_ANIM_DEATH);
                elapsedEyeColorChangeTime = 0f;
                deathStaticParticle.Play();
                deathSprayParticles.Play();
                GameMan.Instance.CastleManager.DisableBarrier(GlobalVariables.CASTLE_FINAL_BOSS_BARRIER);
                Settings.Instance.Audio.PlayEffect(audioSource, AudioContainer.FinalBossSFX.Death, 3f);
                SetNextBehaviour(FinalBossBehaviours.Dead);
            }
        }

        private void DisableAttacks()
        {
            laser.gameObject.SetActive(false);
            for (int i = 0; i < projectileSpawners.Length; i++)
            {
                projectileSpawners[i].gameObject.SetActive(false);
            }

            magicScythe.gameObject.SetActive(false);
        }

        private void Awaking()
        {
            elapsedEyeColorChangeTime += Time.deltaTime;
            if (elapsedEyeColorChangeTime >= eyeColorChangeTime)
            {
                eyeMat.SetColor("_EmissionColor", awakeEyeColor);
                IsImmortal = false;
                SetNextBehaviour(FinalBossBehaviours.Idle);
                return;
            }

            eyeColor = Color.Lerp(sleepEyeColor, awakeEyeColor, elapsedEyeColorChangeTime / eyeColorChangeTime);
            eyeMat.SetColor("_EmissionColor", eyeColor);
        }

        private void Idle()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(IdleTrans);
            }

            Vector3 playerPos = playerT.position;
            Vector3 transPos = transform.position;

            targetRot = MathUtils.GetLookRotationYAxis(playerPos, transPos, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * idleTrackingSpeed);

            elapsedIdleTime += Time.deltaTime;
            if (elapsedIdleTime >= idleTime)
            {
                // TODO: randomize attack
                int attackDecider = Random.Range(0, 3);
                switch (attackDecider)
                {
                    case 0:
                        SetNextBehaviour(FinalBossBehaviours.MagicScythe);
                        break;
                    case 1:
                        SetNextBehaviour(FinalBossBehaviours.Projectile);
                        break;
                    case 2:
                        SetNextBehaviour(FinalBossBehaviours.ScanLaser);
                        break;
                }
            }
        }

        private void Walk()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(WalkTrans);
            }

            targetRot = MathUtils.GetLookRotationYAxis(playerT.position, transform.position, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * walkTrackingSpeed);
        }

        private void Dash()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(DashTrans);
            }
        }

        private void ScanLaser()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(ScanLaserTrans);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * currentLaserRotSpeed);
        }

        private void MagicScythe()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(MagicScytheTrans);
            }
        }

        private void Projectile()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(ProjectileTrans);
            }

            targetRot = MathUtils.GetLookRotationYAxis(playerT.position, transform.position, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * projectileAttackTrackingSpeed);
        }

        private void Dead()
        {
            DisableAttacks();
            float deltaTime = Time.deltaTime;
            elapsedEyeColorChangeTime += deltaTime;
            if (elapsedEyeColorChangeTime >= eyeColorChangeTime)
            {
                eyeMat.SetColor("_EmissionColor", sleepEyeColor);
                this.enabled = false;
                return;
            }

            eyeColor = Color.Lerp(awakeEyeColor, sleepEyeColor, elapsedEyeColorChangeTime / eyeColorChangeTime);
            eyeMat.SetColor("_EmissionColor", eyeColor);
            deathStaticParticleShape.scale += Vector3.one * 1.4f * deltaTime;
        }

        // TRANSITIONS
        private void TransitionIn(TransitionDel transitionDel)
        {
            transitionDel();

            hasTransitionedIn = true;
        }

        private void IdleTrans()
        {
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.FINALBOSS_ANIM_IDLE);
            idleTime = Random.Range(MIN_IDLE_TIME, MAX_IDLE_TIME);
            elapsedIdleTime = 0f;
        }

        private void WalkTrans()
        {
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.FINALBOSS_ANIM_WALK);
        }

        private void DashTrans()
        {
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.FINALBOSS_ANIM_DASH);
        }

        private void ScanLaserTrans()
        {
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.FINALBOSS_ANIM_SCAN_LASER);
            scanLaserStartAngle = transform.rotation.eulerAngles.y + 90f;
            scanLaserEndAngle = scanLaserStartAngle + 180f;

            currentLaserRotSpeed = laserSetupRotSpeed;
            targetRot = Quaternion.Euler(0f, scanLaserStartAngle, 0f);
        }

        private void MagicScytheTrans()
        {
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.FINALBOSS_ANIM_PROJECTILE);
        }

        private void ProjectileTrans()
        {
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.FINALBOSS_ANIM_MAGIC_SCYTHE);
        }

        private void YAxisKillCheck()
        {
            float posY = transform.position.y;
            // TODO: take into account enemy origin height or give possibility to ignore height kill check.
            if (posY < -75f || posY > 20f)
            {
                if (currentBehaviour != FinalBossBehaviours.Dead)
                {
                    Die();
                }
            }
        }

        private void SetNextBehaviour(FinalBossBehaviours nextBehaviour)
        {
            if (currentBehaviour != FinalBossBehaviours.Dead)
            {
                currentBehaviour = nextBehaviour;
                hasTransitionedIn = false;
            }
        }

        // ANIM EVENTS
        private void ScanLaserStartAnimEvent()
        {
            currentLaserRotSpeed = laserScanRotSpeed;
            targetRot = Quaternion.Euler(0f, scanLaserEndAngle, 0f);
            laser.gameObject.SetActive(true);
            laser.Initialize(scanLaserDamageAmount, DamageType.Fire);

            Settings.Instance.Audio.PlayEffect(audioSource, AudioContainer.FinalBossSFX.Laser, 2f);
        }

        private void DisableLaserAnimEvent()
        {
            laser.gameObject.SetActive(false);
        }

        private void ScanLaserEndAnimEvent()
        {
            SetNextBehaviour(FinalBossBehaviours.Idle);
        }

        private void ProjectileStartAnimEvent()
        {
            for (int i = 0; i < projectileSpawners.Length; i++)
            {
                projectileSpawners[i].gameObject.SetActive(true);
                projectileSpawners[i].Initialize(projectileDamageAmount, DamageType.Magic);
            }

            Settings.Instance.Audio.PlayEffect(audioSource, AudioContainer.FinalBossSFX.ProjectileLaunch);
        }

        private void ProjectileEndAnimEvent()
        {
            for (int i = 0; i < projectileSpawners.Length; i++)
            {
                projectileSpawners[i].gameObject.SetActive(false);
            }

            SetNextBehaviour(FinalBossBehaviours.Idle);
        }

        private void MagicScytheStartAnimEvent()
        {
            magicScythe.gameObject.SetActive(true);
            magicScythe.StartAttack(magicScytheDamageAmount, DamageType.Ice);

            Settings.Instance.Audio.PlayEffect(audioSource, AudioContainer.FinalBossSFX.MagicScythe);
        }

        private void MagicScytheCloseDmgWindowAnimEvent()
        {
            magicScythe.gameObject.SetActive(false);
        }

        private void MagicScytheEndAnimEvent()
        {
            SetNextBehaviour(FinalBossBehaviours.Idle);
        }

        private void DeathExplosionAnimEvent()
        {
            deathSprayParticles.Stop();
            deathStaticParticle.Stop();
            deathExplosionParticles.Play();
            hitbox.enabled = false;
            meshRenderer.enabled = false;
            Settings.Instance.Audio.ChangeMusic(AudioContainer.Music.CastleAmbience);
        }
    }
}