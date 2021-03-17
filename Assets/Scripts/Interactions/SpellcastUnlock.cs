using System;
using UnityEngine;

namespace CursedWoods
{
    public class SpellcastUnlock : InteractionBase
    {
        public event Action SpellcastUnlocked;
        public override float Interaction()
        {
            base.Interaction();
            if (SpellcastUnlocked != null)
            {
                SpellcastUnlocked?.Invoke();
            }

            Destroy(gameObject, 0.2f);
            return animTime;
        }
    }
}