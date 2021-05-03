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
        private float fadeOutSpeed = 1f;
        private float fadeInSpeed = 0.75f;
        private Collider hitbox;
        [SerializeField]
        private Renderer meshRenderer;
        private Material glowyMat;

        protected override void Awake()
        {
            base.Awake();
            hitbox = GetComponent<Collider>();
            glowyMat = meshRenderer.materials[0];
        }

        private void Start()
        {
            StartCoroutine(GotSpellAtStartCheck());
        }

        protected override void AfterInteraction()
        {
            GameMan.Instance.PlayerManager.UnlockSpellByType(unlockSpell);
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MiscSFX.RunestoneInteract);

            StartCoroutine(DisplayInfoTextWait());
            /*
            foreach (GameObject go in disableObjects)
            {
                go.SetActive(false);
            }
            */

            hitbox.enabled = false;
            base.AfterInteraction();
        }

        private IEnumerator DisplayInfoTextWait()
        {
            float alpha = 0f;
            while (alpha < 1f)
            {
                alpha += Time.deltaTime * fadeInSpeed; // Bad hard coding to match the sfx
                GameMan.Instance.LevelUIManager.DisplayInfoText("", alpha);
                yield return null;
            }

            StartCoroutine(DisplayInfoText());
        }

        private IEnumerator DisplayInfoText()
        {
            glowyMat.SetColor("_EmissionColor", Color.black);
            float alpha = 2.75f;
            while (alpha > 0f)
            {
                alpha -= Time.deltaTime * fadeOutSpeed;
                GameMan.Instance.LevelUIManager.DisplayInfoText(displayInfoText, alpha);
                yield return null;
            }

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