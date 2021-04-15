using UnityEngine;
using CursedWoods.Utils;
using System.Collections;

namespace CursedWoods
{
    public class KeyPickUp : InteractionBase
    {
        [SerializeField]
        private GameObject[] disableObjects;
        [SerializeField]
        private KeyType keyType;
        [SerializeField]
        private string displayInfoText = "";
        private float fadeSpeed = 1.5f;
        private Collider hitbox;

        private RotateAndBounce rotateAndBounce;

        private void Awake()
        {
            rotateAndBounce = GetComponentInChildren<RotateAndBounce>();
            rotateAndBounce.SetOrigin(transform.position);
            hitbox = GetComponent<Collider>();
        }

        private void OnEnable()
        {
        }

        protected override void AfterInteraction()
        {
            GameMan.Instance.PlayerManager.CollectedKey(keyType);
            StartCoroutine(DisplayInfoText());

            foreach (GameObject go in disableObjects)
            {
                go.SetActive(false);
            }

            hitbox.enabled = false;
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