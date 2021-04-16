using UnityEngine;

namespace CursedWoods
{
    public class KillTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            IHealth otherHealth = other.GetComponent<IHealth>();
            if (otherHealth == null)
            {
                otherHealth = other.GetComponentInParent<IHealth>();
            }

            otherHealth.DecreaseHealth(999999, DamageType.KillTrigger);
        }
    }
}