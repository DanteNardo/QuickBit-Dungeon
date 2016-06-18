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
		
		private static KeyboardState preState;
		private static KeyboardState curState;

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

		private static Attack playerAttack;
		public static  Attack PlayerAttack
		{
			get { return playerAttack; }
			set { playerAttack = value; }
		}
		public enum Attack
		{
			PHYSICAL,
			SPECIAL,
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
			preState = curState;
			curState = Keyboard.GetState();
		}

		/*
			Determines if correct keys are
			being pressed to count as input.
			Updates direction accordingly.
		*/
		public static void GetInput()
		{
			// ======================================
			// ============= Movement ===============
			// ======================================

			// North
			if (curState.IsKeyDown(Keys.W))
				currentDirection = Direction.NORTH;
			// South
			if (curState.IsKeyDown(Keys.S))
				currentDirection = Direction.SOUTH;
			// East
			if (curState.IsKeyDown(Keys.D))
				currentDirection = Direction.EAST;
			// West
			if (curState.IsKeyDown(Keys.A))
				currentDirection = Direction.WEST;

			// ======================================
			// ============== Combat ================
			// ======================================

			// Physical
			if (Released(Keys.J))
				playerAttack = Attack.PHYSICAL;
			// Special
			if (Released(Keys.I))
				playerAttack = Attack.SPECIAL;
		}

		/*
			Returns true if the key was
			just released.
		*/
		public static bool Released(Keys k)
		{
			if (preState.IsKeyDown(k) &&
				curState.IsKeyUp(k))
			{
				return true;
			}
			else return false;
		}
	}
}
