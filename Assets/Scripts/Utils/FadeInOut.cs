using UnityEngine;
using UnityEngine.UI;

namespace CursedWoods.Utils
{
    public class FadeInOut : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        private float fadeSpeed = 0.1f;
        private float currentFadeSpeed;

        private void Awake()
        {
            image.enabled = true;
        }

        private void Update()
        {
            currentFadeSpeed += fadeSpeed / 50 * Time.deltaTime;
            Color color = image.color;
            color.a -= currentFadeSpeed;
            image.color = color;
            if (color.a <= 0f)
            {
                image.enabled = false;
                this.enabled = false;
            }
        }

        // TODO: create methods for fade in and out and exposed boolean that determinates should we fade in or out.
    }
}