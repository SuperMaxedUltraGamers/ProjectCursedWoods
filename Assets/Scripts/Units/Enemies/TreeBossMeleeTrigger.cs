using UnityEngine;

namespace CursedWoods
{
    public class TreeBossMeleeTrigger : MonoBehaviour
    {
        private int currentAttackDmg;
        private DamageType attackDmgType;
        private Collider hitbox;

        private void Awake()
        {
            hitbox = GetComponent<Collider>();
        }

        public void SetDmg(int currentDmg, DamageType dmgType)
        {
            hitbox.enabled = true;
            currentAttackDmg = currentDmg;
            attackDmgType = dmgType;
        }

        public void DisableCollider()
        {
            hitbox.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!TreeBoss.hasMeleeHit)
            {
                if (other.gameObject.layer == GlobalVariables.PLAYER_LAYER)
                {
                    IHealth otherHealth = other.GetComponent<IHealth>();
                    if (otherHealth == null)
                    {
                        otherHealth = other.GetComponentInParent<IHealth>();
                    }

                    otherHealth.DecreaseHealth(currentAttackDmg, attackDmgType);
                    TreeBoss.hasMeleeHit = true;

                    ParticleEffectBase hitParticles = (ParticleEffectBase)GameMan.Instance.ObjPoolMan.GetObjectFromPool(ObjectPoolType.MeleeHitParticles);
                    hitParticles.Activate(GameMan.Instance.PlayerT.position, Quaternion.identity);
                }
            }
        }
    }
}