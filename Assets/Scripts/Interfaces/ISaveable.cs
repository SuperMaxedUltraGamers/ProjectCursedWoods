namespace CursedWoods.SaveSystem
{
	public interface ISaveable
	{
		void Save(ISave saveSystem, string keyPrefix);
		void Load(ISave saveSystem, string keyPrefix);
	}
}