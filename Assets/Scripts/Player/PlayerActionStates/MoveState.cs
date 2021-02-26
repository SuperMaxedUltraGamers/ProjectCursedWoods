using UnityEngine;

namespace CursedWoods
{
    public class MoveState : PlayerActionStateBase
    {
        private PlayerMover mover;
        private Vector2 inputDir;

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
            mover.Initialize(actionStateManager);
        }

        public override void HandleInput()
        {
            inputDir = mover.InputDir();

            if (Input.GetButtonDown(CharController.DASH))
            {
                actionStateManager.ChangeState(PlayerInputType.Dash);
            }
            else if (Input.GetButtonDown(CharController.ATTACK))
            {
                actionStateManager.ChangeState(PlayerInputType.Attack);
            }
            else if (Input.GetButtonDown(CharController.SPELLCAST))
            {
                actionStateManager.ChangeState(PlayerInputType.Spellcast);
            }
            else if (Input.GetButtonDown(CharController.INTERACT))
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