using UnityEngine;
using System;
using System.Collections;

namespace CursedWoods
{
    public class InteractionBase : Interactable
    {
        [SerializeField]
        protected float animTime = 1f;
        protected float afterInteractionTime = 0.1f;
        [SerializeField]
        protected string interactionText = "";

        public override string InteractionText { get { return interactionText; } }

        public event Action Interacted;

        public override float Interaction()
        {
            if (Interacted != null)
            {
                Interacted();
            }

            GameMan.Instance.CharController.PlayerAnim.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_INTERACT);
            StartCoroutine(AfterInteractionTimer());
            return animTime;
        }

        private IEnumerator AfterInteractionTimer()
        {
            yield return new WaitForSeconds(afterInteractionTime);
            AfterInteraction();
        }

        protected virtual void AfterInteraction()
        {
            this.enabled = false;
        }
    }
}