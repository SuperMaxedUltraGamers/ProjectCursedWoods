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
                Settings.Instance.Audio.ChangeMusic(Data.AudioContainer.Music.Combat);
                AwakeTreeBossEvent();
                gameObject.SetActive(false);
            }
        }
    }
}