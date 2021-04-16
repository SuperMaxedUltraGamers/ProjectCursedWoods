using UnityEngine;

namespace CursedWoods
{
    public class SpellcastUnlock : InteractionBase
    {
        [SerializeField]
        private GameObject[] disableObjects;
        private Collider hitbox;

        private void Awake()
        {
            hitbox = GetComponent<Collider>();
        }

        protected override void AfterInteraction()
        {
            base.AfterInteraction();
            foreach (GameObject go in disableObjects)
            {
                go.SetActive(false);
            }

            hitbox.enabled = false;
        }
    }
}