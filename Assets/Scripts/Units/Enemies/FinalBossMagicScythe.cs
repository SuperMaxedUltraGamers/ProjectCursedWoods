using UnityEngine;

namespace CursedWoods
{
    public class FinalBossMagicScythe : MonoBehaviour
    {
        private int damageAmount;
        private DamageType damageType;

        private Animator animator;
        private ParticleSystem spawnParticles;
        private Transform playerT;
        private bool canHit;
        private float trackSpeed = 1.5f;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            spawnParticles = GetComponentInChildren<ParticleSystem>();
            playerT = GameMan.Instance.PlayerT;
        }

        private void Update()
        {
            Vector3 trackPos = playerT.position;
            trackPos.y = 2.5f;
            transform.position = Vector3.Lerp(transform.position, trackPos, trackSpeed * Time.deltaTime);
        }

        public void StartAttack(int dmgAmount, DamageType dmgType)
        {
            damageAmount = dmgAmount;
            damageType = dmgType;
            canHit = true;
            Vector3 startPos = playerT.position;
            startPos.y = 2.5f;
            transform.position = startPos;
            spawnParticles.Play();
            animator.Play(GlobalVariables.MAGIC_SCYTHE_ATTACK_ANIM);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (canHit)
            {
                int otherLayer = other.gameObject.layer;
                if (otherLayer == GlobalVariables.PLAYER_LAYER)
                {
                    IHealth otherHealth = other.GetComponent<IHealth>();
                    if (otherHealth == null)
                    {
                        otherHealth = other.GetComponentInParent<IHealth>();
                    }

                    otherHealth.DecreaseHealth(damageAmount, damageType);
                    canHit = false;
                }

                ParticleEffectBase hitParticles = (ParticleEffectBase)GameMan.Instance.ObjPoolMan.GetObjectFromPool(ObjectPoolType.MeleeHitParticles);
                hitParticles.Activate(playerT.position, Quaternion.identity);
            }
        }
    }
}