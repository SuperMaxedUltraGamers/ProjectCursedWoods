using UnityEngine;
using CursedWoods.Utils;

namespace CursedWoods
{
    public class ParticleEffectBase : PoolObjectBase
    {
        [SerializeField]
        private float lifeTime = 2f;
        [SerializeField]
        private Vector3 rotOffset;
        private Timer lifeTimeTimer;
        [SerializeField]
        private ParticleSystem mainParticles;

        protected virtual void Awake()
        {
            lifeTimeTimer = gameObject.AddComponent<Timer>();
            lifeTimeTimer.Set(lifeTime);
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            lifeTimeTimer.TimerCompleted += Deactivate;
        }

        private void OnDisable()
        {
            lifeTimeTimer.TimerCompleted -= Deactivate;
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot * Quaternion.Euler(rotOffset));
            mainParticles.Play(true);
            lifeTimeTimer.Run();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            mainParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}