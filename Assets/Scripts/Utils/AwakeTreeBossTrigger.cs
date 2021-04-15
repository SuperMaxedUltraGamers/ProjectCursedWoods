using System;
using UnityEngine;

namespace CursedWoods.Utils
{
    public class AwakeTreeBossTrigger : MonoBehaviour
    {
        public static event Action AwakeTreeBossEvent;
        private void OnTriggerEnter(Collider other)
        {
            if (AwakeTreeBossEvent != null)
            {
                AwakeTreeBossEvent();
                gameObject.SetActive(false);
            }
        }
    }
}