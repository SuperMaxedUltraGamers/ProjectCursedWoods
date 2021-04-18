using System.Collections;
using UnityEngine;

namespace CursedWoods.SaveSystem
{
    public class AutoSaveTrigger : MonoBehaviour
    {
        [SerializeField]
        private bool disableAfterTrigger = true;
        private bool hasTriggered;
        private void OnTriggerEnter(Collider other)
        {
            if (!hasTriggered)
            {
                StartCoroutine(Checkpoint());
            }
        }

        private IEnumerator Checkpoint()
        {
            yield return null;
            GameMan.Instance.AutoSave();
            if (disableAfterTrigger)
            {
                gameObject.SetActive(false);
            }
        }
    }
}