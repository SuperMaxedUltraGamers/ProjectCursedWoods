using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public class ToggleBarrierOnInteraction : InteractionHandlerArray
    {
        [SerializeField]
        private GameObject[] barrierChildObjs;
        private bool isOn;

        protected override void InteractionCause()
        {
            ToggleObjects();
        }

        private void ToggleObjects()
        {
            foreach (GameObject go in barrierChildObjs)
            {
                go.SetActive(!isOn);
            }

            isOn = !isOn;
        }
    }
}