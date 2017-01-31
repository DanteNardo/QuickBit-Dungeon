using System.Collections.Generic;
using QuickBit_Dungeon.Characters;
using QuickBit_Dungeon.Managers;

namespace QuickBit_Dungeon.DungeonGeneration
{
	public class Cell
	{
		// ======================================
		// ============== Members ===============
		// ======================================

		public Entity Local { get; set; } = null;
		public Door Door { get; set; } = null;
	    public float Alpha { get; set; } = 1;
		public int Weight { get; set; } = 0;

		private int Size { get; set; }
		public char Type { get; set; }
		public char Rep { get; set; }

		public int X { get; private set; }
		public int Y { get; private set; }
		public List<int[]> Neighbors { get; private set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		#region Cell Methods

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="size">The size of the grid</param>
		/// <param name="y">The y coordinate of the cell</param>
		/// <param name="x">The x coordinate of the cell</param>
		public void Construct(int size, int y, int x)
		{
			// Initialize variables
			Type = ' ';
			Rep = Type;
			Size = size;
			X = x;
			Y = y;
			Neighbors = new List<int[]>();
		}

		/// <summary>
		/// Generates a list of int arrays
		/// The arrays correspond to x,y cooridnates
		/// </summary>
		public void GenerateNeighbors()
		{
			if (X + 1 < Size)
				Neighbors.Add(new int[2] {Y, X + 1});
			if (X - 1 >= 0)
				Neighbors.Add(new int[2] {Y, X - 1});
			if (Y + 1 < Size)
				Neighbors.Add(new int[2] {Y + 1, X});
			if (Y - 1 >= 0)
				Neighbors.Add(new int[2] {Y - 1, X});
		}

		/// <summary>
		/// Wipes the local and it's representation
		/// from the current cell.
		/// </summary>
		public void ClearLocal()
		{
			Rep = Type;
			Local = null;
		}

		/// <summary>
		/// Adds the new local's data to the cell
		/// and changes the cell representation.
		/// </summary>
		/// <param name="e">The new local entity</param>
		public void NewLocal(Entity e)
		{
			Rep = GameManager.ConvertToChar(e.HealthRep);
			Local = e;
		}

		#endregion
	}
}