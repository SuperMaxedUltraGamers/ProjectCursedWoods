using UnityEngine;

namespace CursedWoods
{
    public class InteractionBase : Interactable
    {
        [SerializeField]
        protected float animTime = 1.5f;

        public override float Interaction()
        {
            return animTime;
        }
    }
}