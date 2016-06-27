using System.Collections.Generic;

namespace QuickBit_Dungeon.DUNGEON
{
	public class Cell
	{

		// ======================================
		// ============= Variables ==============
		// ======================================

		private int _size;
		
		public char Type { get; set; }
		public char Rep { get; set; }

		public int X { get; private set; }
		public int Y { get; private set; }
		public List<int[]> Neighbors { get; private set; }


		// ======================================
		// ============== Methods ===============
		// ======================================

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
			_size = size;
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
			if (X + 1 < _size)
				Neighbors.Add(new int[2] {Y, X + 1});
			if (X - 1 >= 0)
				Neighbors.Add(new int[2] {Y, X - 1});
			if (Y + 1 < _size)
				Neighbors.Add(new int[2] {Y + 1, X});
			if (Y - 1 >= 0)
				Neighbors.Add(new int[2] {Y - 1, X});
		}
	}
}