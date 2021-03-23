using UnityEngine;

namespace CursedWoods.Utils
{
    public class RoateYAxis : MonoBehaviour
    {
        [SerializeField]
        private float rotSpeed = 1f;

        private void Update()
        {
            transform.rotation *= Quaternion.Euler(0f, rotSpeed * Time.deltaTime, 0f);
        }
    }
}