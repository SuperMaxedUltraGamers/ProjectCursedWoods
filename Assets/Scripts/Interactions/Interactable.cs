using UnityEngine;

namespace CursedWoods
{
    public abstract class Interactable : MonoBehaviour
    {
        public abstract string InteractionText { get; }
        public abstract float Interaction();
    }
}