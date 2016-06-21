namespace QuickBit_Dungeon.CORE
{
	/*
		Every object that attacks or moves
		inherits from the entity class.
	*/
	public class Entity
	{
		// ======================================
		// ============== Members ===============
		// ======================================
		
		public int X { get; set; }
		public int Y { get; set; }

		public int MaxHealth { get; private set; }
		public int Health { get; set; }
		public int HealthRep { get; private set; }
		public int Armor { get; set; }
		public int Strength { get; set; }
		public int Dexterity { get; set; }
		public int Wisdom { get; set; }
		public float MaxMana { get; set; }
		public float HealthMana { get; set; }
		public float AttackMana { get; set; }
		public int Xp { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		#region Methods

		/// <summary>
		/// Sets the player's default hard coded
		///	starting values for all stats.
		/// </summary>
		public void ConstructPlayer()
		{
			MaxHealth = 100;
			Health = 100;
			HealthRep = 9;
			Armor = 5;
			Strength = 10;
			Dexterity = 10;
			Wisdom = 10;
			MaxMana = 100f;
			HealthMana = MaxMana;
			AttackMana = MaxMana;
		}

		/// <summary>
		/// Creates a random enemy with random
		///	stats (that still have bounds).
		/// </summary>
		public void ConstructMonster()
		{
			MaxHealth = GameManager.Random.Next(20, 100);
			Health = MaxHealth;
			Armor = GameManager.Random.Next(1, 9);
			Strength = GameManager.Random.Next(7, 14);
			Dexterity = GameManager.Random.Next(7, 14);
			Wisdom = GameManager.Random.Next(7, 14);
			CalculateHealthRep();
			GenerateXp();
		}

		/// <summary>
		/// Returns the correct representation
		///	of the entitie's current health.
		/// </summary>
		public void CalculateHealthRep()
		{
			float ch = Health;
			float mh = MaxHealth;
			var hr = ch/mh*10;
			if (hr == 10) hr = 9;
			HealthRep = (int) hr;
		}

		/// <summary>
		/// Updates the Health correctly.
		/// </summary>
		/// <param name="modifier">Amount to modify health by</param>
		public void UpdateHealth(int modifier)
		{
			Health += modifier;
			if (Health > MaxHealth)
				Health = MaxHealth;
			if (Health < 0)
				Health = 0;
		}

		/// <summary>
		/// Determines the amount of xp that
		///	the player should receive from 
		///	defeating this entity. 
		/// </summary>
		public void GenerateXp()
		{
			Xp += MaxHealth;
			Xp += Armor;
			Xp += Strength;
			Xp += Dexterity;
			Xp += Wisdom;
		}

		/// <summary>
		/// Levels up the character based on
		///	their desired stat change.
		/// </summary>
		/// <param name="color">The color the player picked to level up</param>
		public void LevelUp(string color)
		{
			switch (color)
			{
				case "red":
					Strength += GameManager.Random.Next(1, 4);
					MaxHealth += Strength;
					Health = MaxHealth;
					break;
				case "green":
					Dexterity += GameManager.Random.Next(1, 4);
					MaxHealth += Strength;
					Health = MaxHealth;
					break;
				case "blue":
					Wisdom += GameManager.Random.Next(1, 4);
					MaxHealth += Strength;
					Health = MaxHealth;
					break;
			}
		}

		#endregion

	}
}
