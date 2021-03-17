using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class SpellHitscanBase<T> : SpellBase
        where T : Object, IHitscan, IPoolObject, ICauseDamage
    {
        #region Protected fields

        /// <summary>
        /// Current hitscan object.
        /// </summary>
        protected T hitScan = null;

        #endregion Protected fields

        #region Public API

        public override void CastSpell()
        {
            base.CastSpell();
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
        /// 
        private void DaSpellDoing()
        {
            // Calculate spawning position for the object we get from a pool.
            Vector3 spawnPos = transform.position + transform.forward * startOffset.x
                + transform.up * startOffset.y + transform.right * startOffset.z;

            // If the current hitscan is null or fading, that means it is not in use 
            // by holding type spells and so we get a new one.
            if (hitScan == null || hitScan.IsFading)
            {
                // Get object from pool, assign and activate it.
                hitScan = (T)GameMan.Instance.ObjPoolMan.GetObjectFromPool(objectPoolType);
                hitScan.InitDamageInfo(DamageAmount, DamageType);
                hitScan.Activate(spawnPos, transform.rotation);
            }

            // Separate what to do with spell by it's IsHoldingType.
            if (hitScan.IsHoldingType && !hitScan.IsFading)
            {
                hitScan.HoldRay(spawnPos, transform.rotation);
            }
            else if (!hitScan.IsHoldingType)
            {
                GameMan.Instance.CharController.PlayerAnim.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_NULL);
                hitScan.ShootRay(spawnPos, transform.rotation);
            }

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