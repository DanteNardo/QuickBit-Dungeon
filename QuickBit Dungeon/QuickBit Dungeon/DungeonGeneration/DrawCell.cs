using Microsoft.Xna.Framework;

namespace QuickBit_Dungeon.DungeonGeneration
{
	internal class DrawCell
	{
		// ======================================
		// ============== Members ===============
		// ======================================

		public char GameObject { get; set; }
		public Vector2 Position { get; set; }
        public Color CurrentShade { get; set; }
		public Color Shade { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		#region DrawCell Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public DrawCell()
		{
			GameObject = ' ';
			Position = Vector2.Zero;
			Shade = Color.White;
		}

		#endregion
	}
}
