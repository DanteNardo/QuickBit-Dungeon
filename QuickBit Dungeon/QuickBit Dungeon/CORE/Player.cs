using QuickBit_Dungeon.DUNGEON;

namespace QuickBit_Dungeon.CORE
{
	public class Player : Entity
	{
		// ======================================
		// ============== Members ===============
		// ======================================	
		
		public int XpNeeded { get; }
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
				Health += Wisdom;
				HealthMana -= ManaCost;
				if (Health > MaxHealth) Health = MaxHealth;
				CalculateHealthRep();
				Dungeon.Grid[X][Y].Rep = GameManager.ConvertToChar(HealthRep);
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

		#endregion
	}
}
