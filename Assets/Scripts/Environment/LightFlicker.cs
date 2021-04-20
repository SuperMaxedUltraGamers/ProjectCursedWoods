using UnityEngine;

namespace CursedWoods
{
    public class LightFlicker : MonoBehaviour
    {
        private Light lightObj;
        private float targetIntensity;
        private float targetRange;
        private float changeSpeed = 8f;
        private float acceptedOffset = 0.1f;

        private void Awake()
        {
            lightObj = GetComponent<Light>();
            targetIntensity = Random.Range(0.1f, 1f);
        }

        private void Update()
        {
            float intensity = lightObj.intensity;
            lightObj.intensity = Mathf.Lerp(intensity, targetIntensity, changeSpeed * Time.deltaTime);
            lightObj.range = Mathf.Lerp(lightObj.range, targetRange, changeSpeed * Time.deltaTime);
            bool isTargetGreater = intensity < targetIntensity;
            if (isTargetGreater)
            {
                if (intensity + acceptedOffset >= targetIntensity)
                {
                    targetIntensity = Random.Range(0.1f, 1f);
                    targetRange = Random.Range(0.1f, 0.3f);
                }
            }
            else
            {
                if (intensity - acceptedOffset <= targetIntensity)
                {
                    targetIntensity = Random.Range(0.1f, 1f);
                    targetRange = Random.Range(0.1f, 0.3f);
                }
            }
        }
    }
}