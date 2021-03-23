namespace CursedWoods
{
    public class MaxHealthPickUp : InteractionBase
    {
        protected override void AfterInteraction()
        {
            GameMan.Instance.CharController.IncreaseHealth(GameMan.Instance.CharController.MaxHealth);
            gameObject.SetActive(false);
        }
    }
}