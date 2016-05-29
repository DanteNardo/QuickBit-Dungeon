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

		private static bool canMove = true;
		public static bool CanMove
		{
			get { return canMove; }
			set { canMove = value; }
		}
		private static int maxTime = 15;
		private static int curTime = maxTime;

		private static Direction currentDirection;
		public static Direction CurrentDirection
		{
			get { return currentDirection; }
			set { currentDirection = value; }
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

			// Handle time
			if (!canMove) curTime--;
			if (curTime == 0)
			{
				curTime = maxTime;
				canMove = true;
			}
		}

		/*
			Handles all input functionality.
			Also handles interaction between
			movement and the dungeon.
		*/
		public static bool Moving()
		{
			if (canMove)
			{
				GetInput();

				switch (currentDirection)
				{
					case Direction.NORTH:
						if (Dungeon.CanMove(-1, 0))
						{
							//Dungeon.MovePlayer(-1, 0);
							//canMove = false;
							//currentDirection = Direction.NONE;
							return true;
						}
						break;

					case Direction.SOUTH:
						if (Dungeon.CanMove(1, 0))
							return true;
						break;

					case Direction.EAST:
						if (Dungeon.CanMove(0, 1))
							return true;
						break;

					case Direction.WEST:
						if (Dungeon.CanMove(0, -1))
							return true;
						break;
				}
			}
			
			// Default return - when we aren't moving
			return false;
		}

		/*
			Determines if correct keys are
			being pressed to count as input.
			Updates direction accordingly.
		*/
		private static void GetInput()
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
