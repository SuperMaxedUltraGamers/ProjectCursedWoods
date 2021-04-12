using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class GateInteraction : InteractionBase
    {
        [SerializeField]
        private KeyType neededKey;
        [SerializeField]
        private string closedInfoText = "";
        [SerializeField]
        private string openInfoText = "";
        private float fadeSpeed = 1f;
        private Collider hitbox;
        private bool hasKey;

        private void Awake()
        {
            hitbox = GetComponent<Collider>();
        }

        public override float Interaction()
        {
            hasKey = GameMan.Instance.PlayerManager.GetKeyCollectedStatus(neededKey);
            hitbox.enabled = false;
            if (hasKey)
            {
                StartCoroutine(DisplayInfoText(openInfoText));
                return base.Interaction();
            }
            else
            {
                StartCoroutine(DisplayInfoText(closedInfoText));
                return InteractionAnimation();
            }
        }

        private IEnumerator DisplayInfoText(string text)
        {
            float alpha = 2.5f;
            while (alpha > 0f)
            {
                alpha -= Time.deltaTime * fadeSpeed;
                GameMan.Instance.LevelUIManager.DisplayInfoText(text, alpha);
                yield return null;
            }

            if (hasKey)
            {
                gameObject.SetActive(false);
            }
            else
            {
                hitbox.enabled = true;
            }
        }
    }
}