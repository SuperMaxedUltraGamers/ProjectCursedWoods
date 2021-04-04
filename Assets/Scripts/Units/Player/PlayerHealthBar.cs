using TMPro;
using UnityEngine;

namespace CursedWoods
{
    public class PlayerHealthBar : HealthBar
    {
        [SerializeField]
        private TextMeshProUGUI textMeshProUGUI;

        private void Start()
        {
            CharController charController = GameMan.Instance.CharController;
            textMeshProUGUI.text = "" + charController.CurrentHealth + "/" + charController.MaxHealth;
        }

        protected override void HealthChange(int currentHealth, int maxHealth)
        {
            base.HealthChange(currentHealth, maxHealth);
            textMeshProUGUI.text = "" + currentHealth + "/" + maxHealth;
        }
    }
}