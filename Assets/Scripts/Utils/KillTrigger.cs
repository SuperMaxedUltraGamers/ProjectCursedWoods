using UnityEngine;

namespace CursedWoods
{
    public class KillTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<IHealth>().DecreaseHealth(999999, DamageType.KillTrigger);
        }
    }
}