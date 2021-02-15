using UnityEngine;

namespace CursedWoods
{
    public class NoneState : PlayerActionStateBase
    {
        private Vector3 velocity;

        public override PlayerInputType Type
        {
            get
            {
                return PlayerInputType.None;
            }
        }

        private void Awake()
        {
            AddTargetState(PlayerInputType.Move);
            AddTargetState(PlayerInputType.Dash);
            AddTargetState(PlayerInputType.Attack);
            AddTargetState(PlayerInputType.Spellcast);
        }

        public override void HandleInput()
        {
            Vector2 inputDir = new Vector2(Input.GetAxisRaw(CharController.HORIZONTAL), Input.GetAxisRaw(CharController.VERTICAL));
            if (inputDir.magnitude != 0f)
            {
                actionStateManager.ChangeState(PlayerInputType.Move);
            }
            else if (Input.GetButtonDown(CharController.DASH))
            {
                actionStateManager.ChangeState(PlayerInputType.Dash);
            }
        }

        public override void DaUpdate()
        {
            velocity = actionStateManager.PlayerRb.velocity;
            Vector3 slowedVel = velocity * 42f;
            velocity = new Vector3(slowedVel.x, velocity.y, slowedVel.z);
        }

        public override void DaFixedUpdate()
        {
            // Is the fucking deltatime necessary inside FixedUpdate???
            actionStateManager.PlayerRb.velocity = new Vector3(velocity.x * Time.fixedDeltaTime, velocity.y, velocity.z * Time.fixedDeltaTime);
        }
    }
}