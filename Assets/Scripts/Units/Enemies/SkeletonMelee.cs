using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace CursedWoods
{
    public class SkeletonMelee : EnemyBase
    {
        private NavMeshAgent agent;
        private Animator animator;
        private Rigidbody rb;

        [SerializeField]
        private Transform playerT;

        private float minStateTime = 3f;
        private float maxStateTime = 6f;
        private float timeOnCurrentState;
        private bool hasTransitionedIn;

        private float animTimeBeforeDmg;
        private float animTimeAfterDmg;

        [SerializeField]
        private int attackDamageAmount = 15;

        [SerializeField]
        private float attackTrackingSpeed = 40;
        private DamageType attackDmgType = DamageType.Melee;

        [SerializeField]
        private LayerMask playerLayerMask;

        [SerializeField, Tooltip("What distance from we notice the player in any direction.")]
        private float playerCheckRadius = 5f;

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

        private delegate void TransitionDel();

        protected override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            animTimeBeforeDmg = 4f / 3f;
            animTimeAfterDmg = 5.042f / 3f - animTimeBeforeDmg;
        }

        private void Start()
        {
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
                /*
            case EnemyBehaviours.ChasePlayer:
                agent.SetDestination(playerT.position);
                break;
                */
                case EnemyBehaviours.AttackPlayer:

                    float distanceToPlayer = Vector3.Distance(transform.position, playerT.position);
                    if (distanceToPlayer > minComfortRange)
                    {
                        //agent.SetDestination(playerT.position);
                        //animator.SetFloat("Blend", 1f, 0.1f, Time.fixedDeltaTime);
                        animator.SetFloat("Blend", 0f, 0.1f, Time.fixedDeltaTime);
                    }
                    else // if (distanceToPlayer < minComfortRange)
                    {
                        /*
                        transform.rotation = Quaternion.Slerp(transform.rotation,
                            Quaternion.LookRotation(playerT.position - transform.position, transform.up),
                            Time.fixedDeltaTime * attackTrackingSpeed);
                        */
                        transform.position = Vector3.Lerp(transform.position,
                            transform.position - transform.forward * 0.5f,
                            Time.fixedDeltaTime);

                        animator.SetFloat("Blend", 1f, 0.1f, Time.fixedDeltaTime);
                    }
                    /*
                    else
                    {
                        animator.SetFloat("Blend", 0f, 0.1f, Time.fixedDeltaTime);
                    }
                    */

                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(playerT.position - transform.position, transform.up),
                        Time.fixedDeltaTime * attackTrackingSpeed);

                    break;
                case EnemyBehaviours.FleeFromPlayer:
                    rb.velocity = transform.forward * fleeSpeed * Time.deltaTime;
                    break;
            }
        }

        protected override void TookDamage(int dmg)
        {
            if (currentBehaviour == EnemyBehaviours.Idle || currentBehaviour == EnemyBehaviours.Patrol)
            {
                hasTransitionedIn = false;
                currentBehaviour = EnemyBehaviours.ChasePlayer;
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

            animator.SetFloat("Blend", 0f, 0.1f, Time.deltaTime);
            animator.SetFloat("TorsoBlend", 0f, 0.1f, Time.deltaTime);

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

            animator.SetFloat("Blend", 1f, 0.1f, Time.deltaTime);
            animator.SetFloat("TorsoBlend", 1f, 0.1f, Time.deltaTime);

            StateTimeFlow(EnemyBehaviours.Idle);
        }

        private void ChasePlayer()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(hasRandomStateTime: false, ChaseTrans);
            }

            float distanceToPlayer = Vector3.Distance(transform.position, playerT.position);

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
                animator.SetFloat("Blend", 1f, 0.1f, Time.deltaTime);
                animator.SetFloat("TorsoBlend", 1f, 0.1f, Time.deltaTime);
            }
        }

        private void AttackPlayer()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(hasRandomStateTime: true, AttackTrans);
            }
        }

        private void Flee()
        {
            if (!hasTransitionedIn)
            {
                TransitionIn(hasRandomStateTime: false, FleeTrans);
            }

            newRotation = Quaternion.LookRotation(transform.position - playerT.position, transform.up);
            newRotation.x = 0f;
            newRotation.z = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * fleeRotSpeed);

            animator.SetFloat("Blend", 1f, 0.1f, Time.deltaTime);
            animator.SetFloat("TorsoBlend", 1f, 0.1f, Time.deltaTime);

            if (Vector3.Distance(transform.position, playerT.position) > 35f)
            {
                Die();
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
            rb.velocity = Vector3.zero;
            agent.enabled = false;
            hasTransitionedIn = true;
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

            hasTransitionedIn = true;
        }

        private void ChaseTrans()
        {
            animator.speed = 3f;
            rb.isKinematic = true;
            agent.enabled = true;
            hasTransitionedIn = true;
        }

        private void AttackTrans()
        {
            animator.speed = 2f;
            StartCoroutine(AttackTimer());
            hasTransitionedIn = true;
            animator.SetBool("IsAttacking", true);
        }

        private void FleeTrans()
        {
            StopAllCoroutines();
            animator.SetBool("IsAttacking", false);
            rb.isKinematic = false;
            animator.speed = 3f;
            agent.enabled = false;
            hasTransitionedIn = true;
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

        private bool CheckForPlayerInRadius()
        {
            if (Vector3.Distance(transform.position, playerT.position) < playerCheckRadius)
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
            // Check if skeleton is dead.
            if (CurrentHealth > MinHealth)
            {
                // TODO: do we need to be check if this object is still active?

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
            // Check if skeleton is dead.
            if (CurrentHealth > MinHealth)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerT.position);
                if (distanceToPlayer < attackRange)
                {
                    StartCoroutine(AttackTimer());
                }
                else if (distanceToPlayer > attackRange)
                {
                    currentBehaviour = EnemyBehaviours.ChasePlayer;
                    hasTransitionedIn = false;
                    animator.SetBool("IsAttacking", false);
                }
            }
        }
    }
}