using CursedWoods.Utils;

namespace CursedWoods
{
    public class FinalBossAmuletPickUp : InteractionBase
    {
        private RotateAndBounce rotateAndBounce;

        private void Awake()
        {
            rotateAndBounce = GetComponentInChildren<RotateAndBounce>();
            rotateAndBounce.SetOrigin(transform.position);
        }

        protected override void AfterInteraction()
        {
            GameMan.Instance.CharController.IgnoreControl = true;
            GameMan.Instance.LevelUIManager.StartFade(FadeType.FadeOut, Level.Outro);
            gameObject.SetActive(false);
        }
    }
}