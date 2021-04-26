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
        //[SerializeField]
        //private MeshRenderer meshRenderer;
        //private Material[] materials;
        //private Material runestoneMat;

        private void Awake()
        {
            hitbox = GetComponent<Collider>();
            //materials = meshRenderer.materials;
            //runestoneMat = materials[1];
        }

        private void Start()
        {
            StartCoroutine(GotSpellAtStartCheck());
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

            //meshRenderer.materials[0] = runestoneMat;
            gameObject.SetActive(false);
        }

        private IEnumerator GotSpellAtStartCheck()
        {
            // Dirty way to wait for 2 frames to make sure loading is complete.
            yield return null;
            yield return null;

            if (GameMan.Instance.PlayerManager.GetSpellLockStatus(unlockSpell))
            {
                gameObject.SetActive(false);
            }
        }
    }
}