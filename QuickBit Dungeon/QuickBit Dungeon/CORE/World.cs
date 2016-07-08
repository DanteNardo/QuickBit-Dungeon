using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.DUNGEON;
using QuickBit_Dungeon.INTERACTION;
using QuickBit_Dungeon.UI;
using QuickBit_Dungeon.UI.Effects;

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

		private static Monster _target;			// Stores the currently targeted enemy
		private static Combat _combat;			// Contains the methods for all combat
		private static StatBox _statBox;		// Displays the player's stats
		private static Light _light;			// Draws the lighting effect
		private static ProgressBar _healthBar;	// Displays the player's health mana bar
		private static ProgressBar _attackBar;	// Displays the player's attack mana bar
		private static Timer _levelTimer;		// Counts down how much time is left for this level

		// For drawing placement
		private static readonly Vector2 ScreenCenter = new Vector2(300, 300);
		private static Vector2 _dgPos;

		// Properties
		public static Player MainPlayer { get; set; }
		public static List<Monster> Monsters { get; set; }
		public static Random Rand { get; set; }
		public static int LevelCount { get; set; } = 1;

		// ======================================
		// =========== Main Methods =============
		// ======================================

		/// <summary>
		/// Initializes internal data.
		/// </summary>
		public static void Init()
		{
			MainPlayer          = new Player();
			Monsters            = new List<Monster>();
			Rand                = new Random();
			_combat             = new Combat();
			_light              = new Light();
			_statBox            = new StatBox();
			_levelTimer			= new Timer(120*60);
			_healthBar          = new ProgressBar("Health Mana");
			_attackBar          = new ProgressBar("Attack Mana");
			_healthBar.Position = new Vector2(10, 10);
			_attackBar.Position = new Vector2(10, 50);
			_healthBar.Init((int) MainPlayer.MaxMana, (int) MainPlayer.HealthMana);
			_attackBar.Init((int) MainPlayer.MaxMana, (int) MainPlayer.AttackMana);
			_levelTimer.PerformAction();
			_statBox.GenerateStats(MainPlayer);
			GenerateMonsters();
			Dungeon.GetPlayerPosition(MainPlayer);
		}

		/// <summary>
		/// Initializes and places monsters 
		///	in rooms throughout the dungeon.
		/// </summary>
		private static void GenerateMonsters()
		{
			foreach (var r in Dungeon.Rooms)
			{
				var monsterAmount = Rand.Next(1, 4);

				for (var i = 0; i < monsterAmount; i++)
				{
					int ry;
					int rx;
					while (true)
					{
						ry = Rand.Next(r.Position[0], r.Position[0] + r.Height);
						rx = Rand.Next(r.Position[1], r.Position[1] + r.Width);

						if (!MonsterAt(ry, rx, ref _target) &&
							Dungeon.Grid[ry][rx].Type != '@')
							break;
					}

					var m = new Monster();
					m.ConstructMonster();
					m.Y = ry;
					m.X = rx;
					Dungeon.ResetRep(ry, rx, m.HealthRep);
					Monsters.Add(m);
				}
			}
		}

		/// <summary>
		/// Levels up all of the monsters in a
		/// level. Increases level difficulty.
		/// </summary>
		/// <param name="levelCount"></param>
		private static void LevelUpMonsters(int levelCount)
		{
			var colors = new List<string> {"red", "blue", "green"};

			for (int i = 0; i < levelCount; i++)
				foreach (var m in Monsters)
					m.LevelUp(colors[GameManager.Random.Next(0, 3)]);
		}

		/// <summary>
		/// Handles all game updating.
		/// </summary>
		public static void Update()
		{
			// TODO: Make this one method
			if (_levelTimer.ActionReady)
			{
				StateManager.GameState = StateManager.EGameState.GameOver;
				return;
			}

			// TODO: Make this one method
			if (Input.GamePaused)
			{
				StateManager.SetState(StateManager.EGameState.Pause);
				Input.GamePaused = false; // resets, won't be checked until state changes
				return;
			}

			// TODO: Make this one method
			if (Dungeon.EndReached())
			{
				Dungeon.NewLevel();
				Dungeon.GetPlayerPosition(MainPlayer);
				Monsters.Clear();
				Monsters = new List<Monster>();
				GenerateMonsters();
				LevelUpMonsters(LevelCount);
				_levelTimer.PerformAction();
				LevelCount++;
			}

			foreach (var m in Monsters)
				m.Update();

			MovePlayer();
			
			if (CombatExists())
				_combat.PerformCombat(MainPlayer, Monsters);
			_combat.PlayerRegen();
			
			if (MainPlayer.HasEnoughXp())
				MainPlayer.LevelUp("red"); // Later will be switched based on UI selection

			_levelTimer.Update();
			_healthBar.UpdateValues((int) MainPlayer.MaxMana, (int) MainPlayer.HealthMana);
			_attackBar.UpdateValues((int) MainPlayer.MaxMana, (int) MainPlayer.AttackMana);
			_healthBar.Update();
			_attackBar.Update();
			_statBox.GenerateStats(MainPlayer);

			if (PlayerDied())
				StateManager.SetState(StateManager.EGameState.GameOver);
		}

		/// <summary>
		/// Loads all content for the game.
		/// </summary>
		public static void LoadContent()
		{
			// Stats box
			_statBox.LoadContent();
			var stringSize = ArtManager.DungeonFont.MeasureString(Dungeon.PlayerView())/2;
			_dgPos = ScreenCenter - stringSize;

			// Special Effects
			_light.LoadContent();
			_light.PositionLight(_dgPos);
		}

		/// <summary>
		/// Moves the player in the dungeon using
		///	the input class, dungeon class, and
		///	the user's given input.
		/// </summary>
		private static void MovePlayer()
		{
			Input.GetInput();

			switch (Input.CurrentDirection)
			{
				case Input.Direction.North:
					if (Dungeon.CanMove(MainPlayer, -1, 0))
					{
						Dungeon.MoveEntity(MainPlayer, -1, 0, GameManager.ConvertToChar(MainPlayer.HealthRep));
						MainPlayer.Y = Dungeon.PlayerY;
						MainPlayer.X = Dungeon.PlayerX;
					}
					break;

				case Input.Direction.South:
					if (Dungeon.CanMove(MainPlayer, 1, 0))
					{
						Dungeon.MoveEntity(MainPlayer, 1, 0, GameManager.ConvertToChar(MainPlayer.HealthRep));
						MainPlayer.Y = Dungeon.PlayerY;
						MainPlayer.X = Dungeon.PlayerX;
					}
					break;

				case Input.Direction.East:
					if (Dungeon.CanMove(MainPlayer, 0, 1))
					{
						Dungeon.MoveEntity(MainPlayer, 0, 1, GameManager.ConvertToChar(MainPlayer.HealthRep));
						MainPlayer.Y = Dungeon.PlayerY;
						MainPlayer.X = Dungeon.PlayerX;
					}
					break;

				case Input.Direction.West:
					if (Dungeon.CanMove(MainPlayer, 0, -1))
					{
						Dungeon.MoveEntity(MainPlayer, 0, -1, GameManager.ConvertToChar(MainPlayer.HealthRep));
						MainPlayer.Y = Dungeon.PlayerY;
						MainPlayer.X = Dungeon.PlayerX;
					}
					break;
			}
		}


		// ======================================
		// =============== Combat ===============
		// ======================================

		/// <summary>
		/// Determines if combat is being exchanged.
		/// </summary>
		/// <returns>Whether or not combat can be exchanged</returns>
		private static bool CombatExists()
		{
			// Check player's position vs all monsters
			foreach (var m in Monsters)
			{
				if ((m.Y == MainPlayer.Y && m.X == MainPlayer.X + 1) ||
				    (m.Y == MainPlayer.Y && m.X == MainPlayer.X - 1) ||
				    (m.Y == MainPlayer.Y + 1 && m.X == MainPlayer.X) ||
				    (m.Y == MainPlayer.Y - 1 && m.X == MainPlayer.X))
				{
					return true;
				}
			}

			// Default return
			return false;
		}

		/// <summary>
		/// Determines if a monster exists at those
		///	coordinates.
		/// </summary>
		/// <param name="y">The y coordinate</param>
		/// <param name="x">The x coordinate</param>
		/// <param name="target">The monster to check for</param>
		/// <returns>Whether or not a monster is at that position</returns>
		public static bool MonsterAt(int y, int x, ref Monster target)
		{
			foreach (var m in Monsters)
			{
				if (m.Y != y || m.X != x) continue;
				target = m;
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
			return MainPlayer.Health <= 0;
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
			DrawDungeon(sb);
			_light.DrawLight(sb);
			_statBox.DrawStats(sb);
			_healthBar.DrawProgressBar(sb);
			_attackBar.DrawProgressBar(sb);
			_levelTimer.Draw(sb, 500, 25);
		}

		/// <summary>
		/// Draws the player's view of the dungeon.
		/// </summary>
		/// <param name="sb">The spritebatch</param>
		private static void DrawDungeon(SpriteBatch sb)
		{
			sb.DrawString(ArtManager.DungeonFont,
							Dungeon.PlayerView(),
							_dgPos,
							Color.White);
		}
	}
}