using UnityEngine;

namespace CursedWoods
{
    public class MaxHealthIncrease : InteractionBase
    {
        [SerializeField, Range(0.01f, 1), Tooltip("The percent of the current max health, which then will be added to the max health.")]
        private float maxHealthIncreasePercent = 0.1f;

        protected override void AfterInteraction()
        {
            GameMan.Instance.CharController.IncreaseMaxHealth((int) (GameMan.Instance.CharController.MaxHealth * maxHealthIncreasePercent));
            gameObject.SetActive(false);
        }
    }
}