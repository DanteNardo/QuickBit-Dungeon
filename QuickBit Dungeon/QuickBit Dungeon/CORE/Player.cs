using System;
using System.Collections.Generic;
using QuickBit_Dungeon.DUNGEON;
using QuickBit_Dungeon.MANAGERS;

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
		/// Generates a weight map for the dungeon.
		/// Used for the Monster's AI.
		/// </summary>
		public void GenerateWeightMap()
		{
			var weight = 1;
			var steps = 0;
			var totalSteps = 10;

			var cells = new List<int[]>();
			var nextCells = new List<int[]>();
			var cc = Dungeon.Grid[Y][X];
			cells.Add(new[] {Y, X});
			
			// Keep stepping
			while (steps < totalSteps)
			{
				// Select the current cell
				if (cells.Count > 1)
				{
					var rcell = GameManager.Random.Next(0, cells.Count - 1);
					cc = Dungeon.Grid[cells[rcell][0]][cells[rcell][1]];
				}
				else cc = Dungeon.Grid[cells[0][0]][cells[0][1]];

				// Set weight to valid cells only - add their neighbors
				if (cc.Weight == 0 && cc.Type != ' ')
				{
					if (cc.Door != null && cc.Door.Closed)
						RemoveArray(ref cells, new[] {cc.Y, cc.X});
					else if (cc.Local is Monster)
					{
						int open = 0;
						foreach (var n in cc.Neighbors)
							if (Dungeon.Grid[n[0]][n[1]].Type != ' ')
								open++;
						if (open == 2)
							nextCells.AddRange(cc.Neighbors);

						cc.Weight = weight;
					}
					else
					{
						cc.Weight = weight;
						nextCells.AddRange(cc.Neighbors);
					}
				}

				// Remove current cell
				RemoveArray(ref cells, new[] {cc.Y, cc.X});

				// Go to the next list of cells, update steps and weight
				if (cells.Count == 0)
				{
					if (nextCells.Count == 0)
						return;
					weight++;
					steps++;
					foreach (var cell in nextCells)
						cells.Add(cell);
					nextCells.Clear();
				}
			}
		}

		/// <summary>
		/// Resets all cell weights to 0.
		/// </summary>
		public void ClearWeightMap()
		{
			foreach (var row in Dungeon.Grid)
				foreach (var cell in row)
					cell.Weight = 0;
		}

		/// <summary>
		/// Searches through a list of int arrays
		/// and removes an array if it matches the
		/// input paramater.
		/// </summary>
		/// <param name="list">The list to modify</param>
		/// <param name="array">The array to remove from the list</param>
		private void RemoveArray(ref List<int[]> list, int[] array)
		{
			for (var i = 0; i < list.Count; i++)
				if (list[i][0] == array[0] &&
				    list[i][1] == array[1])
				{
					list.RemoveAt(i);
					return;
				}
		}

		/// <summary>
		/// Regenerates the player's mana based
		///	off of their wisdom.
		/// </summary>
		public void RegenerateMana()
		{
			HealthMana += ((float)Wisdom)/10;
			AttackMana += ((float)Wisdom)/10;
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
		    XpNeeded *= 2;
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
