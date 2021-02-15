using UnityEngine;

namespace CursedWoods
{
    public class DashState : PlayerActionStateBase
    {
        //private Vector3 velocity;
        //private bool isDashing = false;

        public override PlayerInputType Type
        {
            get
            {
                return PlayerInputType.Dash;
            }
        }

        private void Awake()
        {
            AddTargetState(PlayerInputType.None);
            AddTargetState(PlayerInputType.Move);
            AddTargetState(PlayerInputType.Attack);
            AddTargetState(PlayerInputType.Spellcast);
        }

        public override void DaUpdate()
        {
        }

        public override void DaFixedUpdate()
        {
        }
    }
}