using UnityEngine;

namespace CursedWoods
{
    public class MeleeUnlock : InteractionBase
    {
        [SerializeField]
        private GameObject[] disableObjects;

        protected override void AfterInteraction()
        {
            foreach (GameObject go in disableObjects)
            {
                go.SetActive(false);
            }

            GetComponent<Collider>().enabled = false;
            this.enabled = false;
        }
    }
}