using UnityEngine;

namespace CursedWoods
{
    public class GroundCheck : MonoBehaviour
    {
        private float radius = 0.5f;
        private float distance = 0.6f;

        public bool RayCastGround()
        {
            if (Physics.SphereCast(transform.position, radius, -transform.up, out _, distance))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}