using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.DUNGEON;
using QuickBit_Dungeon.INTERACTION;
using QuickBit_Dungeon.MANAGERS;
using QuickBit_Dungeon.UI;

namespace QuickBit_Dungeon.CORE
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

		private static Combat _combat;			// Contains the methods for all combat
		private static StatBox _statBox;		// Displays the player's stats
		private static ProgressBar _healthBar;	// Displays the player's health mana bar
		private static ProgressBar _attackBar;	// Displays the player's attack mana bar
		private static Timer _levelTimer;		// Counts down how much time is left for this level

		// For drawing placement
		private static readonly Vector2 ScreenCenter = new Vector2(300, 300);

		public static int LevelCount { get; set; } = 1;

		// ======================================
		// =========== Main Methods =============
		// ======================================

		/// <summary>
		/// Initializes internal data.
		/// </summary>
		public static void Init()
		{
			Dungeon.Init();
			_combat             = new Combat();
			_statBox            = new StatBox();
			_levelTimer			= new Timer(120*60);
			_healthBar          = new ProgressBar("Health Mana");
			_attackBar          = new ProgressBar("Attack Mana");
			_healthBar.Position = new Vector2(10, 10);
			_attackBar.Position = new Vector2(10, 50);
			_healthBar.Init((int) Dungeon.MainPlayer.MaxMana, (int) Dungeon.MainPlayer.HealthMana);
			_attackBar.Init((int) Dungeon.MainPlayer.MaxMana, (int) Dungeon.MainPlayer.AttackMana);
            _combat.PerformCombat(Dungeon.MainPlayer, Dungeon.Monsters);
			_levelTimer.PerformAction();
			_statBox.GenerateStats(Dungeon.MainPlayer);
		}

		/// <summary>
		/// Handles all game updating.
		/// </summary>
		public static void Update()
		{
			if (OutOfTime()) return;
			if (GamePaused()) return;
			if (PlayerDied()) return;

			Dungeon.Update();
			NextLevel();

			PerformCombat();
			
			if (Dungeon.MainPlayer.HasEnoughXp())
				Dungeon.MainPlayer.LevelUp("red"); // Later will be switched based on UI selection

			_levelTimer.Update();
			_healthBar.UpdateValues((int) Dungeon.MainPlayer.MaxMana, (int) Dungeon.MainPlayer.HealthMana);
			_attackBar.UpdateValues((int) Dungeon.MainPlayer.MaxMana, (int) Dungeon.MainPlayer.AttackMana);
			_healthBar.Update();
			_attackBar.Update();
			_statBox.GenerateStats(Dungeon.MainPlayer);
		}

		/// <summary>
		/// Loads all content for the game.
		/// </summary>
		public static void LoadContent()
		{
			// Stats box
			_statBox.LoadContent();
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
				_combat.PerformCombat(Dungeon.MainPlayer, Dungeon.Monsters);
			_combat.PlayerRegen();
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
			_statBox.DrawStats(sb);
			_healthBar.DrawProgressBar(sb);
			_attackBar.DrawProgressBar(sb);
			_levelTimer.Draw(sb, 500, 25);
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
			if (_levelTimer.ActionReady)
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
				_levelTimer.PerformAction();
			}
		}
	}
}