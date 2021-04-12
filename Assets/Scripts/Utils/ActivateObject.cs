using UnityEngine;

namespace CursedWoods.Utils
{
    public class ActivateObject : MonoBehaviour
    {
        [SerializeField]
        private GameObject objectToActivate;
        [SerializeField]
        private bool useCallersPosition = true;

        // Called from some animations events
        private void ActivateObj()
        {
            if (objectToActivate != null)
            {
                if (useCallersPosition)
                {
                    objectToActivate.transform.position = transform.position;
                }

                objectToActivate.transform.parent = null;
                objectToActivate.SetActive(true);
            }
        }
    }
}