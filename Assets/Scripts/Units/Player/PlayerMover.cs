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

        private float combatRotationSpeed = 0.25f;

        private PlayerActionStateManager actionStateManager;
        private Rigidbody rb;

        private delegate void CorrectDirectionsDel(float f);
        private CorrectDirectionsDel controlTypeDel;

        /*
        public PlayerControlType PlayerMoverState
        {
            get;
            set;
        } = PlayerControlType.Explore;
        */

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnDisable()
        {
            CharController.ControlTypeChanged -= MoverControlTypeChanged;
        }

        public void Initialize(PlayerActionStateManager actionStateMan)
        {
            if (actionStateManager == null)
            {
                actionStateManager = actionStateMan;
                //rb = actionStateManager.PlayerRb;
                //print("rb set");
            }

            if (controlTypeDel == null)
            {
                controlTypeDel = ExploreStateMovement;
                CharController.ControlTypeChanged += MoverControlTypeChanged;
                //print("inited");
            }

            //print("inited");
            //CharController.ControlTypeChanged += MoverControlTypeChanged;
        }

        public Vector2 InputDir()
        {
            inputDir = new Vector2(Input.GetAxisRaw(GlobalVariables.HORIZONTAL), Input.GetAxisRaw(GlobalVariables.VERTICAL));
            return inputDir;
        }

        public void Movement()
        {
            //ExploreStateCorrectDirections();
            controlTypeDel(1f);
            //ExploreVelocity(1f);
        }

        public void Move(float deltaTime)
        {
            rb.velocity = new Vector3(velocity.x * deltaTime, velocity.y, velocity.z * deltaTime);
        }

        public void HoldMovement()
        {
            velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        public void HalfSpeedMovement()
        {
            controlTypeDel(0.5f);
        }

        public Vector3 GetCorrectMoverDir()
        {
            Vector3 correctDir = rightDir * inputDir.x + forwardDir * inputDir.y;
            return correctDir.normalized;
        }

        private void MoverControlTypeChanged()
        {
            if (controlTypeDel == ExploreStateMovement)
            {
                //print("changed to combat");
                controlTypeDel = CombatStateMovement;
            }
            else
            {
                //print("changed to explore");
                controlTypeDel = ExploreStateMovement;
            }
        }

        private void ExploreStateMovement(float speedMultiplier)
        {
            ExploreStateCorrectDirections();
            ExploreVelocity(speedMultiplier);
        }

        private void CombatStateMovement(float speedMultiplier)
        {
            CombatStateCorrecDirections();
            CombatVelocity(speedMultiplier);
        }

        private void ExploreStateCorrectDirections()
        {
            // TODO: maybe create check so that these are only calculated if camera has rotated.
            forwardDir = actionStateManager.CamT.forward;
            forwardDir.y = 0f;
            forwardDir.Normalize();
            rightDir = Quaternion.Euler(0f, 90f, 0f) * forwardDir;

        }

        // Same stuff as ExploreStateCorrectDirections, but just here for future if we want different behaviour in combat state.
        private void CombatStateCorrecDirections()
        {
            ExploreStateCorrectDirections();
            /*
            forwardDir = actionStateManager.CamT.forward;
            forwardDir.y = 0f;
            forwardDir.Normalize();
            rightDir = Quaternion.Euler(new Vector3(0f, 90f, 0f)) * forwardDir;
            */
        }

        private void ExploreVelocity(float speedMultiplier)
        {
            if (inputDir.magnitude > 1f)
            {
                inputDir.Normalize();
            }

            Vector3 correctMoveDir = GetCorrectMoverDir() * moveSpeed * speedMultiplier;
            moveAmount = Vector3.SmoothDamp(moveAmount, correctMoveDir, ref smoothMoveVel, .1f);

            if (correctMoveDir.magnitude != 0f)
            {
                transform.forward = moveAmount.normalized;

                rb.isKinematic = false;
                Vector3 rbVel = rb.velocity;
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
            else
            {
                NoneStateMovement();
            }


            /*
            Vector3 rbVel = rb.velocity;
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
            */

        }

        private void CombatVelocity(float speedMultiplier)
        {
            if (inputDir.magnitude > 1f)
            {
                inputDir.Normalize();
            }

            Vector3 correctMoveDir = GetCorrectMoverDir() * moveSpeed * speedMultiplier;
            moveAmount = Vector3.SmoothDamp(moveAmount, correctMoveDir, ref smoothMoveVel, .1f);

            if (!CharController.IgnoreCameraControl)
            {
                Vector3 lookDirInput = new Vector3(Input.GetAxisRaw(GlobalVariables.HORIZONTAL_RS), 0f, Input.GetAxisRaw(GlobalVariables.VERTICAL_RS));
                Vector3 correctLookDir = rightDir * lookDirInput.x + forwardDir * lookDirInput.z;
                if (lookDirInput.magnitude != 0f)
                {
                    transform.forward = Vector3.Lerp(transform.forward, correctLookDir.normalized, Time.deltaTime * combatRotationSpeed * (Vector3.Angle(transform.forward, correctLookDir.normalized) + 1f));
                }
            }

            if (correctMoveDir.magnitude != 0f)
            {
                rb.isKinematic = false;
                Vector3 rbVel = rb.velocity;
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
            else
            {
                NoneStateMovement();
            }

            /*
            Vector3 rbVel = rb.velocity;
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
            */
        }

        private void NoneStateMovement()
        {
            velocity = rb.velocity;
            Vector3 slowedVel = velocity * 42f;
            if (actionStateManager.CharController.IsGrounded)
            {
                if (velocity.y < 0f)
                {
                    rb.isKinematic = true;
                }
                else
                {
                    velocity = new Vector3(slowedVel.x, velocity.y, slowedVel.z);
                }
            }
            else
            {
                rb.isKinematic = false;
                if (velocity.y > 0f)
                {
                    velocity = new Vector3(slowedVel.x, 0f, slowedVel.z);
                }
                else
                {
                    velocity = new Vector3(slowedVel.x, velocity.y, slowedVel.z);
                }
            }
        }
    }
}