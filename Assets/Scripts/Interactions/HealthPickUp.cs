using UnityEngine;
using CursedWoods.Utils;
using System.Collections;

namespace CursedWoods
{
    public class HealthPickUp : InteractablePoolable
    {
        [SerializeField]
        private int healthIncreaseAmount = 300;

        [SerializeField]
        private GameObject[] disableObjects;
        private Collider hitbox;
        private RotateAndBounce rotateAndBounce;
        private float sfxLength = 2.75f;

        protected override void Awake()
        {
            rotateAndBounce = GetComponentInChildren<RotateAndBounce>();
            hitbox = GetComponent<Collider>();
            base.Awake();
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot);
            foreach (GameObject go in disableObjects)
            {
                go.SetActive(true);
            }

            hitbox.enabled = true;
            rotateAndBounce.SetOrigin(pos);
        }

        protected override void AfterInteraction()
        {
            GameMan.Instance.CharController.IncreaseHealth(healthIncreaseAmount);
            Settings.Instance.Audio.PlayEffect(audioSource, Data.AudioContainer.MiscSFX.HealthPickUp, 4f);
            foreach (GameObject go in disableObjects)
            {
                go.SetActive(false);
            }

            hitbox.enabled = false;
            StartCoroutine(DisableGO());
        }

        private IEnumerator DisableGO()
        {
            yield return new WaitForSeconds(sfxLength);
            Deactivate();
        }
    }
}