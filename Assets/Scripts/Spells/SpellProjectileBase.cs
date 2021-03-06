using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    // TODO: refactor the class and it's child classes to not be generic since
    // it's not needed anymore, could potentially get rid of whole child classes too.
    public class SpellProjectileBase<T> : SpellBase
        where T : Object, IPoolObject, ICauseDamage
    {
        #region Public API

        public override void CastSpell()
        {
            IsCasting = true;
            if (CastTime > 0f)
            {
                // TODO: create timer and use that instead of coroutines.
                StartCoroutine(CastTimer());
            }
            else
            {
                DaSpellDoing();
            }
        }

        #endregion Public API

        #region Private functionality

        /// <summary>
        /// The meat and bones of what this spell does.
        /// </summary>
        private void DaSpellDoing()
        {
            // Calculate spawning position for the object we get from a pool.
            Vector3 spawnPos = transform.position + transform.forward * startOffset.x
                + transform.up * startOffset.y + transform.right * startOffset.z;

            // Get object from pool, assign and activate it.
            T projectile = (T)GameMan.Instance.ObjPoolMan.GetObjectFromPool(objectPoolType);
            projectile.InitDamageInfo(DamageAmount, DamageType);
            projectile.Activate(spawnPos, transform.rotation);
            IsCasting = false;

            // Start cooldown if the spell has cooldown.
            if (CoolDownTime > 0f)
            {
                IsInCoolDown = true;
                // Again bad practice to call coroutine while inside one..
                // TODO: create timer and use that instead of coroutines.
                StartCoroutine(CooldownTimer());
            }
        }

        /// <summary>
        /// Delay called before spell doing if the spell has casting time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CastTimer()
        {
            yield return new WaitForSeconds(CastTime);
            DaSpellDoing();
        }

        /// <summary>
        /// Delay called before spell can be casted again if the spell has cooldown time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CooldownTimer()
        {
            yield return new WaitForSeconds(CoolDownTime);
            IsInCoolDown = false;
        }

        #endregion Private functionality
    }
}