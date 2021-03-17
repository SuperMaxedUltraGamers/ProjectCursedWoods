using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class SkeletonMelee : EnemyBase
    {
        private NavMeshAgent agent;
        private Animator animator;
        private Rigidbody rb;
        private CapsuleCollider hitbox;

        private Transform playerT;

        private float animChangeDampTime = 0.1f;
        private float minStateTime = 3f;
        private float maxStateTime = 6f;
        private float timeOnCurrentState;
        private bool hasTransitionedIn;

        private float animTimeBeforeDmg;
        private float animTimeAfterDmg;
        private float deactivationAfterDeathTime;

        [SerializeField]
        private int attackDamageAmount = 15;

        [SerializeField]
        private float attackTrackingSpeed = 40;
        private DamageType attackDmgType = DamageType.Melee;

        [SerializeField]
        private LayerMask playerLayerMask;

        [SerializeField, Tooltip("What distance from we notice the player in any direction.")]
        private float playerCheckRadius = 30f;

        [SerializeField, Tooltip("How far away from player until giving up chasing.")]
        private float giveUpChaseDistance = 25f;

        [SerializeField]
        private float patrolSpeed;

        [SerializeField]
        private float patrolRotSpeed;
        private Quaternion newRotation;

        [SerializeField]
        private float fleeSpeed;

        [SerializeField]
        private float fleeRotSpeed;

        private float backUpSpeed = 20f;

        private float knockBackForce = 20000f;
        private float knockBackstaggerTime = 4f;

        private delegate void TransitionDel();

        protected override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            hitbox = GetComponent<CapsuleCollider>();
            animTimeBeforeDmg = 4f / 3f;
            animTimeAfterDmg = 5.042f / 3f - animTimeBeforeDmg;
            deactivationAfterDeathTime = 2f;
            gameObject.SetActive(false);
        }

        private void Start()
        {
            playerT = GameMan.Instance.PlayerT;
            RandomStateTime();
        }

        private void Update()
        {
            switch (currentBehaviour)
            {
                case EnemyBehaviours.Idle:
                    Idle();
                    break;
                case EnemyBehaviours.Patrol:
                    Patrol();
                    break;
                case EnemyBehaviours.ChasePlayer:
                    ChasePlayer();
                    break;
                case EnemyBehaviours.AttackPlayer:
                    AttackPlayer();
                    break;
                case EnemyBehaviours.FleeFromPlayer:
                    Flee();
                    break;
                case EnemyBehaviours.Knockback:
                    KnockBack();
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
                case EnemyBehaviours.Patrol:
                    rb.velocity = transform.forward * patrolSpeed * Time.fixedDeltaTime;
                    break;
                case EnemyBehaviours.AttackPlayer:

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
                case EnemyBehaviours.FleeFromPlayer:
                    rb.velocity = transform.forward * fleeSpeed * Time.fixedDeltaTime;
                    break;
                case EnemyBehaviours.Knockback:
                    Vector3 newVel = rb.velocity * 0.9f;
                    rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);
                    break;
            }
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot);
            ResetValues();
            currentBehaviour = EnemyBehaviours.Idle;
            hitbox.enabled = true;
            hasTransitionedIn = false;
        }

        protected override void TookDamage(int currentHealth, int maxHealth)
        {
            if (currentBehaviour == EnemyBehaviours.Idle || currentBehaviour == EnemyBehaviours.Patrol)
            {
                hasTransitionedIn = false;
                currentBehaviour = EnemyBehaviours.ChasePlayer;
            }
        }

        protected override void GotKnockedBack()
        {
            if (currentBehaviour != EnemyBehaviours.Knockback && currentBehaviour != EnemyBehaviours.Dead)
            {
                currentBehaviour = EnemyBehaviours.Knockback;
                hasTransitionedIn = false;
            }
        }

        protected override void CheckFleePossibility(int fleeAffectorValue)
        {
            if (currentBehaviour == EnemyBehaviours.AttackPlayer || currentBehaviour == EnemyBehaviours.ChasePlayer)
            {
                if (fleeAffectorValue * cowardnessValue + Random.Range(0, 21) >= 100)
                {
                    hasTransitionedIn = false;
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
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_DEATH);
            animator.speed = 3f;
            currentBehaviour = EnemyBehaviours.Dead;
            StartCoroutine(DieTimer());
        }

        private void Idle()
        {
            if (CheckForPlayerInRadius())
            {
                return;
            }

            if (!hasTransitionedIn)
            {
                TransitionIn(hasRandomStateTime: true, IdleTrans);
            }

            animator.SetFloat("Blend", 0f, animChangeDampTime, Time.deltaTime);
            animator.SetFloat("TorsoBlend", 0f, animChangeDampTime, Time.deltaTime);

            StateTimeFlow(EnemyBehaviours.Patrol);
        }

        private void Patrol()
        {
            if (CheckForPlayerInRadius())
            {
                return;
            }

            if (!hasTransitionedIn)
            {
                TransitionIn(hasRandomStateTime: true, PatrolTrans);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * patrolRotSpeed);

            animator.SetFloat("Blend", 1f, animChangeDampTime, Time.deltaTime);
            animator.SetFloat("TorsoBlend", 1f, animChangeDampTime, Time.deltaTime);

            StateTimeFlow(EnemyBehaviours.Idle);
        }

        private void ChasePlayer()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(hasRandomStateTime: false, ChaseTrans);
            }

            float distanceToPlayer = GetDistanceToPlayer();
            if (distanceToPlayer < attackRange)
            {
                hasTransitionedIn = false;
                currentBehaviour = EnemyBehaviours.AttackPlayer;
            }
            else if (distanceToPlayer > giveUpChaseDistance)
            {
                hasTransitionedIn = false;
                currentBehaviour = EnemyBehaviours.Patrol;
            }
            else
            {
                agent.SetDestination(playerT.position);
                animator.SetFloat("Blend", 1f, animChangeDampTime, Time.deltaTime);
                animator.SetFloat("TorsoBlend", 1f, animChangeDampTime, Time.deltaTime);
            }
        }

        private void AttackPlayer()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(hasRandomStateTime: true, AttackTrans);
            }

            newRotation = newRotation = MathUtils.GetLookRotationYAxis(playerT.position, transform.position, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * attackTrackingSpeed);
        }

        private void Flee()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(hasRandomStateTime: false, FleeTrans);
            }

            newRotation = MathUtils.GetLookRotationYAxis(transform.position, playerT.position, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * fleeRotSpeed);

            animator.SetFloat("Blend", 1f, animChangeDampTime, Time.deltaTime);
            animator.SetFloat("TorsoBlend", 1f, animChangeDampTime, Time.deltaTime);

            if (GetDistanceToPlayer() > 1000f)
            {
                Die();
            }
        }

        private void KnockBack()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(hasRandomStateTime: false, KnockBackTrans);
            }
        }

        // TRANSITIONS
        private void TransitionIn(bool hasRandomStateTime, TransitionDel transitionDel)
        {
            transitionDel();
            if (hasRandomStateTime)
            {
                RandomStateTime();
            }

            hasTransitionedIn = true;
        }

        private void IdleTrans()
        {
            animator.speed = 1f;
            rb.isKinematic = false;
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            agent.enabled = false;
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_NULL);
        }

        private void PatrolTrans()
        {
            animator.speed = 1f;
            agent.enabled = false;
            rb.isKinematic = false;
            newRotation = transform.rotation * Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);
            if (Physics.Raycast(transform.position + transform.up, newRotation * Vector3.forward, 4f))
            {
                newRotation = transform.rotation * Quaternion.Euler(0f, 180f, 0f);
            }

            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_NULL);
        }

        private void ChaseTrans()
        {
            animator.speed = 3f;
            rb.isKinematic = true;
            agent.enabled = true;
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_NULL);
        }

        private void AttackTrans()
        {
            rb.isKinematic = false;
            agent.enabled = false;
            animator.speed = 2f;
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_ATTACK);
            StartCoroutine(AttackTimer());
        }

        private void FleeTrans()
        {
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_FLEE);
            rb.isKinematic = false;
            animator.speed = 3f;
            agent.enabled = false;
        }

        private void KnockBackTrans()
        {
            animator.speed = 1f;
            animator.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.ENEMY_ANIM_STAGGER);
            rb.isKinematic = false;
            agent.enabled = false;
            rb.AddRelativeForce(new Vector3(0f, knockBackForce, -knockBackForce * 5f));

            StartCoroutine(KnockBackTimer());
        }

        private void RandomStateTime()
        {
            timeOnCurrentState = Random.Range(minStateTime, maxStateTime);
        }

        private void StateTimeFlow(EnemyBehaviours nexState)
        {
            timeOnCurrentState -= Time.deltaTime;

            if (timeOnCurrentState <= 0)
            {
                if (Random.Range(0f, 1f) > 0.25f)
                {
                    if (currentBehaviour != EnemyBehaviours.ChasePlayer && currentBehaviour != EnemyBehaviours.AttackPlayer)
                    {
                        currentBehaviour = nexState;
                        hasTransitionedIn = false;
                    }
                }
            }
        }

        private float GetDistanceToPlayer()
        {
            Vector3 toPlayer = playerT.position - transform.position;
            return Vector3.SqrMagnitude(toPlayer);
        }

        private bool CheckForPlayerInRadius()
        {
            float distanceToPlayer = GetDistanceToPlayer();
            if (distanceToPlayer < playerCheckRadius)
            {
                hasTransitionedIn = false;
                currentBehaviour = EnemyBehaviours.ChasePlayer;
                return true;
            }
            else
            {
                return false;
            }
        }

        private IEnumerator AttackTimer()
        {
            yield return new WaitForSeconds(animTimeBeforeDmg / animator.speed);
            if (currentBehaviour != EnemyBehaviours.Dead && currentBehaviour != EnemyBehaviours.FleeFromPlayer && currentBehaviour != EnemyBehaviours.Knockback)
            {
                if (Physics.Raycast(transform.position + transform.up, transform.forward, attackRange, playerLayerMask))
                {
                    playerT.gameObject.GetComponent<IHealth>().DecreaseHealth(attackDamageAmount, attackDmgType);
                }

                StartCoroutine(AttackEndTimer());
            }
        }

        private IEnumerator AttackEndTimer()
        {
            yield return new WaitForSeconds(animTimeAfterDmg / animator.speed);
            if (currentBehaviour != EnemyBehaviours.Dead && currentBehaviour != EnemyBehaviours.FleeFromPlayer && currentBehaviour != EnemyBehaviours.Knockback)
            {
                float distanceToPlayer = GetDistanceToPlayer();
                if (distanceToPlayer < attackRange)
                {
                    StartCoroutine(AttackTimer());
                }
                else if (distanceToPlayer > attackRange)
                {
                    currentBehaviour = EnemyBehaviours.ChasePlayer;
                    hasTransitionedIn = false;
                }
            }
        }

        private IEnumerator DieTimer()
        {
            yield return new WaitForSeconds(deactivationAfterDeathTime);
            Deactivate();
        }

        private IEnumerator KnockBackTimer()
        {
            yield return new WaitForSeconds(knockBackstaggerTime);
            if (currentBehaviour != EnemyBehaviours.Dead && currentBehaviour != EnemyBehaviours.FleeFromPlayer)
            {
                currentBehaviour = EnemyBehaviours.Idle;
                hasTransitionedIn = false;
            }
        }
    }
}