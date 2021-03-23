using UnityEngine;

namespace CursedWoods
{
    public class HealthPickUp : InteractionBase
    {
        [SerializeField]
        private int healthIncreaseAmount = 300;

        protected override void AfterInteraction()
        {
            GameMan.Instance.CharController.IncreaseHealth(healthIncreaseAmount);
            gameObject.SetActive(false);
        }
    }
}