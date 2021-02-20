namespace CursedWoods
{
    public class InteractState : PlayerActionStateBase
    {
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
        }

        public override void DaUpdate()
        {
            print("InteractState");
        }
    }
}