using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class PossessedTree : EnemyBase
    {
        private NavMeshAgent agent;
        private NavMeshObstacle obstacle;
        private Animator animator;
        private Rigidbody rb;
        private Collider hitbox;

        private float animChangeDampTime = 0.1f;
        private bool hasTransitionedIn;

        private float deactivationAfterDeathTime = 7f;
        private float deathDescendSpeed = 0.3f;
        private bool isDescending;

        [SerializeField]
        private int meleeDamageAmount = 50;
        [SerializeField]
        private int rangedDamageAmount = 25;

        [SerializeField]
        private float attackTrackingSpeed = 40;

        private float meleeAttackRangeRealUnits = 3.75f;
        private DamageType meleeAttackDmgType = DamageType.Physical;
        [SerializeField]
        private float rangedAttackRange = 200f;
        private DamageType rangedAttackDmgType = DamageType.Physical;

        [SerializeField]
        private LayerMask playerLayerMask;

        [SerializeField, Tooltip("What distance from we notice the player in any direction.")]
        private float playerCheckRadius = 100f;

        [SerializeField, Tooltip("How far away from player until giving up chasing.")]
        private float giveUpChaseDistance = 350f;

        private Quaternion newRotation;

        [SerializeField]
        private float fleeSpeed;

        [SerializeField]
        private float fleeRotSpeed;
        private int maxAddedCowardnessValue = 5;

        private float backUpSpeed = 16f;

        private float knockBackForce = 20000f;
        private float knockBackstaggerTime = 3f;

        [SerializeField]
        private Canvas healthBar;

        private EnemyBehaviours lastBehaviour = EnemyBehaviours.Idle;

        private delegate void TransitionDel();

        protected override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
            obstacle = GetComponent<NavMeshObstacle>();
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            hitbox = GetComponent<Collider>();
            agent.enabled = false;
            obstacle.enabled = false;
            healthBar.enabled = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            FallenThroughGroundKillCheck();
            //print(transform.position);

            switch (currentBehaviour)
            {
                case EnemyBehaviours.Idle:
                    Idle();
                    break;
                case EnemyBehaviours.ChasePlayer:
                    ChasePlayer();
                    break;
                case EnemyBehaviours.MeleeAttackPlayer:
                    MeleeAttackPlayer();
                    break;
                case EnemyBehaviours.RangeAttackPlayer:
                    RangedAttackPlayer();
                    break;
                case EnemyBehaviours.FleeFromPlayer:
                    Flee();
                    break;
                case EnemyBehaviours.Knockback:
                    KnockBack();
                    break;
                case EnemyBehaviours.Dead:
                    if (isDescending)
                    {
                        //print("descending");
                        transform.position += -transform.up * deathDescendSpeed * Time.deltaTime;
                    }

                    break;
            }
        }

        private void FixedUpdate()
        {
            switch (currentBehaviour)
            {
                case EnemyBehaviours.Idle:
                    rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                    break;
                case EnemyBehaviours.MeleeAttackPlayer:

                    float distanceToPlayer = GetDistanceToPlayer();
                    if (distanceToPlayer > minComfortRange)
                    {
                        animator.SetFloat("Blend", 0f, animChangeDampTime, Time.fixedDeltaTime);
                        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                    }
                    else
                    {
                        Vector3 vel = -transform.forward * backUpSpeed * Time.fixedDeltaTime;
                        rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);
                        animator.SetFloat("Blend", 1f, animChangeDampTime, Time.fixedDeltaTime);
                    }

                    break;
                case EnemyBehaviours.RangeAttackPlayer:
                    animator.SetFloat("Blend", 0f, animChangeDampTime, Time.fixedDeltaTime);
                    rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                    break;
                case EnemyBehaviours.FleeFromPlayer:
                    Vector3 fleeVel = transform.forward * fleeSpeed * Time.fixedDeltaTime;
                    rb.velocity = new Vector3(fleeVel.x, rb.velocity.y, fleeVel.z);
                    break;
                case EnemyBehaviours.Knockback:
                    Vector3 knockbackVel = rb.velocity * 0.9f;
                    rb.velocity = new Vector3(knockbackVel.x, rb.velocity.y, knockbackVel.z);
                    break;
            }
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot);
            ResetValues();
            currentBehaviour = EnemyBehaviours.Idle;
            lastBehaviour = EnemyBehaviours.Idle;
            hitbox.enabled = true;
            hasTransitionedIn = false;
            isDescending = false;
        }

        protected override void TookDamage(int currentHealth, int maxHealth)
        {
            if (currentBehaviour == EnemyBehaviours.Idle || currentBehaviour == EnemyBehaviours.Patrol)
            {
                hasTransitionedIn = false;
                lastBehaviour = currentBehaviour;
                currentBehaviour = EnemyBehaviours.ChasePlayer;
            }
        }

        protected override void GotKnockedBack()
        {
            if (currentBehaviour != EnemyBehaviours.Knockback && currentBehaviour != EnemyBehaviours.Dead)
            {
                lastBehaviour = currentBehaviour;
                currentBehaviour = EnemyBehaviours.Knockback;
                hasTransitionedIn = false;
            }
        }

        protected override void CheckFleePossibility(int fleeAffectorValue)
        {
            if (currentBehaviour == EnemyBehaviours.MeleeAttackPlayer || currentBehaviour == EnemyBehaviours.ChasePlayer)
            {
                if (fleeAffectorValue * cowardnessValue + Random.Range(0, maxAddedCowardnessValue) >= 100)
                {
                    hasTransitionedIn = false;
                    lastBehaviour = currentBehaviour;
                    currentBehaviour = EnemyBehaviours.FleeFromPlayer;
                }
            }
        }

        protected override void Die()
        {
            base.Die();
            rb.isKinematic = true;
            hitbox.enabled = false;
            agent.enabled = false;
            obstacle.enabled = false;
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_DEATH);
            currentBehaviour = EnemyBehaviours.Dead;
            //StartCoroutine(DieTimer());
        }

        private void Idle()
        {
            if (CheckForPlayerInRadius())
            {
                return;
            }

            if (!hasTransitionedIn)
            {
                TransitionIn(IdleTrans);
            }

            animator.SetFloat("Blend", 0f, animChangeDampTime, Time.deltaTime);
            animator.SetFloat("TorsoBlend", 0f, animChangeDampTime, Time.deltaTime);
        }

        private void ChasePlayer()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(ChaseTrans);
            }

            float distanceToPlayer = GetDistanceToPlayer();
            if (distanceToPlayer < attackRange)
            {
                hasTransitionedIn = false;
                lastBehaviour = currentBehaviour;
                currentBehaviour = EnemyBehaviours.MeleeAttackPlayer;
            }
            else if (distanceToPlayer < rangedAttackRange)
            {
                hasTransitionedIn = false;
                lastBehaviour = currentBehaviour;
                currentBehaviour = EnemyBehaviours.RangeAttackPlayer;
            }
            else if (distanceToPlayer > giveUpChaseDistance)
            {
                hasTransitionedIn = false;
                lastBehaviour = currentBehaviour;
                currentBehaviour = EnemyBehaviours.Idle;
            }
            else
            {
                agent.SetDestination(playerT.position);
                animator.SetFloat("Blend", 1f, animChangeDampTime, Time.deltaTime);
                animator.SetFloat("TorsoBlend", 1f, animChangeDampTime, Time.deltaTime);
            }
        }

        private void MeleeAttackPlayer()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(MeleeAttackTrans);
            }

            newRotation = newRotation = MathUtils.GetLookRotationYAxis(playerT.position, transform.position, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * attackTrackingSpeed);
        }

        private void RangedAttackPlayer()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(RangedAttackTrans);
            }

            newRotation = newRotation = MathUtils.GetLookRotationYAxis(playerT.position, transform.position, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * attackTrackingSpeed);
        }

        private void Flee()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(FleeTrans);
            }

            newRotation = MathUtils.GetLookRotationYAxis(transform.position, playerT.position, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * fleeRotSpeed);

            animator.SetFloat("Blend", 1f, animChangeDampTime, Time.deltaTime);
            animator.SetFloat("TorsoBlend", 1f, animChangeDampTime, Time.deltaTime);

            if (GetDistanceToPlayer() > 1000f)
            {
                hasTransitionedIn = false;
                lastBehaviour = EnemyBehaviours.Idle;
                currentBehaviour = EnemyBehaviours.Idle;
            }
        }

        private void KnockBack()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(KnockBackTrans);
            }

            animator.SetFloat("Blend", 0f, animChangeDampTime, Time.deltaTime);
            animator.SetFloat("TorsoBlend", 0f, animChangeDampTime, Time.deltaTime);
        }

        // TRANSITIONS
        private void TransitionIn(TransitionDel transitionDel)
        {
            transitionDel();

            hasTransitionedIn = true;
        }

        private void IdleTrans()
        {
            rb.isKinematic = false;
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            agent.enabled = false;
            obstacle.enabled = true;
            healthBar.enabled = false;
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_NULL);
        }

        private void ChaseTrans()
        {
            rb.isKinematic = true;
            obstacle.enabled = false;
            agent.enabled = true;
            //agent.isStopped = false;
            healthBar.enabled = true;
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_NULL);
        }

        private void MeleeAttackTrans()
        {
            // With these this cannot move backwards while meleeattacking.
            //rb.isKinematic = true;
            //rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            //agent.enabled = true;
            //agent.isStopped = true;
            // With these other agents can push this easily around.
            rb.isKinematic = false;
            agent.enabled = false;
            obstacle.enabled = true;

            healthBar.enabled = true;
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_MELEE_ATTACK);
        }

        private void RangedAttackTrans()
        {
            // With these this cannot move backwards while meleeattacking.
            //rb.isKinematic = true;
            //rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            //agent.enabled = true;
            //agent.isStopped = true;
            // With these other agents can push this easily around.
            rb.isKinematic = false;
            agent.enabled = false;
            obstacle.enabled = true;

            healthBar.enabled = true;
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_RANGED_ATTACK);
        }

        private void FleeTrans()
        {
            //animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_FLEE);
            // Remove these if we get flee animation.
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_NULL);
            //animator.SetFloat("Blend", 1f, animChangeDampTime, Time.deltaTime);
            //animator.SetFloat("TorsoBlend", 1f, animChangeDampTime, Time.deltaTime);
            rb.isKinematic = false;
            agent.enabled = false;
            obstacle.enabled = true;
            healthBar.enabled = true;
        }

        private void KnockBackTrans()
        {
            //animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_STAGGER);
            // Remove these if we get stagger animation.
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_NULL);
            //animator.SetFloat("Blend", 0f, animChangeDampTime, Time.deltaTime);
            //animator.SetFloat("TorsoBlend", 0f, animChangeDampTime, Time.deltaTime);
            rb.isKinematic = false;
            agent.enabled = false;
            obstacle.enabled = true;
            healthBar.enabled = true;
            rb.AddRelativeForce(new Vector3(0f, knockBackForce, -knockBackForce * 5f));

            StartCoroutine(KnockBackTimer());
        }

        private bool CheckForPlayerInRadius()
        {
            float distanceToPlayer = GetDistanceToPlayer();
            if (distanceToPlayer < playerCheckRadius)
            {
                hasTransitionedIn = false;
                lastBehaviour = currentBehaviour;
                currentBehaviour = EnemyBehaviours.ChasePlayer;
                return true;
            }
            else
            {
                return false;
            }
        }

        // ANIMATION EVENTS
        private void MeleeAttackAnimEvent()
        {
            if (currentBehaviour != EnemyBehaviours.Dead && currentBehaviour != EnemyBehaviours.FleeFromPlayer && currentBehaviour != EnemyBehaviours.Knockback)
            {
                if (Physics.Raycast(transform.position + transform.up, transform.forward, meleeAttackRangeRealUnits, playerLayerMask))
                {
                    playerT.gameObject.GetComponent<IHealth>().DecreaseHealth(meleeDamageAmount, meleeAttackDmgType);
                }
            }
        }

        private void MeleeAttackEndAnimEvent()
        {
            if (currentBehaviour != EnemyBehaviours.Dead && currentBehaviour != EnemyBehaviours.FleeFromPlayer && currentBehaviour != EnemyBehaviours.Knockback)
            {
                float distanceToPlayer = GetDistanceToPlayer();
                if (distanceToPlayer < rangedAttackRange && distanceToPlayer > attackRange)
                {
                    lastBehaviour = currentBehaviour;
                    currentBehaviour = EnemyBehaviours.RangeAttackPlayer;
                    hasTransitionedIn = false;
                }
                else if (distanceToPlayer > rangedAttackRange)
                {
                    lastBehaviour = currentBehaviour;
                    currentBehaviour = EnemyBehaviours.ChasePlayer;
                    hasTransitionedIn = false;
                }
            }
        }

        private void RangedAttackAnimEvent()
        {
            if (currentBehaviour != EnemyBehaviours.Dead && currentBehaviour != EnemyBehaviours.FleeFromPlayer && currentBehaviour != EnemyBehaviours.Knockback)
            {
                PosTreeProjectile projectile = (PosTreeProjectile)GameMan.Instance.ObjPoolMan.GetObjectFromPool(ObjectPoolType.TreeProjectile);
                projectile.InitDamageInfo(rangedDamageAmount, rangedAttackDmgType);
                projectile.Activate(transform.position + transform.forward * 2f + transform.up, transform.rotation);
            }
        }

        private void RangedAttackEndAnimEvent()
        {
            if (currentBehaviour != EnemyBehaviours.Dead && currentBehaviour != EnemyBehaviours.FleeFromPlayer && currentBehaviour != EnemyBehaviours.Knockback)
            {
                float distanceToPlayer = GetDistanceToPlayer();
                if (distanceToPlayer < attackRange)
                {
                    lastBehaviour = currentBehaviour;
                    currentBehaviour = EnemyBehaviours.MeleeAttackPlayer;
                    hasTransitionedIn = false;
                }
                else if (distanceToPlayer > rangedAttackRange)
                {
                    lastBehaviour = currentBehaviour;
                    currentBehaviour = EnemyBehaviours.ChasePlayer;
                    hasTransitionedIn = false;
                }
            }
        }

        // Called from animation event
        private IEnumerator DieTimer()
        {
            isDescending = true;
            healthBar.enabled = false;

            int spawnHealthDecider = Random.Range(0, 100);
            if (spawnHealthDecider < 20)
            {
                MaxHealthPickUp health = (MaxHealthPickUp) GameMan.Instance.ObjPoolMan.GetObjectFromPool(ObjectPoolType.MaxHealthPickUp);
                health.Activate(transform.position, transform.rotation);
            }
            else if (spawnHealthDecider < 60)
            {
                HealthPickUp health = (HealthPickUp) GameMan.Instance.ObjPoolMan.GetObjectFromPool(ObjectPoolType.HealthPickUp);
                health.Activate(transform.position, transform.rotation);
            }

            yield return new WaitForSeconds(deactivationAfterDeathTime);
            Deactivate();
        }

        private IEnumerator KnockBackTimer()
        {
            yield return new WaitForSeconds(knockBackstaggerTime);
            if (currentBehaviour != EnemyBehaviours.Dead && lastBehaviour != EnemyBehaviours.FleeFromPlayer)
            {
                float distanceToPlayer = GetDistanceToPlayer();
                // Dont necessarily have to check if in attackrange because we will move to attack from chase if close enough.
                if (distanceToPlayer < giveUpChaseDistance)
                {
                    lastBehaviour = currentBehaviour;
                    currentBehaviour = EnemyBehaviours.ChasePlayer;
                }
                else
                {
                    lastBehaviour = currentBehaviour;
                    currentBehaviour = EnemyBehaviours.Idle;
                }
            }
            else if (lastBehaviour == EnemyBehaviours.FleeFromPlayer)
            {
                lastBehaviour = currentBehaviour;
                currentBehaviour = EnemyBehaviours.FleeFromPlayer;
            }

            hasTransitionedIn = false;
        }
    }
}