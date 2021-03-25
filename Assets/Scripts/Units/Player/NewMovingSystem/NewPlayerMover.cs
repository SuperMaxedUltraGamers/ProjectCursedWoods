﻿using UnityEngine;

namespace CursedWoods
{
    public class NewPlayerMover : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 5f;
        private Vector3 moveAmount;
        private Vector3 smoothMoveVel;
        private Vector3 velocity;
        private Vector3 forwardDir, rightDir;
        private Vector2 inputDir;
        private float gravity = 0.2f;

        private CharacterController characterController;

        private float combatRotationSpeed = 0.25f;

        private PlayerActionStateManager actionStateManager;

        private delegate void CorrectDirectionsDel(float f);
        private CorrectDirectionsDel controlTypeDel;

        public Vector3 Velocity { get { return velocity; } }

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void OnDisable()
        {
            if (GameMan.Instance != null)
            {
                GameMan.Instance.CharController.ControlTypeChanged -= MoverControlTypeChanged;
            }
        }

        public void Initialize(PlayerActionStateManager actionStateMan)
        {
            if (actionStateManager == null)
            {
                actionStateManager = actionStateMan;
            }

            if (controlTypeDel == null)
            {
                controlTypeDel = ExploreStateMovement;
                GameMan.Instance.CharController.ControlTypeChanged += MoverControlTypeChanged;
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
            controlTypeDel(1f);
        }

        public void Move(float deltaTime)
        {
            characterController.Move(velocity * deltaTime);
        }

        public void HoldMovement()
        {
            velocity.x = 0f;
            velocity.z = 0f;
        }

        public void HalfSpeedMovement()
        {
            controlTypeDel(0.5f);
        }

        public Vector3 GetCorrectMoverDir()
        {
            Vector3 correctDir = rightDir * inputDir.x + forwardDir * inputDir.y;
            return correctDir;
        }

        public void DashMovement(Vector3 moveAmount)
        {
            characterController.Move(moveAmount);
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

            AnimationBlend(Mathf.Abs(inputDir.magnitude) * 2f);

            Vector3 correctMoveDir = GetCorrectMoverDir();
            if (correctMoveDir.magnitude > 1f)
            {
                correctMoveDir.Normalize();
            }

            Vector3 newMoveAmount = correctMoveDir * moveSpeed * speedMultiplier;
            moveAmount = Vector3.SmoothDamp(moveAmount, newMoveAmount, ref smoothMoveVel, .1f);

            if (newMoveAmount.magnitude != 0f)
            {
                transform.forward = moveAmount.normalized;

                if (GameMan.Instance.CharController.IsGrounded)
                {
                    velocity = new Vector3(moveAmount.x, 0f, moveAmount.z);
                }
                else
                {
                    velocity.y -= gravity;
                }

            }
            else
            {
                NoneStateMovement();
            }
        }

        private void CombatVelocity(float speedMultiplier)
        {
            if (inputDir.magnitude > 1f)
            {
                inputDir.Normalize();
            }

            AnimationBlend(Mathf.Abs(inputDir.magnitude) * 2f);

            Vector3 correctMoveDir = GetCorrectMoverDir();
            if (correctMoveDir.magnitude > 1f)
            {
                correctMoveDir.Normalize();
            }

            Vector3 newMoveAmount = correctMoveDir * moveSpeed * speedMultiplier;
            moveAmount = Vector3.SmoothDamp(moveAmount, newMoveAmount, ref smoothMoveVel, .1f);

            if (!GameMan.Instance.CharController.IgnoreCameraControl)
            {
                Vector3 lookDirInput = new Vector3(Input.GetAxisRaw(GlobalVariables.HORIZONTAL_RS), 0f, Input.GetAxisRaw(GlobalVariables.VERTICAL_RS));
                Vector3 correctLookDir = rightDir * lookDirInput.x + forwardDir * lookDirInput.z;
                if (lookDirInput.magnitude != 0f)
                {
                    Vector3 transForward = transform.forward;
                    transform.forward = Vector3.Lerp(transForward, correctLookDir.normalized, Time.deltaTime * combatRotationSpeed * (Vector3.Angle(transForward, correctLookDir.normalized) + 1f));
                }
            }

            if (newMoveAmount.magnitude != 0f)
            {
                if (GameMan.Instance.CharController.IsGrounded)
                {
                    velocity = new Vector3(moveAmount.x, 0f, moveAmount.z);
                }
                else
                {
                    velocity.y -= gravity;
                }

            }
            else
            {
                NoneStateMovement();
            }
        }

        private void NoneStateMovement()
        {
            if (GameMan.Instance.CharController.IsGrounded)
            {
                velocity *= 0.8f;
            }
            else
            {
                velocity.x *= 0.98f;
                velocity.z *= 0.98f;
                velocity.y -= gravity;
            }
        }

        private void AnimationBlend(float blendValue)
        {
            GameMan.Instance.CharController.PlayerAnim.SetFloat("Blend", blendValue);
            GameMan.Instance.CharController.PlayerAnim.SetFloat("TorsoBlend", blendValue);
        }
    }
}