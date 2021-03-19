using UnityEngine;

namespace CursedWoods
{
    public abstract class InteractionHandlerArray : MonoBehaviour
    {
        [SerializeField]
        private InteractionBase[] interactions;

        protected virtual void OnEnable()
        {
            if (interactions.Length > 0)
            {
                foreach (InteractionBase interaction in interactions)
                {
                    interaction.Interacted += InteractionCause;
                }
            }
        }

        protected virtual void OnDisable()
        {
            if (interactions.Length > 0)
            {
                foreach (InteractionBase interaction in interactions)
                {
                    interaction.Interacted -= InteractionCause;
                }
            }
        }

        protected abstract void InteractionCause();
    }
}