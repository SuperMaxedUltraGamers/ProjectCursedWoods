using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class InteractState : PlayerActionStateBase
    {
        private PlayerInputType nextState = PlayerInputType.None;
        private float interactionTime = 1.5f;
        private bool isInteracting;

        public override PlayerInputType Type
        {
            get
            {
                return PlayerInputType.Interact;
            }
        }

        private void Awake()
        {
            AddTargetState(PlayerInputType.None);
            AddTargetState(PlayerInputType.Move);
            AddTargetState(PlayerInputType.Attack);
            AddTargetState(PlayerInputType.Spellcast);

            if (mover == null)
            {
                mover = GetComponent<NewPlayerMover>();
            }
        }

        private void Start()
        {
            // Null check inside the method so no worries.
            mover.Initialize(actionStateManager);
        }

        public override void HandleInput()
        {
            inputDir = mover.InputDir();
            if (Input.GetButtonDown(GlobalVariables.ATTACK))
            {
                nextState = PlayerInputType.Attack;
            }
            else if (Input.GetButtonDown(GlobalVariables.SPELLCAST))
            {
                nextState = PlayerInputType.Spellcast;
            }
            else if (inputDir.magnitude != 0f)
            {
                nextState = PlayerInputType.Move;
            }
            else if (inputDir.magnitude == 0f)
            {
                nextState = PlayerInputType.None;
            }
        }

        /*
        public override void DaFixedUpdate()
        {
            Vector3 vel = actionStateManager.PlayerRb.velocity;
            vel.x = 0f;
            vel.z = 0f;
            actionStateManager.PlayerRb.velocity = vel;
        }
        */

        public override void TransitionIn()
        {
            if (!isInteracting)
            {
                isInteracting = true;
                CharController charController = GameMan.Instance.CharController;
                Collider[] colliders = Physics.OverlapSphere(transform.position, charController.InteractRadius, charController.InteractableMask);
                if (colliders.Length > 0)
                {
                    if (colliders[0] != null)
                    {
                        InteractionBase interaction = colliders[0].GetComponent<InteractionBase>();
                        // Get the animation/interactionTime from the interaction since the interaction object is responsible to play the correct animation.
                        interactionTime = interaction.Interaction();

                        StartCoroutine(InteractTimer());
                    }
                    else
                    {
                        actionStateManager.ChangeState(nextState);
                    }
                }
                else
                {
                    actionStateManager.ChangeState(nextState);
                }
            }
        }

        public override void TransitionOut()
        {
            isInteracting = false;
        }

        private IEnumerator InteractTimer()
        {
            yield return new WaitForSeconds(interactionTime);
            GameMan.Instance.CharController.PlayerAnim.SetInteger(GlobalVariables.UNIQUE_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_NULL);
            actionStateManager.ChangeState(nextState);
        }
    }
}