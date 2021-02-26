using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public abstract class PlayerActionStateBase : MonoBehaviour, IPlayerInput
    {
        // All the possible state we can change to from this state.
        private List<PlayerInputType> possibleStates = new List<PlayerInputType>();

        protected PlayerActionStateManager actionStateManager = null;

        // Defined in child classes
        public abstract PlayerInputType Type { get; }

        public virtual void DaUpdate() { }

        public virtual void DaFixedUpdate() { }

        public virtual void HandleInput() { }

        public void Init(PlayerActionStateManager actionStateMan)
        {
            if (actionStateManager == null)
            {
                actionStateManager = actionStateMan;
            }
        }

        public virtual void TransitionIn()
        {

        }

        public virtual void TransitionOut()
        {

        }

        public bool IsValidTargetState(PlayerInputType targetState)
        {
            return possibleStates.Contains(targetState);
        }

        protected bool AddTargetState(PlayerInputType targetState)
        {
            bool canAdd = !possibleStates.Contains(targetState);
            if (canAdd)
            {
                possibleStates.Add(targetState);
            }

            return canAdd;
        }

        protected bool RemoveTargetState(PlayerInputType targetState)
        {
            return possibleStates.Remove(targetState);
        }
    }
}