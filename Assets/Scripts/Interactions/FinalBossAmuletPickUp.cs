using CursedWoods.Utils;

namespace CursedWoods
{
    public class FinalBossAmuletPickUp : InteractionBase
    {
        //[SerializeField]
        //private GameObject[] disableObjects;
        //[SerializeField]
        //private string displayInfoText = "";
        //private float fadeSpeed = 1.5f;
        //private Collider hitbox;

        private RotateAndBounce rotateAndBounce;

        private void Awake()
        {
            rotateAndBounce = GetComponentInChildren<RotateAndBounce>();
            rotateAndBounce.SetOrigin(transform.position);
            //hitbox = GetComponent<Collider>();
        }

        protected override void AfterInteraction()
        {
            GameMan.Instance.CharController.IgnoreControl = true;
            //StartCoroutine(DisplayInfoText());

            //foreach (GameObject go in disableObjects)
            //{
            //    go.SetActive(false);
            //}

            //hitbox.enabled = false;
            GameMan.Instance.LevelUIManager.StartFade(FadeType.FadeOut, Level.Outro);
            gameObject.SetActive(false);
        }

        /*
        private IEnumerator DisplayInfoText()
        {
            float alpha = 2.75f;
            while (alpha > 0f)
            {
                alpha -= Time.deltaTime * fadeSpeed;
                GameMan.Instance.LevelUIManager.DisplayInfoText(displayInfoText, alpha);
                yield return null;
            }

            GameMan.Instance.LevelUIManager.StartFade(FadeType.FadeOut, Level.Outro);
            gameObject.SetActive(false);
        }
        */
    }
}