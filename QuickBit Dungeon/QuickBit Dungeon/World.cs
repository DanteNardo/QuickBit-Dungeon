using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.DungeonGeneration;
using QuickBit_Dungeon.Interaction;
using QuickBit_Dungeon.Managers;
using QuickBit_Dungeon.UI;
using QuickBit_Dungeon.UI.HUD;

namespace QuickBit_Dungeon
{
	/// <summary>
	/// Handles all game functionality.
	///	Controls the flow of the game.
	/// </summary>
	public static class World
	{
		// ======================================
		// ============== Members ===============
		// ======================================

		private static Combat m_combat;			// Contains the methods for all combat
		private static StatBox m_statBox;		// Displays the player's stats
		private static ProgressBar m_healthBar;	// Displays the player's health mana bar
		private static ProgressBar m_attackBar;	// Displays the player's attack mana bar
		private static Scoring m_scoring;		// Displays the player's score and awards
		private static Timer m_levelTimer;		// Counts down how much time is left for this level

		// For drawing placement
		private static readonly Vector2 ScreenCenter = new Vector2(300, 300);

		public static int LevelCount { get; set; } = 1;

		// ======================================
		// =========== Main Methods =============
		// ======================================

		#region World Methods

		/// <summary>
		/// Initializes internal data.
		/// </summary>
		public static void Init()
		{
			Dungeon.Init();
			AwardHandler.Init();
			m_combat             = new Combat();
			m_statBox            = new StatBox();
			m_levelTimer		 = new Timer(120*60);
			m_healthBar          = new ProgressBar("Health Mana");
			m_attackBar          = new ProgressBar("Attack Mana");
			m_scoring            = new Scoring();
			m_healthBar.Position = new Vector2(10, 10);
			m_attackBar.Position = new Vector2(10, 40);
			m_healthBar.Init((int) Dungeon.MainPlayer.MaxMana, (int) Dungeon.MainPlayer.HealthMana);
			m_attackBar.Init((int) Dungeon.MainPlayer.MaxMana, (int) Dungeon.MainPlayer.AttackMana);
            m_combat.PerformCombat(Dungeon.MainPlayer, Dungeon.Monsters);
			m_levelTimer.PerformAction();
			m_statBox.GenerateStats(Dungeon.MainPlayer);
		}

		/// <summary>
		/// Handles all game updating.
		/// </summary>
		public static void Update()
		{
			if (OutOfTime()) return;
			if (GamePaused()) return;
			if (PlayerLeveledUp()) return;
			if (PlayerDied()) return;

			Dungeon.Update();
			AwardHandler.Update();
			GetAwards();
			NextLevel();
			PerformCombat();

		    m_levelTimer.Update();
			m_healthBar.UpdateValues((int) Dungeon.MainPlayer.MaxMana, (int) Dungeon.MainPlayer.HealthMana);
			m_attackBar.UpdateValues((int) Dungeon.MainPlayer.MaxMana, (int) Dungeon.MainPlayer.AttackMana);
			m_healthBar.Update();
			m_attackBar.Update();
			m_scoring.Update();
			m_statBox.GenerateStats(Dungeon.MainPlayer);
		}

		/// <summary>
		/// Loads all content for the game.
		/// </summary>
		public static void LoadContent()
		{
			// Stats box
			m_statBox.LoadContent();
		}

		// ======================================
		// =============== Combat ===============
		// ======================================

		/// <summary>
		/// Handles all combat.
		/// </summary>
		private static void PerformCombat()
		{
			if (CombatExists())
				m_combat.PerformCombat(Dungeon.MainPlayer, Dungeon.Monsters);
			m_combat.PlayerRegen();
		}

		/// <summary>
		/// Determines if combat is being exchanged.
		/// </summary>
		/// <returns>Whether or not combat can be exchanged</returns>
		private static bool CombatExists()
		{
			// Check player's position vs all monsters
			foreach (var m in Dungeon.Monsters)
			{
				if ((m.Y == Dungeon.MainPlayer.Y && m.X == Dungeon.MainPlayer.X + 1) ||
				    (m.Y == Dungeon.MainPlayer.Y && m.X == Dungeon.MainPlayer.X - 1) ||
				    (m.Y == Dungeon.MainPlayer.Y + 1 && m.X == Dungeon.MainPlayer.X) ||
				    (m.Y == Dungeon.MainPlayer.Y - 1 && m.X == Dungeon.MainPlayer.X))
				{
					return true;
				}
			}

			// Default return
			return false;
		}

		/// <summary>
		/// Gets the awards from the handler and adds
		/// them to the scoring class for displaying
		/// and scoring.
		/// </summary>
		private static void GetAwards()
		{
			foreach (var award in AwardHandler.Awards)
				m_scoring.AddAward(award);
			AwardHandler.Awards.Clear();
		}

		/// <summary>
		/// Determines if the player leveled up or not.
		/// </summary>
		/// <returns>Whether player leveled up</returns>
		private static bool PlayerLeveledUp()
		{
			if (Dungeon.MainPlayer.HasEnoughXp())
		    {
		        StateManager.SetState(StateManager.EGameState.LevelUp);
                return true;
		    }
			return false;
		}

		/// <summary>
		/// Determines if the player died or not.
		/// </summary>
		/// <returns>Whether player died</returns>
		private static bool PlayerDied()
		{
			if (Dungeon.MainPlayer.Health <= 0)
			{
				StateManager.SetState(StateManager.EGameState.GameOver);
				return true;
			}
			return false;
		}

		// ======================================
		// ============== Drawing ===============
		// ======================================

		/// <summary>
		/// Handles all game drawing.
		/// </summary>
		/// <param name="sb">The spritebatch</param>
		public static void Draw(SpriteBatch sb)
		{
			Dungeon.Draw(sb);
			m_statBox.DrawStats(sb);
			m_healthBar.DrawProgressBar(sb);
			m_attackBar.DrawProgressBar(sb);
			m_scoring.Draw(sb);
			m_levelTimer.Draw(sb, 500, 25);
		}

		// ======================================
		// ============== Utility ===============
		// ======================================

		/// <summary>
		/// Returns whether or not the player
		/// ran out of time for this level.
		/// </summary>
		/// <returns>Whether or not the player ran out of time</returns>
		private static bool OutOfTime()
		{
			if (m_levelTimer.ActionReady)
			{
				StateManager.GameState = StateManager.EGameState.GameOver;
				return true;
			}
			else return false;
		}

		/// <summary>
		/// Returns whether or not the player
		/// has paused the game.
		/// </summary>
		/// <returns>Whether or not the game is paused</returns>
		private static bool GamePaused()
		{
			if (Input.GamePaused)
			{
				StateManager.SetState(StateManager.EGameState.Pause);
				Input.GamePaused = false; // resets, won't be checked until state changes
				return true;
			}
			else return false;
		}

		/// <summary>
		/// Checks if the player has reached
		/// the next level. If they have, 
		/// generate the new level.
		/// </summary>
		private static void NextLevel()
		{
			if (Dungeon.EndReached())
			{
				Dungeon.NewLevel();
				m_levelTimer.PerformAction();
			}
		}

		#endregion
	}
}