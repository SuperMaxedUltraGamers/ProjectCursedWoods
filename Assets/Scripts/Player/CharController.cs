using UnityEngine;
using System.Collections;

namespace CursedWoods
{
    public class CharController : MonoBehaviour
    {
        // TODO: Create and move these to some static and constant class or struct.
        public const string HORIZONTAL = "Horizontal";
        public const string VERTICAL = "Vertical";
        public const string DASH = "Dash";
        public const string ATTACK = "Attack";
        public const string SPELLCAST = "Spellcast";
        public const string INTERACT = "Interact";

        /*
        [SerializeField]
        private float moveSpeed = 200f;
        private Vector3 moveAmount;
        private Vector3 smoothMoveVel;
        private Vector3 velocity;
        private Vector3 forwardDir, rightDir;
        */

        /*
        public Rigidbody PlayerRb
        {
            get;
            private set;
        }

        public Transform CamT
        {
            get;
            private set;
        }
        */

        //public event Action<Rigidbody, float> MoveEvent;

        private void Awake()
        {
            //PlayerRb = GetComponent<Rigidbody>();
            //CamT = Camera.main.transform;
            //CorrectDirections();
        }

        /*
        private void Update()
        {
            //Movement();
        }

        private void FixedUpdate()
        {
            // If state machine shit fucks up uncomment this.
            // Move(Time.fixedDeltaTime);
            // CameraMove(Time.fixedDeltaTime);
        }
        */

        /*
        private void Movement()
        {
            CorrectDirections();

            Vector2 inputDir = new Vector2(Input.GetAxisRaw(HORIZONTAL), Input.GetAxisRaw(VERTICAL));
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

            velocity = new Vector3(moveAmount.x, playerRb.velocity.y, moveAmount.z);
        }
        */

        // Unused now since refactored so that camera is independent.
        /*
        private void Move(float deltaTime)
        {
            rb.velocity = new Vector3(velocity.x * deltaTime, velocity.y, velocity.z * deltaTime);
            // If state machine shit fucks up uncomment this.
            // CameraMove(deltaTime);
        }

        private void CameraMove(float deltaTime)
        {
            // Calls CamController's FollowPlayer method
            MoveEvent?.Invoke(rb, deltaTime);
        }
        */

        /*
        public void CorrectDirections()
        {
            // TODO: maybe create check so that these are only calculated if camera has rotated.
            forwardDir = camT.forward;
            forwardDir.y = 0f;
            forwardDir.Normalize();
            rightDir = Quaternion.Euler(new Vector3(0f, 90f, 0f)) * forwardDir;
        }
        */
    }
}