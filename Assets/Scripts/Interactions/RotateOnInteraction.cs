using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class RotateOnInteraction : InteractionHandlerArray
    {
        //[SerializeField, Tooltip("Leave unassigned if no need to use!")]
        //private GraveyardManager graveyardMan;
        [SerializeField]
        private GraveyardGateType gateType;
        [SerializeField]
        private float rotateAmount = 90f;
        [SerializeField]
        private float rotSpeed = 1f;
        [SerializeField]
        private Axis rotationAxis = Axis.AxisY;
        [SerializeField, Tooltip("Should we change the rotation direction after each time interacted, e.g. door.")]
        private bool toggleRotationDirAfter = true;

        /// <summary>
        /// Used to decide which should be our target rotation if we are using toggleRotationDirAfter.
        /// </summary>
        private bool isFlipped;
        // TODO: rename since doesnt really tell if is rotating or not, maybe could get rid of totally
        private bool isRotating;
        private float currentLerpPos;
        private float acceptedOffMargin = 0.1f;

        private Quaternion targetRot;
        private Quaternion orignalRot;
        private Quaternion addedRot;

        private void Awake()
        {
            orignalRot = transform.rotation;
            switch (rotationAxis)
            {
                case Axis.AxisX:
                    //targetRot = orignalRot * Quaternion.Euler(rotateAmount, 0f, 0f);
                    addedRot = Quaternion.Euler(rotateAmount, 0f, 0f);
                    break;
                case Axis.AxisY:
                    //targetRot = orignalRot * Quaternion.Euler(0f, rotateAmount, 0f);
                    addedRot = Quaternion.Euler(0f, rotateAmount, 0f);
                    break;
                case Axis.AxisZ:
                    //targetRot = orignalRot * Quaternion.Euler(0f, 0f, rotateAmount);
                    addedRot = Quaternion.Euler(0f, 0f, rotateAmount);
                    break;
            }

            targetRot = orignalRot * addedRot;
        }

        private void Start()
        {
            StartCoroutine(OpenAtStartCheck());
        }

        //protected override void OnEnable()
        //{
        //    base.OnEnable();
        //    /*
        //    if (graveyardMan != null)
        //    {
        //        switch (gateType)
        //        {
        //            case GraveyardGate.SwordGate:
        //                graveyardMan.SwordGateEvent += InteractionCause;
        //                break;
        //            case GraveyardGate.BookGate:
        //                graveyardMan.SpellbookGateEvent += InteractionCause;
        //                break;
        //            case GraveyardGate.MiddleAreaGate:
        //                graveyardMan.MiddleAreaGateEvent += InteractionCause;
        //                break;
        //        }
        //    }
        //    */
        //}

        //protected override void OnDisable()
        //{
        //    base.OnDisable();
        //    /*
        //    if (graveyardMan != null)
        //    {
        //        switch (gateType)
        //        {
        //            case GraveyardGate.SwordGate:
        //                graveyardMan.SwordGateEvent -= InteractionCause;
        //                break;
        //            case GraveyardGate.BookGate:
        //                graveyardMan.SpellbookGateEvent -= InteractionCause;
        //                break;
        //            case GraveyardGate.MiddleAreaGate:
        //                graveyardMan.MiddleAreaGateEvent -= InteractionCause;
        //                break;
        //        }
        //    }
        //    */
        //}

        /*
        private void Update()
        {
            // TODO: change so that isRotating is set to false after rotation is done.
            //if (isRotating)
            //{
            //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed);
            //}
        }
        */

        protected override void InteractionCause()
        {
            if (toggleRotationDirAfter)
            {
                if (isFlipped)
                {
                    targetRot = orignalRot;
                    isFlipped = false;
                }
                else
                {
                    targetRot = orignalRot * addedRot;
                    isFlipped = true;
                }
            }
            else
            {
                // Only to check if this object has rotated once yet
                // If not checked then we would rotate twice the rotateAmount on the first spin.
                if (isRotating)
                {
                    targetRot *= addedRot;
                }
            }

            isRotating = true;
            currentLerpPos = 0f;
            StartCoroutine(Rotate());
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

        private IEnumerator Rotate()
        {
            while (currentLerpPos < 1f - acceptedOffMargin)
            {
                currentLerpPos += Time.deltaTime * rotSpeed;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, currentLerpPos);
                /*
                if (currentLerpPos >= 1f - acceptedOffMargin)
                {
                    isRotating = false;
                }
                */

                yield return null;
            }
        }
    }
}