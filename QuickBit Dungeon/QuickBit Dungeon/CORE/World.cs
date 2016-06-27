using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.DUNGEON;
using QuickBit_Dungeon.INTERACTION;
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

		private static Monster _target; // Stores the currently targeted enemy
		private static Combat _combat; // Contains the methods for all combat
		private static StatBox _statBox; // Displays the player's stats
		private static Light _light; // Draws the lighting effect
		private static ProgressBar _healthBar; // Displays the player's health mana bar
		private static ProgressBar _attackBar; // Displays the player's attack mana bar

		// For drawing placement
		private static readonly Vector2 ScreenCenter = new Vector2(300, 300);
		private static Vector2 _dgPos;

		// Properties
		public static Player MainPlayer { get; set; }
		public static List<Monster> Monsters { get; set; }
		public static Random Rand { get; set; }

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
			_healthBar          = new ProgressBar("Health Mana");
			_attackBar          = new ProgressBar("Attack Mana");
			_healthBar.Position = new Vector2(10, 10);
			_attackBar.Position = new Vector2(10, 50);
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

						if (!MonsterAt(ry, rx, ref _target)) break;
					}

					var m = new Monster();
					m.ConstructMonster();
					m.Y = ry;
					m.X = rx;
					Dungeon.Grid[ry][rx].Rep = GameManager.ConvertToChar(m.HealthRep);
					Monsters.Add(m);
				}
			}
		}

		/// <summary>
		/// Handles all game updating.
		/// </summary>
		public static void Update()
		{
			// Update all entities
			foreach (var m in Monsters)
				m.Update();

			// Update all input
			Input.Update();
			MovePlayer();

			// Update all combat and regeneration
			if (CombatExists())
				_combat.PerformCombat(MainPlayer, Monsters);
			_combat.PlayerRegen();
			
			// XP and Level Up
			if (MainPlayer.HasEnoughXp())
				MainPlayer.LevelUp("red"); // Later will be switched based on UI selection

			// Update all progress bars
			_healthBar.UpdateValues((int) MainPlayer.MaxMana, (int) MainPlayer.HealthMana);
			_attackBar.UpdateValues((int) MainPlayer.MaxMana, (int) MainPlayer.AttackMana);
			_healthBar.Update();
			_attackBar.Update();

			// Update the stats box
			_statBox.GenerateStats(MainPlayer);
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