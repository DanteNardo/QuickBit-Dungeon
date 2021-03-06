﻿using Microsoft.Xna.Framework.Input;

namespace QuickBit_Dungeon.Interaction
{
	public static class Input
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		private static KeyboardState m_preKeyState;
		private static KeyboardState m_curKeyState;

		private static GamePadState m_preGamepadState;
		private static GamePadState m_curGamepadState;

		public static Direction CurrentDirection { get; set; }
		public static Direction LastDirection { get; set; }
		public enum Direction
		{
			North,
			South,
			East,
			West,
			None
		}

		public static EPlayerState PlayerState { get; set; }
		public enum EPlayerState
		{
			Physical,
			Special,
			Healing,
			Charging,
			None
		}

		public static bool GamePaused { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		#region Input Methods

		/// <summary>
		/// Updates the keyboard state variable
		///	every frame.
		/// </summary>
		public static void Update()
		{
			m_preKeyState = m_curKeyState;
			m_curKeyState = Keyboard.GetState();

			m_preGamepadState = m_curGamepadState;
			m_curGamepadState = GamePad.GetState(0);
		}

		/// <summary>
		/// Determines if correct keys are
		///	being pressed to count as input.
		///	Updates direction accordingly.
		/// </summary>
		public static void GetInput()
		{
			// Reset input variables
			if (CurrentDirection != Direction.None)
				LastDirection = CurrentDirection;
			CurrentDirection = Direction.None;
			PlayerState = EPlayerState.None;

			// Check to see if game is paused
			if (Released(Keys.P) || Released(Buttons.Start))
			{
				GamePaused = true;
				return;
			}

			// ======================================
			// ============= Movement ===============
			// ======================================

			// North
			if (Released(Keys.W) || Released(Buttons.DPadUp))
				CurrentDirection = Direction.North;
			// South
			if (Released(Keys.S) || Released(Buttons.DPadDown))
				CurrentDirection = Direction.South;
			// East
			if (Released(Keys.D) || Released(Buttons.DPadRight))
				CurrentDirection = Direction.East;
			// West
			if (Released(Keys.A) || Released(Buttons.DPadLeft))
				CurrentDirection = Direction.West;

			// ======================================
			// ============== Combat ================
			// ======================================

			// Physical
			if (Released(Keys.J) || Released(Buttons.A))
				PlayerState = EPlayerState.Physical;
			// Special
			if (Released(Keys.I) || Released(Buttons.Y))
				PlayerState = EPlayerState.Special;
			// Healing
			if (Released(Keys.L) || Released(Buttons.B))
				PlayerState = EPlayerState.Healing;
			// Charging Mana
			if (Released(Keys.K) || Released(Buttons.X))
				PlayerState = EPlayerState.Charging;
		}

		/// <summary>
		/// Returns true if the key was
		///	just released.
		/// </summary>
		/// <param name="k">The key to check if pressed</param>
		/// <returns>Whether the key was pressed or not</returns>
		public static bool Released(Keys k)
		{
			return m_preKeyState.IsKeyDown(k) &&
			       m_curKeyState.IsKeyUp(k);
		}

		/// <summary>
		/// Returns true if the gamepad button
		///	was just released.
		/// </summary>
		/// <param name="b">The button to check if pressed</param>
		/// <returns>Whether the button was pressed or not</returns>
		public static bool Released(Buttons b)
		{
			return m_preGamepadState.IsButtonDown(b) &&
			       m_curGamepadState.IsButtonUp(b);
		}

		#endregion
	}
}
