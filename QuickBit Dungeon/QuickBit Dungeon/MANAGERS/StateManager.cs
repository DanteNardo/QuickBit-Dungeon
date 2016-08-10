namespace QuickBit_Dungeon.MANAGERS
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
            LevelUp,
            RedLevelUp,
            GreenLevelUp,
            BlueLevelUp,
			GameOver,
			Exit,
			Reset,
            None
		}
		public static EGameState GameState { get; set; } = EGameState.MainMenu;
		public static EGameState LastState { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Sets the current and previous game states.
		/// </summary>
		/// <param name="newState">The game state to change to</param>
		public static void SetState(EGameState newState)
		{
			LastState = GameState;
			GameState = newState;
		}
	}
}
