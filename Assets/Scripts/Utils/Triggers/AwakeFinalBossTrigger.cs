using System;
using UnityEngine;

namespace CursedWoods.Utils
{
    public class AwakeFinalBossTrigger : MonoBehaviour
    {
        public static event Action AwakeFinalBossEvent;
        private void OnTriggerEnter(Collider other)
        {
            if (AwakeFinalBossEvent != null)
            {
                Settings.Instance.Audio.PlayMusic(Data.AudioContainer.Music.Combat);
                AwakeFinalBossEvent();
                gameObject.SetActive(false);
            }
        }
    }
}