using UnityEngine;

namespace CursedWoods
{
    public class MoveState : PlayerActionStateBase
    {
        private PlayerMover mover;
        /*
        private float moveSpeed = 200f;
        private Vector3 moveAmount;
        private Vector3 smoothMoveVel;
        private Vector3 velocity;
        private Vector3 forwardDir, rightDir;
        */
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

        /*
        private void Movement()
        {
            CorrectDirections();

            if (inputDir.magnitude > 1f)
            {
                inputDir.Normalize();
            }

            Vector3 correctMoveDir = (rightDir * inputDir.x + forwardDir * inputDir.y) * moveSpeed;
            moveAmount = Vector3.SmoothDamp(moveAmount, correctMoveDir, ref smoothMoveVel, .1f);

            if (correctMoveDir.magnitude != 0f)
            {
                transform.forward = moveAmount.normalized;
            }

            Vector3 rbVel = actionStateManager.PlayerRb.velocity;
            if (actionStateManager.CharController.IsGrounded)
            {
                velocity = new Vector3(moveAmount.x, rbVel.y, moveAmount.z);
            }
            else
            {
                if (rbVel.y > 0f)
                {
                    velocity = new Vector3(moveAmount.x, 0f, moveAmount.z);
                } else
                {
                    velocity = new Vector3(moveAmount.x, rbVel.y, moveAmount.z);
                }
            }
        }

        private void Move(float deltaTime)
        {
            actionStateManager.PlayerRb.velocity = new Vector3(velocity.x * deltaTime, velocity.y, velocity.z * deltaTime);
        }

        public void CorrectDirections()
        {
            // TODO: maybe create check so that these are only calculated if camera has rotated.
            forwardDir = actionStateManager.CamT.forward;
            forwardDir.y = 0f;
            forwardDir.Normalize();
            rightDir = Quaternion.Euler(new Vector3(0f, 90f, 0f)) * forwardDir;
        }
        */
    }
}