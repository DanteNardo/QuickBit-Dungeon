using System;
using QuickBit_Dungeon.DUNGEON;

namespace QuickBit_Dungeon.CORE
{
	public class Player : Entity
	{
		// ======================================
		// ============== Members ===============
		// ======================================	
		
		public int XpNeeded { get; internal set; }
		public int ManaCost { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public Player()
		{
			ConstructPlayer();
			XpNeeded = 100;
			ManaCost = 50;
			CritChance = 5;
		}

		/// <summary>
		/// Regenerates the player's mana based
		///	off of their wisdom.
		/// </summary>
		public void RegenerateMana()
		{
			HealthMana += Wisdom/10;
			AttackMana += Wisdom/10;
			if (HealthMana > MaxMana) HealthMana = MaxMana;
			if (AttackMana > MaxMana) AttackMana = MaxMana;
		}

		/// <summary>
		/// Regenerates the player's health based
		/// off of their mana.
		/// </summary>
		public void RegenerateHealth()
		{
			if (HealthMana > ManaCost && Health < MaxHealth)
			{
				HealthMana -= ManaCost;
				UpdateHealth(Wisdom);
				CalculateHealthRep();
				Dungeon.ResetRep(this);
			}
		}

		/// <summary>
		/// Determines if the player can perform
		///	a special attack.
		/// </summary>
		/// <returns>Whether or not a special attack can be performed</returns>
		public bool CanSpecial()
		{
			return AttackMana > ManaCost;
		}

		/// <summary>
		/// Determines if the player can perform
		///	self healing magic.
		/// </summary>
		/// <returns>Whether or not healing can be performed</returns>
		public bool CanHeal()
		{
			return HealthMana > ManaCost;
		}

		/// <summary>
		/// Determines if the attack is a crit or not.
		/// </summary>
		/// <returns>Whether the attack is a crit.</returns>
		public bool IsCrit()
		{
			return GameManager.Random.Next(0, 100) <= CritChance;
		}

		/// <summary>
		/// Determines if the player has enough Xp
		/// to level up.
		/// </summary>
		/// <returns>Whether or not the player can level up</returns>
		public bool HasEnoughXp()
		{
			return Xp > XpNeeded;
		}

		/// <summary>
		/// Resets the Xp after a level up and also
		/// increases the time to the next level up.
		/// </summary>
		private void ResetXp()
		{
			Xp = 0;
			XpNeeded = (int)Math.Pow(XpNeeded, 1.2);
		}

		/// <summary>
		/// Levels up the player and resets their Xp
		/// </summary>
		/// <param name="color">The color used for leveling up</param>
		public new void LevelUp(string color)
		{
			base.LevelUp(color);
			ResetXp();
			Dungeon.ResetRep(this);
		}

		#endregion
	}
}
