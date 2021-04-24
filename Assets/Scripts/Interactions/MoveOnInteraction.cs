using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class MoveOnInteraction : InteractionHandlerArray
    {
        [SerializeField]
        private GraveyardGateType gateType;
        [SerializeField]
        private float raiseAmount = 4f;
        [SerializeField]
        private float moveSpeed = 0.2f;
        [SerializeField]
        private Axis moveAxis = Axis.AxisY;
        [SerializeField, Tooltip("Should we change the move direction after each time interacted, e.g. gate.")]
        private bool toggleMoveDirectionAfter = true;

        private bool isFlipped;
        // TODO: rename since doesnt really tell if is moving or not, maybe could get rid of totally
        private bool isMoving;
        private float currentLerpPos;
        private readonly float acceptedOffMargin = 0.1f;

        private Vector3 targetPos;
        private Vector3 originalPos;
        private Vector3 addedMovement;

        private void Awake()
        {
            originalPos = transform.position;
            switch (moveAxis)
            {
                case Axis.AxisX:
                    //targetRot = orignalRot * Quaternion.Euler(rotateAmount, 0f, 0f);
                    addedMovement = new Vector3(raiseAmount, 0f, 0f);
                    break;
                case Axis.AxisY:
                    //targetRot = orignalRot * Quaternion.Euler(0f, rotateAmount, 0f);
                    addedMovement = new Vector3(0f, raiseAmount, 0f);
                    break;
                case Axis.AxisZ:
                    //targetRot = orignalRot * Quaternion.Euler(0f, 0f, rotateAmount);
                    addedMovement = new Vector3(0f, 0f, raiseAmount);
                    break;
            }

            targetPos = originalPos + addedMovement;
        }

        private void Start()
        {
            StartCoroutine(OpenAtStartCheck());
        }

        protected override void InteractionCause()
        {
            if (toggleMoveDirectionAfter)
            {
                if (isFlipped)
                {
                    targetPos = originalPos;
                    isFlipped = false;
                }
                else
                {
                    targetPos = originalPos + addedMovement;
                    isFlipped = true;
                }
            }
            else
            {
                // Only to check if this object has moved once yet
                // If not checked then we would move twice the moveAmount on the first move.
                if (isMoving)
                {
                    targetPos += addedMovement;
                }
            }

            isMoving = true;
            currentLerpPos = 0f;
            StartCoroutine(Move());
        }

        private IEnumerator OpenAtStartCheck()
        {
            // Dirty way to wait for 2 frames to make sure loading is complete.
            yield return null;
            yield return null;

            if (!GameMan.Instance.GraveyardManager.GetGateOpenStatus(gateType))
            {
                InteractionCause();
            }
        }

        private IEnumerator Move()
        {
            while (currentLerpPos < 1f - acceptedOffMargin)
            {
                currentLerpPos += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(transform.position, targetPos, currentLerpPos);
                /*
                if (currentLerpPos >= 1f - acceptedOffMargin)
                {
                    isMoving = false;
                }
                */

                yield return null;
            }
        }
    }
}