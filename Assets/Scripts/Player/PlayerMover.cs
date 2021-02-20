using UnityEngine;

namespace CursedWoods
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 200f;
        private Vector3 moveAmount;
        private Vector3 smoothMoveVel;
        private Vector3 velocity;
        private Vector3 forwardDir, rightDir;
        private Vector2 inputDir;

        private PlayerActionStateManager actionStateManager;

        public void Initialize(PlayerActionStateManager actionStateMan)
        {
            if (actionStateManager == null)
            {
                actionStateManager = actionStateMan;
            }
        }

        public Vector2 InputDir()
        {
            inputDir = new Vector2(Input.GetAxisRaw(CharController.HORIZONTAL), Input.GetAxisRaw(CharController.VERTICAL));
            return inputDir;
        }

        public void Movement()
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
                }
                else
                {
                    velocity = new Vector3(moveAmount.x, rbVel.y, moveAmount.z);
                }
            }
        }

        public void Move(float deltaTime)
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

        public void SpellHoldMovement()
        {
            velocity = new Vector3(0f, actionStateManager.PlayerRb.velocity.y, 0f);
        }
    }
}