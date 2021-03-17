using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public class PlayerManager : MonoBehaviour
    {
        public bool IsAttackUnlocked { get; set; }
        public bool IsSpellCastUnlocked { get; set; }

        public Dictionary<int, bool> spellUnlockInfo = new Dictionary<int, bool>();

        public void Initialize()
        {
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