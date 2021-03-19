using UnityEngine;

namespace CursedWoods
{
    public abstract class InteractionHandler : MonoBehaviour
    {
        [SerializeField]
        private InteractionBase interaction;

        protected virtual void OnEnable()
        {
            interaction.Interacted += InteractionCause;
        }

        protected virtual void OnDisable()
        {
            interaction.Interacted -= InteractionCause;
        }

        protected abstract void InteractionCause();
    }
}