using CursedWoods.SaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public class PlayerManager : MonoBehaviour, ISaveable
    {
        public bool IsAttackUnlocked { get; set; }
        public bool IsSpellCastUnlocked { get; set; }

        public Dictionary<Spells, bool> spellUnlockInfo = new Dictionary<Spells, bool>();

        public Dictionary<KeyType, bool> keyCollectInfo = new Dictionary<KeyType, bool>();

        // LevelUI activates icons by subscribing to this.
        public static event Action<Spells> SpellUnlocked;

        // TODO: add other progress info and load from here e.g. player health.

        public void Initialize(ISave saveSystem, string keyPrefix)
        {
            //IsAttackUnlocked = false;
            //IsSpellCastUnlocked = false;
            Load(saveSystem, keyPrefix);

            /*
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
            */
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

        public void Save(ISave saveSystem, string keyPrefix)
        {
            saveSystem.SetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.PLAYER_MELEE_UNLOCK_KEY), IsAttackUnlocked);
            saveSystem.SetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.PLAYER_SPELLCAST_UNLOCK_KEY), IsSpellCastUnlocked);

            // TODO: extract method for these
            for (int i = 0; i < spellUnlockInfo.Count; i++)
            {
                bool hasSpell = spellUnlockInfo[(Spells)i];
                saveSystem.SetBool(SaveUtils.GetKey(keyPrefix, string.Format(SaveUtils.PLAYER_SPELL_UNLOCK_KEY, i)), hasSpell);
            }

            for (int i = 0; i < keyCollectInfo.Count; i++)
            {
                bool hasCollectedKey = keyCollectInfo[(KeyType)i];
                saveSystem.SetBool(SaveUtils.GetKey(keyPrefix, string.Format(SaveUtils.PLAYER_KEY_COLLECT_KEY, i)), hasCollectedKey);
            }
        }

        public void Load(ISave saveSystem, string keyPrefix)
        {
            IsAttackUnlocked = saveSystem.GetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.PLAYER_MELEE_UNLOCK_KEY), false);
            IsSpellCastUnlocked = saveSystem.GetBool(SaveUtils.GetKey(keyPrefix, SaveUtils.PLAYER_SPELLCAST_UNLOCK_KEY), false);

            spellUnlockInfo.Clear();

            for (int i = 0; i < Enum.GetNames(typeof(Spells)).Length; i++)
            {
                bool hasSpell = saveSystem.GetBool(SaveUtils.GetKey(keyPrefix, string.Format(SaveUtils.PLAYER_SPELL_UNLOCK_KEY, i)), false);
                spellUnlockInfo.Add((Spells)i, hasSpell);
            }

            keyCollectInfo.Clear();

            for (int i = 0; i < Enum.GetNames(typeof(KeyType)).Length; i++)
            {
                bool hasKey = saveSystem.GetBool(SaveUtils.GetKey(keyPrefix, string.Format(SaveUtils.PLAYER_KEY_COLLECT_KEY, i)), false);
                keyCollectInfo.Add((KeyType)i, hasKey);
            }
        }
    }
}