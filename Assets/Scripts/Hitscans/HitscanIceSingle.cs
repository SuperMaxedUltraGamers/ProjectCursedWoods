using UnityEngine;

namespace CursedWoods
{
    public class HitscanIceSingle : HitscanBase
    {
        private IceRaycastParticleMover runThruParticles;
        [SerializeField]
        private ParticleSystem hitParticles;

        protected override void Awake()
        {
            Init(false, 0.75f, 50f);
            runThruParticles = GetComponentInChildren<IceRaycastParticleMover>();
            base.Awake();
        }

        public override void AfterRay(Vector3 startPos, Vector3 endPos, bool wasHit)
        {
            runThruParticles.Initialize(startPos, endPos);
            hitParticles.transform.position = endPos;
            hitParticles.Play();
        }
    }
}