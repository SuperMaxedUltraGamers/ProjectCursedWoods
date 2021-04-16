﻿using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class TreeBoss : UnitPoolable
    {
        public static bool hasMeleeHit;

        private const float MIN_IDLE_TIME = 0.25f;
        private const float MAX_IDLE_TIME = 1.5f;
        private const float SLAM_ATTACK_MAX_ANGLE = 16f;

        private Animator animator;

        private Collider hitbox;
        [SerializeField]
        private TreeBossMeleeTrigger[] rightHandColls;
        [SerializeField]
        private TreeBossMeleeTrigger[] leftHandColls;
        [SerializeField]
        private TreeBossRoots roots;

        private Transform playerT;

        private TreeBossBehaviours currentBehaviour = TreeBossBehaviours.Sleep;
        private TreeBossBehaviours lastBehaviour = TreeBossBehaviours.Sleep;

        private float idleTime;
        private float elapsedIdleTime;

        private bool hasTransitionedIn;

        [SerializeField]
        private int slamDamageAmount = 215;
        [SerializeField]
        private int sweepDamageAmount = 155;
        [SerializeField]
        private int rootDamageAmount = 315;
        private int currentAttackDmg;
        private DamageType attacksDmgType = DamageType.Physical;

        private float idleTrackingSpeed = 1f;
        private float slamTrackingSpeed = 0.2f;

        private float maxMeleeAttacksRange = 140f;

        private Quaternion newRotation;

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

        private delegate void TransitionDel();

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            hitbox = GetComponent<MeshCollider>();
            gameObject.SetActive(false);
            eyeMat = meshRenderer.materials[1];
            eyeMat.SetColor("_EmissionColor", sleepEyeColor);
        }

        private void Start()
        {
            playerT = GameMan.Instance.PlayerT;
        }
        
        private void OnEnable()
        {
            //HealthChanged += TookDamage;
            AwakeTreeBossTrigger.AwakeTreeBossEvent += AwakeFromSleep;
        }

        private void Update()
        {
            YAxisKillCheck();

            switch (currentBehaviour)
            {
                case TreeBossBehaviours.Awaking:
                    Awaking();
                    break;
                case TreeBossBehaviours.Idle:
                    Idle();
                    break;
                case TreeBossBehaviours.SlamAttack:
                    SlamAttack();
                    break;
                case TreeBossBehaviours.SweepRight:
                    SweepRight();
                    break;
                case TreeBossBehaviours.SweepLeft:
                    SweepLeft();
                    break;
                case TreeBossBehaviours.RootAttack:
                    RootAttack();
                    break;
                case TreeBossBehaviours.Dead:
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
            currentBehaviour = TreeBossBehaviours.Sleep;
            lastBehaviour = TreeBossBehaviours.Sleep;
            //SetCollidersEnable(hitboxes, true);
            hasTransitionedIn = false;
        }

        public void AwakeFromSleep()
        {
            SetNextBehaviour(TreeBossBehaviours.Awaking);
            elapsedEyeColorChangeTime = 0f;
            GameMan.Instance.LevelUIManager.ConfigureBossHealthBar(this, bossName, CurrentHealth, MaxHealth);
        }

        protected override void Die()
        {
            if (currentBehaviour != TreeBossBehaviours.Dead)
            {
                currentBehaviour = TreeBossBehaviours.Dead;
                //HealthChanged -= TookDamage;
                AwakeTreeBossTrigger.AwakeTreeBossEvent -= AwakeFromSleep;
                //hitbox.enabled = false;
                gameObject.layer = 0; // Set layer to default to keep collisions

                GameMan.Instance.AIManager.EnemiesKilledFleeAffector++;
                GameMan.Instance.LevelUIManager.DisableBossHealthBar();
                animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.TREEBOSS_ANIM_DEATH);
                elapsedEyeColorChangeTime = 0f;
                SetNextBehaviour(TreeBossBehaviours.Dead);
            }
        }

        private void Awaking()
        {
            elapsedEyeColorChangeTime += Time.deltaTime;
            if (elapsedEyeColorChangeTime >= eyeColorChangeTime)
            {
                eyeMat.SetColor("_EmissionColor", awakeEyeColor);
                IsImmortal = false;
                SetNextBehaviour(TreeBossBehaviours.Idle);
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

            newRotation = newRotation = MathUtils.GetLookRotationYAxis(playerPos, transPos, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * idleTrackingSpeed);

            elapsedIdleTime += Time.deltaTime;
            if (elapsedIdleTime >= idleTime)
            {
                int rootAttackRandomChance = Random.Range(0, 5);
                if (rootAttackRandomChance == 0)
                {
                    SetNextBehaviour(TreeBossBehaviours.RootAttack);
                    return;
                }

                Vector3 targetDir = playerPos - transPos;
                float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);
                //print(angle);
                float distanceToPlayer = GetDistanceToPlayer();
                //print(distanceToPlayer);
                if (GetDistanceToPlayer() > maxMeleeAttacksRange)
                {
                    SetNextBehaviour(TreeBossBehaviours.RootAttack);
                }
                else if (angle < SLAM_ATTACK_MAX_ANGLE && angle > -SLAM_ATTACK_MAX_ANGLE)
                {
                    SetNextBehaviour(TreeBossBehaviours.SlamAttack);
                }
                else if (angle < -SLAM_ATTACK_MAX_ANGLE)
                {
                    SetNextBehaviour(TreeBossBehaviours.SweepRight);
                }
                else if (angle > SLAM_ATTACK_MAX_ANGLE)
                {
                    SetNextBehaviour(TreeBossBehaviours.SweepLeft);
                }
            }
        }

        private void SlamAttack()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(SlamTrans);
            }

            newRotation = newRotation = MathUtils.GetLookRotationYAxis(playerT.position, transform.position, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * slamTrackingSpeed);
        }

        private void SweepRight()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(SweepRightTrans);
            }
        }

        private void SweepLeft()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(SweepLeftTrans);
            }
        }

        private void RootAttack()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(RootAttackTrans);
            }
        }

        private void Dead()
        {
            elapsedEyeColorChangeTime += Time.deltaTime;
            if (elapsedEyeColorChangeTime >= eyeColorChangeTime)
            {
                eyeMat.SetColor("_EmissionColor", sleepEyeColor);
                this.enabled = false;
                return;
            }

            eyeColor = Color.Lerp(awakeEyeColor, sleepEyeColor, elapsedEyeColorChangeTime / eyeColorChangeTime);
            eyeMat.SetColor("_EmissionColor", eyeColor);
        }

        // TRANSITIONS
        private void TransitionIn(TransitionDel transitionDel)
        {
            transitionDel();

            hasTransitionedIn = true;
        }

        private void IdleTrans()
        {
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.TREEBOSS_ANIM_IDLE);
            idleTime = Random.Range(MIN_IDLE_TIME, MAX_IDLE_TIME);
            elapsedIdleTime = 0f;
        }

        private void SlamTrans()
        {
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.TREEBOSS_ANIM_SLAM_ATTACK);
            currentAttackDmg = slamDamageAmount;
            //isTriggered = false;
        }

        private void SweepRightTrans()
        {
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.TREEBOSS_ANIM_SWEEP_RIGHT);
            currentAttackDmg = sweepDamageAmount;
            //isTriggered = false;
        }

        private void SweepLeftTrans()
        {
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.TREEBOSS_ANIM_SWEEP_LEFT);
            currentAttackDmg = sweepDamageAmount;
            //isTriggered = false;
        }

        private void RootAttackTrans()
        {
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.TREEBOSS_ANIM_ROOT_ATTACK);
        }

        private float GetDistanceToPlayer()
        {
            return MathUtils.GetDistanceToPlayer(transform.position);
        }

        private void YAxisKillCheck()
        {
            float posY = transform.position.y;
            // TODO: take into account enemy origin height or give possibility to ignore height kill check.
            if (posY < -75f || posY > 20f)
            {
                if (currentBehaviour != TreeBossBehaviours.Dead)
                {
                    Die();
                }
            }
        }

        /*
        private void SetCollidersEnable(Collider[] colls, bool isEnabled)
        {
            for (int i = 0; i < colls.Length; i++)
            {
                colls[i].enabled = isEnabled;
            }
        }
        */

        private void ActivateMeleeTriggers(TreeBossMeleeTrigger[] triggers)
        {
            for (int i = 0; i < triggers.Length; i++)
            {
                triggers[i].SetDmg(currentAttackDmg, attacksDmgType);
            }
        }

        private void DisableMeleeTriggers(TreeBossMeleeTrigger[] triggers)
        {
            for (int i = 0; i < triggers.Length; i++)
            {
                triggers[i].DisableCollider();
            }
        }

        private void SetNextBehaviour(TreeBossBehaviours nextBehaviour)
        {
            lastBehaviour = currentBehaviour;
            currentBehaviour = nextBehaviour;
            hasTransitionedIn = false;
        }

        // ANIM EVENTS
        private void SlamOpenDmgWindowAnimEvent()
        {
            ActivateMeleeTriggers(rightHandColls);
            ActivateMeleeTriggers(leftHandColls);
            hasMeleeHit = false;
        }
        private void SlamCloseDmgWindowAnimEvent()
        {
            DisableMeleeTriggers(rightHandColls);
            DisableMeleeTriggers(leftHandColls);
        }

        private void SlamEndAnimEvent()
        {
            SetNextBehaviour(TreeBossBehaviours.Idle);
        }

        private void SweepRightOpenDmgWindowAnimEvent()
        {
            ActivateMeleeTriggers(rightHandColls);
            hasMeleeHit = false;
        }

        private void SweepRightCloseDmgWindowAnimEvent()
        {
            DisableMeleeTriggers(rightHandColls);
        }

        private void SweepRightEndAnimEvent()
        {
            SetNextBehaviour(TreeBossBehaviours.Idle);
        }

        private void SweepLeftOpenDmgWindowAnimEvent()
        {
            ActivateMeleeTriggers(leftHandColls);
            hasMeleeHit = false;
        }

        private void SweepLeftCloseDmgWindowAnimEvent()
        {
            DisableMeleeTriggers(leftHandColls);
        }

        private void SweepLeftEndAnimEvent()
        {
            SetNextBehaviour(TreeBossBehaviours.Idle);
        }

        private void RootStartAnimEvent()
        {
            roots.gameObject.SetActive(true);
            roots.StartAttack(rootDamageAmount, attacksDmgType);
            // TODO: initialise roots
        }

        private void RootEndAnimEvent()
        {
            roots.gameObject.SetActive(false);
            SetNextBehaviour(TreeBossBehaviours.Idle);
        }
    }
}