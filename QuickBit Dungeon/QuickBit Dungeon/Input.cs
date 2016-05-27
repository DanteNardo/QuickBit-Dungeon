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

		static KeyboardState keyState;
		static bool canMove = true;
		static int maxTime = 15;
		static int curTime = maxTime;

		static Direction direction;
		enum Direction
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

			ProcessInput();
		}

		/*
			Handles all input functionality.
			Also handles interaction between
			movement and the dungeon.
		*/
		private static void ProcessInput()
		{
			if (canMove)
			{
				GetInput();

				switch (direction)
				{
					case Direction.NORTH:
						if (Dungeon.CanMove(-1, 0))
						{
							Dungeon.MovePlayer(-1, 0);
							canMove = false;
							direction = Direction.NONE;
						}
						break;

					case Direction.SOUTH:
						if (Dungeon.CanMove(1, 0))
						{
							Dungeon.MovePlayer(1, 0);
							canMove = false;
							direction = Direction.NONE;
						}
						break;

					case Direction.EAST:
						if (Dungeon.CanMove(0, 1))
						{
							Dungeon.MovePlayer(0, 1);
							canMove = false;
							direction = Direction.NONE;
						}
						break;

					case Direction.WEST:
						if (Dungeon.CanMove(0, -1))
						{
							Dungeon.MovePlayer(0, -1);
							canMove = false;
							direction = Direction.NONE;
						}
						break;
				}
			}
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
				direction = Direction.NORTH;
			// South
			if (keyState.IsKeyDown(Keys.S))
				direction = Direction.SOUTH;
			// East
			if (keyState.IsKeyDown(Keys.D))
				direction = Direction.EAST;
			// West
			if (keyState.IsKeyDown(Keys.A))
				direction = Direction.WEST;
		}
	}
}
