using UnityEngine;

namespace CursedWoods
{
    public class SpellcastState : PlayerActionStateBase
    {
        private SpellCaster caster;
        private PlayerMover mover;

        private Vector2 inputDir;

        public override PlayerInputType Type
        {
            get
            {
                return PlayerInputType.Spellcast;
            }
        }

        private void Awake()
        {
            AddTargetState(PlayerInputType.None);
            AddTargetState(PlayerInputType.Move);
            AddTargetState(PlayerInputType.Dash);
            AddTargetState(PlayerInputType.Attack);

            caster = GetComponent<SpellCaster>();
            if (mover == null)
            {
                mover = GetComponent<PlayerMover>();
            }
        }

        private void Start()
        {
            mover.Initialize(actionStateManager);
        }

        public override void DaUpdate()
        {
            switch(caster.CurrentSpell.SpellMoveType)
            {
                case SpellMoveType.Hold:
                    if (caster.CurrentSpell.IsCasting)
                    {
                        mover.SpellHoldMovement();
                    }
                    else
                    {
                        mover.Movement();
                    }
                    break;
                case SpellMoveType.Restricted:
                    // TODO: add restricted moving
                    break;
                case SpellMoveType.Free:
                    mover.Movement();
                    break;
            }
        }

        public override void DaFixedUpdate()
        {
            mover.Move(Time.fixedDeltaTime);
        }

        public override void HandleInput()
        {
            inputDir = mover.InputDir();
            bool isCasting = caster.CurrentSpell.IsCasting;

            if (Input.GetButton(CharController.SPELLCAST) && !isCasting)
            {
                caster.CastSpell();
            }
            else if (Input.GetButtonDown(CharController.ATTACK) && !isCasting)
            {
                actionStateManager.ChangeState(PlayerInputType.Attack);
            }
            else if (Input.GetButtonDown(CharController.DASH) && !isCasting)
            {
                actionStateManager.ChangeState(PlayerInputType.Dash);
            }
            else if (Input.GetButtonDown(CharController.INTERACT) && !isCasting)
            {
                actionStateManager.ChangeState(PlayerInputType.Interact);
            }
            else if (inputDir.magnitude != 0f && !isCasting)
            {
                actionStateManager.ChangeState(PlayerInputType.Move);
            }
            else if (inputDir.magnitude == 0f && !isCasting)
            {
                actionStateManager.ChangeState(PlayerInputType.None);
            }
        }
    }
}