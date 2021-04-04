using UnityEngine;

namespace CursedWoods.Utils
{
    public class RoateYAxis : MonoBehaviour
    {
        [SerializeField]
        private float rotSpeed = 1f;
        [SerializeField]
        private Axis rotAxis = Axis.AxisY;

        private void Update()
        {
            switch (rotAxis)
            {
                case Axis.AxisX:
                    transform.rotation *= Quaternion.Euler(rotSpeed * Time.deltaTime, 0f, 0f);
                    break;
                case Axis.AxisY:
                    transform.rotation *= Quaternion.Euler(0f, rotSpeed * Time.deltaTime, 0f);
                    break;
                case Axis.AxisZ:
                    transform.rotation *= Quaternion.Euler(0f, 0f, rotSpeed * Time.deltaTime);
                    break;
            }
        }
    }
}