using System;
using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public class PlayerManager : MonoBehaviour
    {
        public bool IsAttackUnlocked { get; set; }
        public bool IsSpellCastUnlocked { get; set; }

        public Dictionary<Spells, bool> spellUnlockInfo = new Dictionary<Spells, bool>();

        public Dictionary<KeyType, bool> keyCollectInfo = new Dictionary<KeyType, bool>();

        // LevelUI activates icons by subscribing to this.
        public static event Action<Spells> SpellUnlocked;

        // TODO: add other progress info and load from here e.g. player health.

        public void Initialize()
        {
            IsAttackUnlocked = false;
            IsSpellCastUnlocked = false;

            spellUnlockInfo.Clear();

            for (int i = 0; i < Enum.GetNames(typeof(Spells)).Length; i++)
            {
                spellUnlockInfo.Add((Spells)i, false);
            }

            keyCollectInfo.Clear();

            for (int i = 0; i < Enum.GetNames(typeof(KeyType)).Length; i++)
            {
                keyCollectInfo.Add((KeyType)i, false);
            }
        }

        public void UnlockSpellByType(Spells spell)
        {
            spellUnlockInfo[spell] = true;
            if (SpellUnlocked != null)
            {
                SpellUnlocked(spell);
            }
        }

        public bool GetSpellLockStatus(Spells spell)
        {
            return spellUnlockInfo[spell];
        }

        public void CollectedKey(KeyType key)
        {
            keyCollectInfo[key] = true;
        }

        public bool GetKeyCollectedStatus(KeyType key)
        {
            return keyCollectInfo[key];
        }
    }
}