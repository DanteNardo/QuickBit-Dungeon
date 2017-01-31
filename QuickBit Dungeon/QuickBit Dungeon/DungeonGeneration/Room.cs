using System.Collections.Generic;

namespace QuickBit_Dungeon.DungeonGeneration
{
	public class Room
	{
		// ======================================
		// ============== Members ===============
		// ======================================
	
		public int Width { get; set; }
		public int Height { get; set; }

		public List<int[]> Edges { get; private set; }
		public int[] Position { get; private set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		#region Room Methods

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="width">Width of the room</param>
		/// <param name="height">Height of the room</param>
		/// <param name="position">Top left corner of the room's position</param>
		public void Construct(int width, int height, int[] position)
		{
			// Initialize variables
			Width = width;
			Height = height;
			Position = position;
			Edges = new List<int[]>();
		}

		/// <summary>
		/// Generates a list of int arrays
		/// The arrays correspond to x,y cooridnates
		/// </summary>
		public void GenerateEdges()
		{
			// Top and Bottom row
			for (var i = 0; i < Width; i++)
			{
				Edges.Add(new int[2] {Position[0], Position[1] + i});
				Edges.Add(new int[2] {Position[0] + Height - 1, Position[1] + i});
			}

			// Left and Right side
			for (var i = 0; i < Width; i++)
			{
				Edges.Add(new int[2] {Position[0], Position[1] + i});
				Edges.Add(new int[2] {Position[0] + Height - 1, Position[1] + i});
			}
		}

		#endregion
	}
}