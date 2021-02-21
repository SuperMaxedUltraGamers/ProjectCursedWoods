using UnityEngine;

namespace CursedWoods
{
    public class ProjectileShockwave : ProjectileBase
    {
        [SerializeField]
        private float targetScale = 10f;
        [SerializeField]
        private float scaleSpeed = 5f;
        private float currentScale = 0.1f;
        private float targetScaleOffAmount = 0.1f;

        private void Update()
        {
            currentScale = Mathf.Lerp(currentScale, targetScale, scaleSpeed * Time.deltaTime);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            if (currentScale >= targetScale - targetScaleOffAmount)
            {
                Destroy(gameObject);
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            //TODO: Do shit for cunt that we collided with
        }
    }
}