using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace CursedWoods
{
    public class BossHealthBar : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textMeshProUGUI;

        private UnitBase currentBoss;
        [SerializeField]
        private Image healthBar;
        private float changeSpeed = 0.24f;

        private string bossName = "";

        public void ConfigureBossHealthBar(UnitBase bossUnit, string bossName, int currentHealth, int maxHealth)
        {
            currentBoss = bossUnit;
            this.bossName = bossName;
            HealthChange(currentHealth, maxHealth);
            currentBoss.HealthChanged += HealthChange;
        }

        public void DisableBossHealthBar()
        {
            currentBoss.HealthChanged -= HealthChange;
            currentBoss = null;
            bossName = "";
        }

        private void HealthChange(int currentHealth, int maxHealth)
        {
            float healthPercent = currentHealth / (float)maxHealth;
            textMeshProUGUI.text = bossName + " " + currentHealth + "/" + maxHealth;
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