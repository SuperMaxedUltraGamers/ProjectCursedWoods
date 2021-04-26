using UnityEngine;

namespace CursedWoods.Utils
{
    public class ChangeSceneInteraction : InteractionBase
    {
        [SerializeField]
        private Level levelToLoad;
        //private bool isTriggered;
        protected override void AfterInteraction()
        {
            GameMan.Instance.CharController.IgnoreControl = true;
            //StartCoroutine(DisplayInfoText());

            //foreach (GameObject go in disableObjects)
            //{
            //    go.SetActive(false);
            //}

            //hitbox.enabled = false;
            GameMan.Instance.LevelUIManager.StartFade(FadeType.FadeOut, levelToLoad);
            gameObject.SetActive(false);
        }

        /*
        private void OnTriggerEnter(Collider other)
        {
            if (!isTriggered)
            {
                GameMan.Instance.CharController.IgnoreControl = true;
                GameMan.Instance.LevelUIManager.StartFade(FadeType.FadeOut, levelToLoad);
                isTriggered = true;
            }
        }
        */
    }
}