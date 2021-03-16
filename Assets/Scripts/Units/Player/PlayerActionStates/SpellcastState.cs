using UnityEngine;

namespace CursedWoods
{
    public class SpellcastState : PlayerActionStateBase
    {
        private SpellCaster caster;

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
            // Null check inside the method so no worries.
            mover.Initialize(actionStateManager);
        }

        public override void DaUpdate()
        {
            switch (caster.CurrentSpell.SpellMoveType)
            {
                case PlayerMoveType.Hold:
                    if (caster.CurrentSpell.IsCasting)
                    {
                        mover.HoldMovement();
                    }
                    else
                    {
                        mover.Movement();
                    }

                    break;
                case PlayerMoveType.HalfSpeed:
                    mover.HalfSpeedMovement();
                    break;
                case PlayerMoveType.Free:
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

            if (Input.GetButton(GlobalVariables.SPELLCAST) && !isCasting)
            {
                caster.CastSpell();
            }
            else if (Input.GetButtonDown(GlobalVariables.ATTACK) && !isCasting)
            {
                actionStateManager.ChangeState(PlayerInputType.Attack);
            }
            else if (Input.GetButtonDown(GlobalVariables.DASH) && !isCasting && CharController.CanMoveToDash)
            {
                actionStateManager.ChangeState(PlayerInputType.Dash);
            }
            else if (Input.GetButtonDown(GlobalVariables.INTERACT) && !isCasting)
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

        public override void TransitionIn()
        {
            CharController.PlayerAnim.SetInteger(GlobalVariables.PLAYER_ANIM_TORSO_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_SPELLCAST);
        }

        public override void TransitionOut()
        {
            CharController.PlayerAnim.SetInteger(GlobalVariables.PLAYER_ANIM_TORSO_ANIM_VALUE, GlobalVariables.PLAYER_ANIM_NULL);
        }
    }
}