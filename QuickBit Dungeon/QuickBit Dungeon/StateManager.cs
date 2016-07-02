using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickBit_Dungeon
{
	public static class StateManager
	{
		// ======================================
		// ============== Members ===============
		// ======================================	
		
		public enum EGameState
		{
			MainMenu,
			HowTo,
			Pause,
			Game,
			GameOver,
			Exit,
			Reset
		}
		public static EGameState GameState { get; set; } = EGameState.MainMenu;
		public static EGameState LastState { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================
	}
}
