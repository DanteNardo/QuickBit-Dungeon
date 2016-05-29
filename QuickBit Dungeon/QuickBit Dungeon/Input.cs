using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace QuickBit_Dungeon
{
	public static class Input
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		private static KeyboardState keyState;

		private static Direction currentDirection;
		public static  Direction CurrentDirection
		{
			get { return currentDirection; }
			set { currentDirection = value; }
		}
		private static Direction lastDirection;
		public static  Direction LastDirection
		{
			get { return lastDirection; }
			set { lastDirection = value; }
		}
		public enum Direction
		{
			NORTH,
			SOUTH,
			EAST,
			WEST,
			NONE
		}

		// ======================================
		// ============== Methods ===============
		// ======================================

		/*
			Updates the keyboard state variable
			every frame.
		*/
		public static void Update()
		{
			keyState = Keyboard.GetState();
		}

		/*
			Determines if correct keys are
			being pressed to count as input.
			Updates direction accordingly.
		*/
		public static void GetInput()
		{
			// North
			if (keyState.IsKeyDown(Keys.W))
				currentDirection = Direction.NORTH;
			// South
			if (keyState.IsKeyDown(Keys.S))
				currentDirection = Direction.SOUTH;
			// East
			if (keyState.IsKeyDown(Keys.D))
				currentDirection = Direction.EAST;
			// West
			if (keyState.IsKeyDown(Keys.A))
				currentDirection = Direction.WEST;
		}
	}
}
