using UnityEngine;

namespace CursedWoods.Utils
{
    public class LoadSceneTrigger : MonoBehaviour
    {
        [SerializeField]
        private Level levelToLoad;
        private bool isTriggered;
        private void OnTriggerEnter(Collider other)
        {
            if (!isTriggered)
            {
                GameMan.Instance.CharController.IgnoreControl = true;
                GameMan.Instance.LevelUIManager.StartFade(FadeType.FadeOut, levelToLoad);
                isTriggered = true;
            }
        }
    }
}