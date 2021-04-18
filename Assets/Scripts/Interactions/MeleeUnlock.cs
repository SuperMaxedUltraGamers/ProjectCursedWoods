﻿using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class MeleeUnlock : InteractionBase
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
            StartCoroutine(OpenAtStartCheck());
        }

        protected override void AfterInteraction()
        {
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

        private IEnumerator OpenAtStartCheck()
        {
            // Dirty way to wait for 2 frames to make sure loading is complete.
            yield return null;
            yield return null;

            if (!GameMan.Instance.GraveyardManager.GetGateOpenStatus(GraveyardGateType.GraveyardBookGate))
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