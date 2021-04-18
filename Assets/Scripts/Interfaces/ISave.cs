namespace CursedWoods.SaveSystem
{
	public interface ISave
	{
		/// <summary>
		/// Indicates weather the game is in loading state
		/// </summary>
		bool IsLoading { get; set; }

		void SetInt(string key, int value);
		void SetFloat(string key, float value);
		void SetBool(string key, bool value);
		void SetString(string key, string value);

		int GetInt(string key, int defaultValue);
		float GetFloat(string key, float defaultValue);
		bool GetBool(string key, bool defaultValue);
		string GetString(string key, string defaultValue);

		bool HasKey(string key);

		/// <summary>
		/// Saves data stored in the ISave to the disk. All the values have to be set first.
		/// </summary>
		/// <param name="saveSlot">Can be used as a save file name for example</param>
		void Save(string saveSlot);

		/// <summary>
		/// Loads the saved data from the disk. Has to be called before anything can be fetched using Get methods.
		/// </summary>
		/// <param name="saveSlot">Can be used as a save file name for example</param>
		void Load(string saveSlot);

		bool DeleteSaveFile(string saveSlot);

		bool SaveFileExist(string saveSlot);

		// TODO: Support for save slots
	}
}