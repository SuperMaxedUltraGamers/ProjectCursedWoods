using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public class Flinch : MonoBehaviour
    {
        [SerializeField]
        private Transform flinchBone;
        [SerializeField]
        private float flinchAmount;
        private float currentFlinch;
        private float flinchSpeed = 100f;
        private bool isFlinching;
        private FlinchDir flinchDir = FlinchDir.Back;

        private enum FlinchDir
        {
            Back = 0,
            Forth
        }

        private void LateUpdate()
        {
            if (isFlinching)
            {
                DoFlinch();
            }
        }

        public void StartFlinch()
        {
            if (!isFlinching)
            {
                isFlinching = true;
                currentFlinch = 0f;
                flinchDir = FlinchDir.Back;
            }
        }

        private void DoFlinch()
        {
            switch (flinchDir)
            {
                case FlinchDir.Back:
                    currentFlinch += flinchSpeed * Time.deltaTime;
                    if (currentFlinch >= flinchAmount)
                    {
                        currentFlinch = flinchAmount;
                        flinchDir = FlinchDir.Forth;
                    }

                    break;
                case FlinchDir.Forth:
                    currentFlinch -= flinchSpeed * Time.deltaTime;
                    if (currentFlinch <= 0f)
                    {
                        currentFlinch = 0f;
                        isFlinching = false;
                    }

                    break;
            }

            Vector3 rot = flinchBone.rotation.eulerAngles;
            rot.x -= currentFlinch;
            rot.y -= currentFlinch;
            flinchBone.eulerAngles = rot;
        }
    }
}