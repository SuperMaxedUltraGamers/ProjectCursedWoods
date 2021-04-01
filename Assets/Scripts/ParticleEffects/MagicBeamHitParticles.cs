using UnityEngine;

namespace CursedWoods
{
    public class MagicBeamHitParticles : MonoBehaviour
    {
        private ParticleSystem hitParticles;

        private void Awake()
        {
            hitParticles = GetComponent<ParticleSystem>();
        }

        public void ActivateHitParticles(Vector3 pos)
        {
            transform.position = pos;
            if (!hitParticles.isPlaying)
            {
                hitParticles.Play();
            }
        }

        public void DisableHitParticles()
        {
            if (hitParticles.isPlaying)
            {
                hitParticles.Stop();
            }
        }
    }
}