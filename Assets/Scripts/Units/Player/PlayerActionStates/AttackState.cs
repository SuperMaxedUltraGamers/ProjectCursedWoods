using UnityEngine;

namespace CursedWoods
{
    public class AttackState : PlayerActionStateBase
    {
        private MeleeAttackBase attacker;

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
            AddTargetState(PlayerInputType.Dash);

            attacker = GetComponent<MeleeAttackBase>();

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
            bool isAttacking = attacker.IsAttacking;

            // Comment out if we dont want to be able to hold attack for continues strikes.
            /*
            if (Input.GetButton(GlobalVariables.ATTACK) && !isAttacking)
            {
                //print("Attacking!");
                attacker.StartAttack();
            }
            else
            */
            if (Input.GetButtonDown(GlobalVariables.SPELLCAST) && !isAttacking)
            {
                actionStateManager.ChangeState(PlayerInputType.Spellcast);
            }
            else if (Input.GetButtonDown(GlobalVariables.DASH) && !isAttacking && GameMan.Instance.CharController.CanMoveToDash)
            {
                actionStateManager.ChangeState(PlayerInputType.Dash);
            }
            else if (inputDir.magnitude != 0f && !isAttacking)
            {
                actionStateManager.ChangeState(PlayerInputType.Move);
            }
            else if (inputDir.magnitude == 0f && !isAttacking)
            {
                actionStateManager.ChangeState(PlayerInputType.None);
            }
        }

        public override void DaUpdate()
        {
            switch (attacker.MoveType)
            {
                case PlayerMoveType.Hold:
                    if (attacker.IsAttacking)
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

            mover.Move(Time.deltaTime);
            //elapsedTime += Time.deltaTime;
        }

        /*
        public override void DaFixedUpdate()
        {
            mover.Move(Time.fixedDeltaTime);
        }
        */

        public override void TransitionIn()
        {
            if (!attacker.IsAttacking)
            {
                attacker.StartAttack();
            }

            //elapsedTime = 0f;
        }

        //private float elapsedTime = 0f;

        public override void TransitionOut()
        {
            //print(elapsedTime);
        }
    }
}