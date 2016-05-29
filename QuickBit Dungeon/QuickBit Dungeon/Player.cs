using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickBit_Dungeon
{
	class Player : Entity
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		
		private int x;				// The x coordinate of the player's position
		private int y;				// The y coordinate of the player's position
		private int xpNeeded;		// The amount of xp necessary to level up
		
		// Properties
		public int X { get { return x; } set { x = value; } }
		public int Y { get { return y; } set { y = value; } }

		// ======================================
		// ============== Methods ===============
		// ======================================

		// Constructor
		public Player() : base()
		{
			ConstructPlayer();
			xpNeeded = 100;
		}
	}
}
