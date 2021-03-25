using UnityEngine;

namespace CursedWoods.Utils
{
    public class RotateAndBounce : MonoBehaviour
    {
        [SerializeField]
        private float rotSpeed = 1f;
        [SerializeField]
        private float bounceSpeed = 1f;
        [SerializeField]
        private float bounceDistance = 1f;
        private float angle;

        private Vector3 originalPos;

        private void Awake()
        {
            originalPos = transform.position;
        }

        public void SetOrigin(Vector3 position)
        {
            originalPos = position;
        }

        private void Update()
        {
            angle += bounceSpeed * Time.deltaTime;
            float sine = Mathf.Abs(Mathf.Sin(angle) * bounceDistance);
            transform.position = originalPos + new Vector3(0f, sine, 0f);
            transform.rotation *= Quaternion.Euler(0f, rotSpeed * Time.deltaTime, 0f);
        }
    }
}