using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class SpellProjectileBase<T> : SpellBase
        where T : Object, IProjectile
    {
        [SerializeField]
        protected T projectilePrefab = null;

        public override void CastSpell()
        {
            IsCasting = true;
            if (CastTime > 0f)
            {
                StartCoroutine(CastTimer());
            }
            else
            {
                DaSpellDoing();
            }
        }

        private void DaSpellDoing()
        {
            Vector3 spawnPos = transform.position + transform.forward * startOffset.x + transform.up * startOffset.y + transform.right * startOffset.z;
            IProjectile yeetedProjectile = Instantiate(projectilePrefab, spawnPos, transform.rotation);
            yeetedProjectile.Launch();
            IsCasting = false;

            if (CoolDownTime > 0f)
            {
                IsInCoolDown = true;
                // Again bad practice to call coroutine while inside one..
                StartCoroutine(CooldownTimer());
            }
        }

        private IEnumerator CastTimer()
        {
            yield return new WaitForSeconds(CastTime);
            DaSpellDoing();
        }

        private IEnumerator CooldownTimer()
        {
            yield return new WaitForSeconds(CoolDownTime);
            IsInCoolDown = false;
        }
    }
}