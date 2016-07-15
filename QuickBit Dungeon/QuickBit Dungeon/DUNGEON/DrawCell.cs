using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace QuickBit_Dungeon.DUNGEON
{
	internal class DrawCell
	{
		// ======================================
		// ============== Members ===============
		// ======================================

		public char GameObject { get; set; }
		public Vector2 Position { get; set; }
		public Color Shade { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Constructor
		/// </summary>
		public DrawCell()
		{
			GameObject = ' ';
			Position = Vector2.Zero;
			Shade = Color.White;
		}
	}
}
