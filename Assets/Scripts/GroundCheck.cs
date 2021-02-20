using UnityEngine;


namespace CursedWoods
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField]
        private float radius = 0.5f;
        [SerializeField]
        private float distance = 1.1f;

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