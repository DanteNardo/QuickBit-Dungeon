using System.Collections.Generic;
using QuickBit_Dungeon.DungeonGeneration;
using QuickBit_Dungeon.Interaction;
using QuickBit_Dungeon.Managers;

namespace QuickBit_Dungeon.Characters
{
	public class Monster : Entity
	{
		// ======================================
		// ============== Members ===============
		// ======================================

		public Timer AttackTimer { get; set; }
		public Timer MoveTimer { get; set; }

		private class Node
		{
			public int Y { get; set; }
			public int X { get; set; }
			public int Weight { get; set; }
		}

		private int Range { get; set; } = 10;
		private Node[,] Graph { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		#region Monster Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public Monster() : base()
		{
			AttackTimer = new Timer(60);
			MoveTimer = new Timer(30);
			Graph = new Node[2*Range + 1, 2*Range + 1];
		}

		/// <summary>
		/// Updates a Monster
		/// </summary>
		public new void Update()
		{
			base.Update();
			AttackTimer.Update();
			MoveTimer.Update();

		    if (MoveTimer.ActionReady)
		    {
		        if (MoveToLowestWeight()) return;
		        if (GameManager.Random.Next(0, 100) == 0)
		        {
		            RegenHealth();
		            MoveRandomOne();
		        }
		    }
		}

		/// <summary>
		/// Passively regenerates health based
		/// off of the Monster's Wisdom.
		/// </summary>
		private void RegenHealth()
		{
		    Health += Wisdom/10;
		    if (Health > MaxHealth)
		        Health = MaxHealth;
		}

		/// <summary>
		/// Moves the monster one unit in a
		/// random direction.
		/// </summary>
		private void MoveRandomOne()
		{
			var result = GameManager.Random.Next(0, 4);
			switch (result)
			{
				case 0:
					if (Dungeon.CanMove(this, 1, 0) &&
						Dungeon.NotAClosedDoor(Y+1, X))
						Dungeon.MoveEntity(this, 1, 0);
					MoveTimer.PerformAction();
					break;
				case 1:
					if (Dungeon.CanMove(this, -1, 0) &&
						Dungeon.NotAClosedDoor(Y-1, X))
						Dungeon.MoveEntity(this, -1, 0);
					MoveTimer.PerformAction();
					break;
				case 2:
					if (Dungeon.CanMove(this, 0, 1) &&
						Dungeon.NotAClosedDoor(Y, X+1))
						Dungeon.MoveEntity(this, 0, 1);
					MoveTimer.PerformAction();
					break;
				case 3:
					if (Dungeon.CanMove(this, 0, -1) &&
						Dungeon.NotAClosedDoor(Y, X-1))
						Dungeon.MoveEntity(this, 0, -1);
					MoveTimer.PerformAction();
					break;
			}
		}

		/// <summary>
		/// Moves the current monster to the
		/// lowest neighboring weight.
		/// </summary>
		private bool MoveToLowestWeight()
		{
            // Find lowest
			var lowest = 10000;
			List<int[]> lowests = new List<int[]>();
            var foundWeight = false;
			foreach (var n in Dungeon.Grid[Y][X].Neighbors)
			{
				// Touching player aka don't move
				if (Dungeon.Grid[n[0]][n[1]].Local is Player)
					return true;

				// Equal lowest values
				if (Dungeon.Grid[n[0]][n[1]].Weight != 0 &&
				    Dungeon.Grid[n[0]][n[1]].Weight == lowest)
				{
					if (Dungeon.Grid[n[0]][n[1]].Local != null) continue;
					lowests.Add(n);
				}

				// New lowest values
				if (Dungeon.Grid[n[0]][n[1]].Weight != 0 &&
				    Dungeon.Grid[n[0]][n[1]].Weight < lowest)
				{
					if (Dungeon.Grid[n[0]][n[1]].Local != null) continue;
					lowest = Dungeon.Grid[n[0]][n[1]].Weight;
					lowests.Clear();
					lowests.Add(n);
					foundWeight = true;
				}
			}

			if (!foundWeight)
                return false;

			// Try to move to all equal lowest values
			while (lowests.Count > 0)
			{
				if (Dungeon.CanMove(this, lowests[0][0] - Y, lowests[0][1] - X))
				{
					Dungeon.MoveEntity(this, lowests[0][0] - Y, lowests[0][1] - X);
					MoveTimer.PerformAction();
				}
				else lowests.RemoveAt(0);
			}
			
			return true;
		}

		#endregion
	}
}
