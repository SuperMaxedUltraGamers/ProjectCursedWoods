using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class SpellcastUnlock : InteractionBase
    {
        [SerializeField]
        private GameObject[] disableObjects;
        private Collider hitbox;
        [SerializeField]
        private string displayInfoText = "";
        private float fadeSpeed = 1.5f;

        private void Awake()
        {
            hitbox = GetComponent<Collider>();
        }

        private void Start()
        {
            StartCoroutine(CollectedAtStartCheck());
        }

        protected override void AfterInteraction()
        {
            GameMan.Instance.PlayerManager.UnlockSpellByType(Spells.Fireball);
            StartCoroutine(DisplayInfoText());
            foreach (GameObject go in disableObjects)
            {
                go.SetActive(false);
            }

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
        }

        private IEnumerator CollectedAtStartCheck()
        {
            // Dirty way to wait for 2 frames to make sure loading is complete.
            yield return null;
            yield return null;

            if (GameMan.Instance.GraveyardManager != null)
            {
                if (!GameMan.Instance.GraveyardManager.GetGateOpenStatus(GraveyardGateType.GraveyardMiddleAreaSouthGate))
                {
                    foreach (GameObject go in disableObjects)
                    {
                        go.SetActive(false);
                    }

                    hitbox.enabled = false;
                    base.AfterInteraction();
                }
            }
        }
    }
}