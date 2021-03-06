using UnityEngine;

namespace CursedWoods
{
    public class MoveState : PlayerActionStateBase
    {
        public override PlayerInputType Type
        {
            get
            {
                return PlayerInputType.Move;
            }
        }

        private void Awake()
        {
            AddTargetState(PlayerInputType.None);
            AddTargetState(PlayerInputType.Dash);
            AddTargetState(PlayerInputType.Attack);
            AddTargetState(PlayerInputType.Spellcast);

            if (mover == null)
            {
                mover = GetComponent<PlayerMover>();
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

            if (Input.GetButtonDown(GlobalVariables.DASH) && CharController.CanMoveToDash)
            {
                actionStateManager.ChangeState(PlayerInputType.Dash);
            }
            else if (Input.GetButtonDown(GlobalVariables.ATTACK))
            {
                actionStateManager.ChangeState(PlayerInputType.Attack);
            }
            else if (Input.GetButtonDown(GlobalVariables.SPELLCAST))
            {
                actionStateManager.ChangeState(PlayerInputType.Spellcast);
            }
            else if (Input.GetButtonDown(GlobalVariables.INTERACT))
            {
                actionStateManager.ChangeState(PlayerInputType.Interact);
            }
            else if (inputDir.magnitude == 0f)
            {
                actionStateManager.ChangeState(PlayerInputType.None);
            }
        }

        public override void DaUpdate()
        {
            mover.Movement();
        }

        public override void DaFixedUpdate()
        {
            mover.Move(Time.fixedDeltaTime);
        }
    }
}