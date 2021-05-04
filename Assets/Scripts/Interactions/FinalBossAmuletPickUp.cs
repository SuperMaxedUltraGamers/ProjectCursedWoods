using CursedWoods.Utils;
using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class FinalBossAmuletPickUp : InteractionBase
    {
        [SerializeField]
        private GameObject[] disableObjects;
        private Collider hitbox;
        private RotateAndBounce rotateAndBounce;
        private float sfxLength = 2f;

        protected override void Awake()
        {
            base.Awake();
            hitbox = GetComponent<Collider>();
            rotateAndBounce = GetComponentInChildren<RotateAndBounce>();
            rotateAndBounce.SetOrigin(transform.position);
        }

        protected override void AfterInteraction()
        {
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MiscSFX.GeneralPickUp);
            GameMan.Instance.CharController.IgnoreControl = true;
            foreach (GameObject go in disableObjects)
            {
                go.SetActive(false);
            }

            hitbox.enabled = false;
            StartCoroutine(DisableGO());
            GameMan.Instance.LevelUIManager.StartFade(FadeType.FadeIn, Level.Outro);
        }

        private IEnumerator DisableGO()
        {
            yield return new WaitForSeconds(sfxLength);
            gameObject.SetActive(false);
        }
    }
}