using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class SpellHitscanBase<T> : SpellBase
        where T : Object, IHitscan
    {
        [SerializeField]
        protected T hitScanPrefab = null;
        protected IHitscan hitScan = null;

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
            if (hitScan == null || hitScan.IsFading)
            {
                hitScan = Instantiate(hitScanPrefab, spawnPos, transform.rotation);
            }

            if (hitScan.IsHoldingType && !hitScan.IsFading)
            {
                hitScan.HoldRay(spawnPos, transform.rotation);
            }
            else if (!hitScan.IsHoldingType)
            {
                print("ShotRay");
                hitScan.ShootRay();
            }

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