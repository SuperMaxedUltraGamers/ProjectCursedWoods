namespace CursedWoods.SaveSystem
{
	public static class SaveUtils
	{
		// Saveslots
		public const string AUTOSAVE_SAVE_SLOT = "AutoSave";

		// Save Prefixes
		public const string GAME_MAN_PREFIX = "GameMan";
		public const string GRAVEYARD_MAN_PREFIX = "GraveyardMan";
		public const string CASTLE_MAN_PREFIX = "CastleMan";
		public const string PLAYER_TRANS_PREFIX = "PlayerTrans";
		public const string CHAR_CONTROLLER_PREFIX = "CharController";
		public const string PLAYER_MAN_PREFIX = "PlayerMan";
		public const string AI_MAN_PREFIX = "AIManager";

		// Save keys

		// GameMan
		public const string GAMEMAN_SCENE_NAME_KEY = "Scene";

		// GraveyardMan
		public const string GRAVEYARD_SPAWN_SKELETON_BOSS_1_KEY = "SpawnSkeletonBoss1";
		public const string GRAVEYARD_SPAWN_MAX_HEALTH_INCREASE_KEY = "SpawnMaxHealthIncrease{0}";
		public const string GRAVEYARD_GATE_OPEN_KEY = "GateOpen{0}";

		// CastleMan
		public const string CASTLE_USE_LEVEL_START_POS_KEY = "UseLevelStartPos";
		public const string CASTLE_FINAL_BOSS_DOOR_OPEN_KEY = "FinalBossDoorOpen";

		// PlayerTrans
		public const string PLAYER_X_KEY = "X";
		public const string PLAYER_Y_KEY = "Y";
		public const string PLAYER_Z_KEY = "Z";
		public const string PLAYER_ROT_KEY = "Rotation";

		// CharController
		public const string PLAYER_MAX_HEALTH_KEY = "MaxHealth";
		public const string PLAYER_CURRENT_HEALTH_KEY = "CurrentHealth";

		// PlayerMan
		public const string PLAYER_MELEE_UNLOCK_KEY = "MeleeUnlock";
		public const string PLAYER_SPELLCAST_UNLOCK_KEY = "SpellCastUnlock";
		public const string PLAYER_SPELL_UNLOCK_KEY = "SpellUnlock_{0}";
		public const string PLAYER_KEY_COLLECT_KEY = "KeyCollect{0}";

		// AIManager
		public const string AI_MAN_ENEMIES_KILLED_AMOUNT_KEY = "EnemiesKilledAmount";

		public static string GetKey(string prefix, string variableKey)
		{
			return $"{prefix}_{variableKey}";
		}
	}
}