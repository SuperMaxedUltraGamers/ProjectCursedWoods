using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CursedWoods
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private UnitBase unitBase;
        [SerializeField]
        private Image healthBar;
        private float changeSpeed = 0.3f;

        private void OnEnable()
        {
            unitBase.HealthChanged += HealthChange;
        }

        private void OnDisable()
        {
            unitBase.HealthChanged -= HealthChange;
        }

        private void HealthChange(int currentHealth, int maxHealth)
        {
            float healthPercent = currentHealth / (float) maxHealth;
            StartCoroutine(ChangeHealth(healthPercent));
        }

        private IEnumerator ChangeHealth(float wantedPercent)
        {
            float startPercent = healthBar.fillAmount;
            float elapsedTime = 0f;

            while (elapsedTime < changeSpeed)
            {
                elapsedTime += Time.deltaTime;
                healthBar.fillAmount = Mathf.Lerp(startPercent, wantedPercent, elapsedTime / changeSpeed);
                yield return null;
            }

            healthBar.fillAmount = wantedPercent;
        }
    }
}