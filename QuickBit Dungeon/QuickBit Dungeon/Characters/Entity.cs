using Microsoft.Xna.Framework;
using QuickBit_Dungeon.Interaction;
using QuickBit_Dungeon.Managers;

namespace QuickBit_Dungeon.Characters
{
	/// <summary>
    /// Every object that attacks or moves
    /// inherits from the entity class.
    /// </summary>
	public class Entity
	{
		// ======================================
		// ============== Members ===============
		// ======================================
		
		public int X { get; set; }
		public int Y { get; set; }

		public int Level { get; private set; }
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
		public int CritChance { get; internal set; }
		public int Xp { get; set; }

		public Color EntityColor { get; set; } = Color.White;
		private Color PlayerColor { get; set; } = Color.Blue;
		private Color MonsterColor { get; set; } = Color.Green;
		private Color PlayerAttackedColor { get; set; } = Color.DarkRed;
		private Color MonsterAttackedColor { get; set; } = Color.Red;
		private Timer ColorTimer { get; set; }

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
			Level = 1;
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
			ColorTimer = new Timer((int)(.15*60));
			ResetColor();
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
			ColorTimer = new Timer((int)(.15*60));
			ResetColor();
		}

		/// <summary>
		/// Updates the entire entity.
		/// </summary>
		public void Update()
		{
			ColorTimer.Update();
			if (ColorTimer.ActionReady)
				ResetColor();
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
			CalculateHealthRep();
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
		/// Levels up an entity based on
		///	their desired stat change.
		/// </summary>
		/// <param name="color">The color the player picked to level up</param>
		public void LevelUp(string color)
		{
			switch (color)
			{
				case "red":
					Strength += GameManager.Random.Next(1, 4);
					MaxHealth += Strength*2;
					Health = MaxHealth;
					CalculateHealthRep();
					Level++;
					break;
				case "green":
					Dexterity += GameManager.Random.Next(1, 4);
					CritChance += GameManager.Random.Next(1, Dexterity)/4;
					Armor += GameManager.Random.Next(1, Dexterity)/4;
					MaxHealth += Strength;
					Health = MaxHealth;
					CalculateHealthRep();
					Level++;
					break;
				case "blue":
					Wisdom += GameManager.Random.Next(1, 4);
					MaxMana += Wisdom;
					HealthMana = MaxMana;
					AttackMana = MaxMana;
					MaxHealth += Strength;
					Health = MaxHealth;
					CalculateHealthRep();
					Level++;
					break;
			}
		}

		/// <summary>
		/// Resets the entity color to the default color
		/// for this entity.
		/// </summary>
		public void ResetColor()
		{
			if (this is Player)
				EntityColor = PlayerColor;
			else if (this is Monster)
				EntityColor = MonsterColor;
		}

		/// <summary>
		/// Changes the color of the entity to the
		/// correct entity attacked coloring.
		/// </summary>
		public void AttackColor()
		{
			if (this is Player)
				EntityColor = PlayerAttackedColor;
			else if (this is Monster)
				EntityColor = MonsterAttackedColor;
			ColorTimer.PerformAction();
		}

		#endregion

	}
}
