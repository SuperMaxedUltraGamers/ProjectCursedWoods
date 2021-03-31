using UnityEngine;

namespace CursedWoods
{
    public class IceRaycastParticleMover : MonoBehaviour
    {
        //[SerializeField]
        private ParticleSystem mainParticles;
        //[SerializeField]
        //private ParticleSystem trailParticles;
        private float lerpTime = 0.1f;
        private Vector3 startPos;
        private Vector3 endPos;
        private float elapsedTime;

        private void Awake()
        {
            mainParticles = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (elapsedTime < lerpTime)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / lerpTime);
            }
            else
            {
                //mainParticles.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        public void Initialize(Vector3 start, Vector3 end)
        {
            mainParticles.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
            mainParticles.Play(withChildren: true);

            startPos = start;
            endPos = end;
            elapsedTime = 0f;
        }
    }
}