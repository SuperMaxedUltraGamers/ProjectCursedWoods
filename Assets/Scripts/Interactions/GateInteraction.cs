using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class GateInteraction : InteractionBase
    {
        [SerializeField]
        private Level levelGateIsIn = Level.Graveyard;
        [SerializeField]
        private KeyType neededKey;
        [SerializeField]
        private GateType gate;
        [SerializeField]
        private string closedInfoText = "";
        [SerializeField]
        private string openInfoText = "";
        private float fadeSpeed = 1f;
        private Collider hitbox;
        private bool hasKey;

        protected override void Awake()
        {
            base.Awake();
            hitbox = GetComponent<Collider>();
        }

        private void Start()
        {
            StartCoroutine(OpenAtStartCheck());
        }

        public override float Interaction()
        {
            hasKey = GameMan.Instance.PlayerManager.GetKeyCollectedStatus(neededKey);
            hitbox.enabled = false;
            if (hasKey)
            {
                switch (levelGateIsIn)
                {
                    case Level.Graveyard:
                        GameMan.Instance.GraveyardManager.SetGateToOpenStatus(gate);
                        Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MiscSFX.GateOpen);
                        break;
                    case Level.Castle:
                        Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MiscSFX.DoorOpen);
                        GameMan.Instance.CastleManager.FinalBossDoorOpen = true;
                        GameMan.Instance.CastleManager.UseStartPos = false;
                        break;
                }

                GameMan.Instance.AutoSave();
                StartCoroutine(DisplayInfoText(openInfoText));
                return base.Interaction();
            }
            else
            {
                Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MiscSFX.Locked);
                StartCoroutine(DisplayInfoText(closedInfoText));
                return InteractionAnimation();
            }
        }

        protected override void AfterInteraction()
        {
            base.AfterInteraction();
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

        private IEnumerator OpenAtStartCheck()
        {
            // Dirty way to wait for 2 frames to make sure loading is complete.
            yield return null;
            yield return null;

            switch (levelGateIsIn)
            {
                case Level.Graveyard:
                    if (!GameMan.Instance.GraveyardManager.GetGateOpenStatus(gate))
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case Level.Castle:
                    if (GameMan.Instance.CastleManager.FinalBossDoorOpen)
                    {
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }
}