using System;
using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public class PlayerManager : MonoBehaviour
    {
        public bool IsAttackUnlocked { get; set; }
        public bool IsSpellCastUnlocked { get; set; }

        public Dictionary<int, bool> spellUnlockInfo = new Dictionary<int, bool>();

        // TODO: add other progress info and load from here e.g. player health.

        public void Initialize()
        {
            IsAttackUnlocked = false;
            IsSpellCastUnlocked = false;

            spellUnlockInfo.Clear();

            for (int i=0; i< Enum.GetNames(typeof(Spells)).Length; i++)
            {
                spellUnlockInfo.Add(i, false);
            }
        }

        public void UnlockSpellByType(Spells spell)
        {
            spellUnlockInfo[(int)spell] = true;
        }

        public bool GetSpellLockStatus(Spells spell)
        {
            return spellUnlockInfo[(int)spell];
        }
    }
}