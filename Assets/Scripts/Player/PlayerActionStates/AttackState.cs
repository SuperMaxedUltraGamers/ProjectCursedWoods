namespace CursedWoods
{
    public class AttackState : PlayerActionStateBase
    {
        public override PlayerInputType Type
        {
            get
            {
                return PlayerInputType.Attack;
            }
        }

        private void Awake()
        {
            AddTargetState(PlayerInputType.None);
            AddTargetState(PlayerInputType.Move);
            AddTargetState(PlayerInputType.Spellcast);
        }

        public override void HandleInput()
        {
        }

        public override void DaUpdate()
        {
        }

        public override void DaFixedUpdate()
        {
        }
    }
}