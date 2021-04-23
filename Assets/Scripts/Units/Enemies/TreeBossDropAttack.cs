using UnityEngine;

namespace CursedWoods
{
    public class TreeBossDropAttack : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem particles;
        private ParticleSystem.ShapeModule particlesShape;
        private float scaleTime = 1f;
        private float elapsedTime = 0f;
        private float startScale = 1f;
        private float targetScale = 4f;
        private float currentScale = 1f;
        private int damageAmount;
        private DamageType damageType;
        private bool canHit;

        private void Awake()
        {
            particlesShape = particles.shape;
        }

        private void Update()
        {
            if (elapsedTime > scaleTime * 2)
            {
                gameObject.SetActive(false);
            }
            else if (elapsedTime > scaleTime)
            {
                elapsedTime += Time.deltaTime;
                canHit = false;
            }
            else
            {
                elapsedTime += Time.deltaTime;
                currentScale = Mathf.Lerp(startScale, targetScale, elapsedTime / scaleTime);
                Vector3 scale = Vector3.one * currentScale;
                transform.localScale = scale;
                particlesShape.scale = scale;
            }
        }

        private void LateUpdate()
        {
            transform.rotation = Quaternion.identity;
        }

        public void StartAttack(int dmgAmount, DamageType dmgType)
        {
            damageAmount = dmgAmount;
            damageType = dmgType;
            elapsedTime = 0f;
            currentScale = startScale;
            particlesShape.scale = Vector3.one * startScale;
            canHit = true;
            particles.Play();
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
                hitParticles.Activate(GameMan.Instance.PlayerT.position, Quaternion.identity);
            }
        }
    }
}