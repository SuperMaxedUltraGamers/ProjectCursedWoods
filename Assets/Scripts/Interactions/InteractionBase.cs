using UnityEngine;
using System;

namespace CursedWoods
{
    public class InteractionBase : Interactable
    {
        [SerializeField]
        protected float animTime = 1f;
        //protected float afterInteractionTime = 0.1f;
        [SerializeField]
        protected string interactionText = "";
        [SerializeField]
        protected bool disableAfterInteraction = true;

        public override string InteractionText { get { return interactionText; } }

        public event Action Interacted;

        public override float Interaction()
        {
            if (Interacted != null)
            {
                Interacted();
            }

            return InteractionAnimation();
        }

        protected float InteractionAnimation()
        {
            GameMan.Instance.CharController.PlayerAnim.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_INTERACT);
            //StartCoroutine(AfterInteractionTimer());
            AfterInteraction();
            return animTime;
        }

        protected virtual void AfterInteraction()
        {
            if (disableAfterInteraction)
            {
                this.enabled = false;
            }
        }

        /*
        private IEnumerator AfterInteractionTimer()
        {
            yield return new WaitForSeconds(afterInteractionTime);
            AfterInteraction();
        }
        */
    }
}