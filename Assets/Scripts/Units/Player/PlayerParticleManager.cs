using UnityEngine;

namespace CursedWoods
{
    public class PlayerParticleManager : MonoBehaviour
    {
        [SerializeField]
        ParticleSystem dashParticles;

        public ParticleSystem DashParticles { get { return dashParticles; } }
    }
}