using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CursedWoods.SaveSystem
{
	public class BinarySaver : ISave
	{
		private SaveData<int> intValues = new SaveData<int>();
		private SaveData<float> floatValues = new SaveData<float>();
		private SaveData<bool> boolValues = new SaveData<bool>();
		private SaveData<string> stringValues = new SaveData<string>();

		public bool IsLoading { get; set; }
		public string SaveFolder { get; }
		public string FileExtension { get; }

		public BinarySaver(string saveFolder)
		{
			SaveFolder = saveFolder;
			FileExtension = ".data";
		}

		public void SetInt(string key, int value)
		{
			intValues[key] = value;
		}

		public void SetFloat(string key, float value)
		{
			floatValues[key] = value;
		}

		public void SetBool(string key, bool value)
		{
			boolValues[key] = value;
		}

		public void SetString(string key, string value)
		{
			stringValues[key] = value;
		}

		public int GetInt(string key, int defaultValue)
		{
			return GetValue(intValues, key, defaultValue);
		}

		public float GetFloat(string key, float defaultValue)
		{
			return GetValue(floatValues, key, defaultValue);
		}

		public bool GetBool(string key, bool defaultValue)
		{
			return GetValue(boolValues, key, defaultValue);
		}

		public string GetString(string key, string defaultValue)
		{
			return GetValue(stringValues, key, defaultValue);
		}

		private TData GetValue<TData>(SaveData<TData> data, string key, TData defaultValue)
		{
			TData result = defaultValue;

			if (data.ContainsKey(key))
			{
				result = data[key];
			}

			return result;
		}

		public bool HasKey(string key)
		{
			return intValues.ContainsKey(key)
					 || floatValues.ContainsKey(key)
					 || boolValues.ContainsKey(key)
					 || stringValues.ContainsKey(key);
		}

		public void Save(string saveSlot)
		{
			try
			{
				// Create a save folder if it does not exist already
				if (!Directory.Exists(SaveFolder))
				{
					Directory.CreateDirectory(SaveFolder);
				}
			}
			catch (IOException e)
			{
				// The directory specified by path is a file or The network name is not known
				Debug.LogException(e);
			}
			catch (UnauthorizedAccessException e)
			{
				// No rights to create folder to path SaveFolder
				Debug.LogException(e);
			}
			catch (ArgumentException e)
			{
				// Illegal characters in the path
				Debug.LogException(e);
			}
			catch (Exception e)
			{
				// Catches all types of exceptions which are not handled already
				Debug.LogException(e);
			}

			string saveFilePath = GetSavePath(saveSlot);

			using (FileStream file = File.Open(saveFilePath, FileMode.Create))
			{
				using (BinaryWriter writer = new BinaryWriter(file))
				{
					// Write Data-dictionaries one-by-one.

					// IntValues:
					// First, we have to write how many items a dictionary has.
					writer.Write(intValues.Count);

					// Then write key-value pairs to the file
					foreach (KeyValuePair<string, int> intData in intValues)
					{
						// Write the key first
						writer.Write(intData.Key);
						// Then the value
						writer.Write(intData.Value);
					}

					// FloatValues:
					writer.Write(floatValues.Count);
					foreach (KeyValuePair<string, float> floatData in floatValues)
					{
						// Write the key first
						writer.Write(floatData.Key);
						// Then the value
						writer.Write(floatData.Value);
					}

					// BoolValues:
					writer.Write(boolValues.Count);
					foreach (KeyValuePair<string, bool> boolData in boolValues)
					{
						// Write the key first
						writer.Write(boolData.Key);
						// Then the value
						writer.Write(boolData.Value);
					}

					// FloatValues:
					writer.Write(stringValues.Count);
					foreach (KeyValuePair<string, string> stringData in stringValues)
					{
						// Write the key first
						writer.Write(stringData.Key);
						// Then the value
						writer.Write(stringData.Value);
					}
				}
			}
		}

		public void Load(string saveSlot)
		{
			string filePath = GetSavePath(saveSlot);
			if (!File.Exists(filePath))
			{
				Debug.Log($"Save file {saveSlot} does not exist!");
				return;
			}

			Reset();

			using (FileStream file = File.OpenRead(filePath))
			{
				using (BinaryReader reader = new BinaryReader(file))
				{
					// Values must be read from the file in the same order they were written.

					// IntValues:
					int count = reader.ReadInt32();
					for (int i = 0; i < count; i++)
					{
						string key = reader.ReadString();
						int value = reader.ReadInt32();

						SetInt(key, value);
					}

					// floatValues:
					count = reader.ReadInt32();
					for (int i = 0; i < count; i++)
					{
						string key = reader.ReadString();
						float value = reader.ReadSingle();

						SetFloat(key, value);
					}

					// boolValues:
					count = reader.ReadInt32();
					for (int i = 0; i < count; i++)
					{
						string key = reader.ReadString();
						bool value = reader.ReadBoolean();

						SetBool(key, value);
					}

					// stringValues:
					count = reader.ReadInt32();
					for (int i = 0; i < count; i++)
					{
						string key = reader.ReadString();
						string value = reader.ReadString();

						SetString(key, value);
					}
				}
			}
		}

		public bool DeleteSaveFile(string saveSlot)
		{
			string filePath = GetSavePath(saveSlot);
			if (File.Exists(filePath))
            {
				File.Delete(filePath);
				Debug.Log($"Save file {saveSlot} deleted!");
				return true;
            }
			else
            {
				Debug.Log($"Save file {saveSlot} does not exist!");
				return false;
            }
		}

		public bool SaveFileExist(string saveSlot)
		{
			string filePath = GetSavePath(saveSlot);
			return File.Exists(filePath);
		}

		private void Reset()
		{
			intValues.Clear();
			floatValues.Clear();
			boolValues.Clear();
			stringValues.Clear();
		}

		private string GetSavePath(string fileName)
		{
			return Path.Combine(SaveFolder, fileName) + FileExtension;
		}
	}
}