using UnityEngine;

namespace CursedWoods
{
    public class HitscanMagicBeam : HitscanBase
    {
        private MagicBeamHitParticles hitParticles;
        protected override void Awake()
        {
            Init(true, 15f, 25f);
            hitParticles = GetComponentInChildren<MagicBeamHitParticles>();
            base.Awake();
        }

        public override void AfterRay(Vector3 startPos, Vector3 endPos, bool wasHit)
        {
            if (wasHit)
            {
                hitParticles.ActivateHitParticles(endPos);
            }
            else
            {
                hitParticles.DisableHitParticles();
            }
        }
    }
}