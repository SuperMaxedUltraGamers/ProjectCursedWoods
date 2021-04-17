using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class SpellUnlock : InteractionBase
   {
        //[SerializeField]
        //private GameObject[] disableObjects;
        [SerializeField]
        private Spells unlockSpell;
        [SerializeField]
        private string displayInfoText = "";
        private float fadeSpeed = 1.5f;
        private Collider hitbox;


        private void Awake()
        {
            hitbox = GetComponent<Collider>();
        }

        protected override void AfterInteraction()
        {
            GameMan.Instance.PlayerManager.UnlockSpellByType(unlockSpell);
            StartCoroutine(DisplayInfoText());

            /*
            foreach (GameObject go in disableObjects)
            {
                go.SetActive(false);
            }
            */

            hitbox.enabled = false;
            base.AfterInteraction();
        }

        private IEnumerator DisplayInfoText()
        {
            float alpha = 2.75f;
            while (alpha > 0f)
            {
                alpha -= Time.deltaTime * fadeSpeed;
                GameMan.Instance.LevelUIManager.DisplayInfoText(displayInfoText, alpha);
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}