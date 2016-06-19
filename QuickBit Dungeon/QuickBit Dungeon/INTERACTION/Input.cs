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
		
		private static KeyboardState preKeyState;
		private static KeyboardState curKeyState;

		private static GamePadState preGamepadState;
		private static GamePadState curGamepadState;

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

		private static ePlayerState playerState;
		public static  ePlayerState PlayerState
		{
			get { return playerState; }
			set { playerState = value; }
		}
		public enum ePlayerState
		{
			PHYSICAL,
			SPECIAL,
			HEALING,
			CHARGING,
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
			preKeyState = curKeyState;
			curKeyState = Keyboard.GetState();

			preGamepadState = curGamepadState;
			curGamepadState = GamePad.GetState(0);
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
			if (Released(Keys.W) || Released(Buttons.DPadUp))
				currentDirection = Direction.NORTH;
			// South
			if (Released(Keys.S) || Released(Buttons.DPadDown))
				currentDirection = Direction.SOUTH;
			// East
			if (Released(Keys.D) || Released(Buttons.DPadLeft))
				currentDirection = Direction.EAST;
			// West
			if (Released(Keys.A) || Released(Buttons.DPadRight))
				currentDirection = Direction.WEST;

			// ======================================
			// ============== Combat ================
			// ======================================
			
			// Physical
			if (Released(Keys.J) || Released(Buttons.A))
				playerState = ePlayerState.PHYSICAL;
			// Special
			if (Released(Keys.I) || Released(Buttons.Y))
				playerState = ePlayerState.SPECIAL;
			// Healing
			if (Released(Keys.L) || Released(Buttons.B))
				playerState = ePlayerState.HEALING;
			// Charging Mana
			if (Released(Keys.K) || Released(Buttons.X))
				playerState = ePlayerState.CHARGING;
		}

		/*
			Returns true if the key was
			just released.
		*/
		public static bool Released(Keys k)
		{
			if (preKeyState.IsKeyDown(k) &&
				curKeyState.IsKeyUp(k))
			{
				return true;
			}
			else return false;
		}

		/*
			Returns true if the gamepad button
			was just released.
		*/
		public static bool Released(Buttons b)
		{
			if (preGamepadState.IsButtonDown(b) &&
				curGamepadState.IsButtonUp(b))
			{
				return true;
			}
			else return false;
		}
	}
}
