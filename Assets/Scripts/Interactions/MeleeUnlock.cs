﻿using System;
using UnityEngine;

namespace CursedWoods
{
    public class MeleeUnlock : InteractionBase
    {
        public event Action MeleeUnlocked;
        public override float Interaction()
        {
            base.Interaction();
            if (MeleeUnlocked != null)
            {
                MeleeUnlocked?.Invoke();
            }

            Destroy(gameObject, 0.2f);
            return animTime;
        }
    }
}