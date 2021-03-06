namespace CursedWoods
{
	public interface IHealth
	{
		/// <summary>
		/// Event laukeaa aina, kun hahmon healthin määrä muuttuu.
		/// </summary>
		event System.Action<int> HealthChanged;

		/// <summary>
		/// If character takes massive damage, this causes them to knockback/staggger.
		/// </summary>
		event System.Action Staggered;

		/// <summary>
		/// Palauttaa hahmon tämänhetkisen healthin määrän.
		/// </summary>
		int CurrentHealth { get; }

		/// <summary>
		/// Healthin maksimimäärä.
		/// CurrentHealth ei koskaan ylitä tätä määrää.
		/// </summary>
		int MaxHealth { get; }

		/// <summary>
		/// Healthin minimimäärä.
		/// CurrentHealth ei koskaan alita tätä määrää.
		/// </summary>
		int MinHealth { get; }

		/// <summary>
		/// If health is decreased more than this value then invoke Staggered event.
		/// </summary>
		int MinCauseStagger { get; set; }

		/// <summary>
		/// Kertoo, onko hahmo tällä hetkellä kuolematon vai ei. Jos pelaaja
		/// (tai vihollinen) on kuolematon, healthia ei saa vähentää,
		/// vaikka pelaaja saisikin osuman.
		/// </summary>
		bool IsImmortal { get; set; }

		/// <summary>
		/// Kasvattaa healthin määrää parametrinä annetun amount:n verran.
		/// Ei kuitenkaan koskaan ylitä MaxHealth:a
		/// </summary>
		/// <param name="amount">Määrä, jolla CurrentHealth:in määrää kasvatetaan</param>
		void IncreaseHealth(int amount);

		/// <summary>
		/// Vähentää healthin määrää parametrinä annetun amount:n verran.
		/// Ei kuitenkaan koskaan alita MinHealth:a
		/// </summary>
		/// <param name="amount">Määrä, jolla CurrentHealth:in määrää vähennetään</param>
		/// <param name="damageType">The type of damage we took.</param>
		void DecreaseHealth(int amount, DamageType damageType);

		/// <summary>
		/// Increases the maximum health with the given amount.
		/// </summary>
		/// <param name="amount">The amount maximum health is increased.</param>
		void IncreaseMaxHealth(int amount);

		/// <summary>
		/// Decreases the maximum health with the given amount.
		/// If CurrentHealth is larger than the new decreased MaxHealth then it is also decreased to the new MaxHealth amount.
		/// </summary>
		/// <param name="amount">The amount maximum health is decreased.</param>
		void DecreaseMaxHealth(int amount);

		/// <summary>
		/// Alustaa komponentin arvot alkutilaan
		/// </summary>
		void ResetValues();
	}
}