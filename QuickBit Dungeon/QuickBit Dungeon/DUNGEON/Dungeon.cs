using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.CORE;
using QuickBit_Dungeon.INTERACTION;
using QuickBit_Dungeon.MANAGERS;

namespace QuickBit_Dungeon.DUNGEON
{
	public static class Dungeon
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		
		private const int GridSize = 20;
		private const int ViewSize = 5;
		
		public static Player MainPlayer { get; set; }		
		public static List<Monster> Monsters { get; set; }
		private static Monster Target { get; set; }	
		
		private static DungeonGenerator DungeonGeneration { get; set; }
		public static List<List<Cell>> Grid { get; private set; }
		public static List<Room> Rooms { get; set; }

		// ======================================
		// =============== Main =================
		// ======================================

		/// <summary>
		/// The update method for the entire dungeon.
		/// </summary>
		public static void Update()
		{
			UpdatePlayerMovement();
		}

		// ======================================
		// ============ Generation ==============
		// ======================================

		#region DungeonGeneration
		
		/// <summary>
		/// First time initialization.
		/// </summary>
		public static void Init()
		{
			MainPlayer = new Player();
			Monsters   = new List<Monster>();
			GenerateMonsters();
		}

		/// <summary>
		/// Use this for initialization.
		/// Also, generates the dungeon.
		/// </summary>
		public static void Construct()
		{
			Grid = new List<List<Cell>>();
			Rooms = new List<Room>();
			DungeonGeneration = new DungeonGenerator(Grid, Rooms);
			DungeonGeneration.GenerateDungeon();
		}

		/// <summary>
		/// Completely clears the current level
		/// then constructs a new one.
		/// </summary>
		public static void NewLevel()
		{
			Grid.Clear();
			Rooms.Clear();
			Construct();
		}

		#endregion

		// ======================================
		// ============= Entities ===============
		// ======================================

		#region Entities

		/// <summary>
		/// Moves the player in the dungeon using
		///	the input class, dungeon class, and
		///	the user's given input.
		/// </summary>
		private static void UpdatePlayerMovement()
		{
			Input.GetInput();

			switch (Input.CurrentDirection)
			{
				case Input.Direction.North:
					if (Dungeon.CanMove(MainPlayer, -1, 0))
						Dungeon.MoveEntity(MainPlayer, -1, 0);
					break;

				case Input.Direction.South:
					if (Dungeon.CanMove(MainPlayer, 1, 0))
						Dungeon.MoveEntity(MainPlayer, 1, 0);
					break;

				case Input.Direction.East:
					if (Dungeon.CanMove(MainPlayer, 0, 1))
						Dungeon.MoveEntity(MainPlayer, 0, 1);
					break;

				case Input.Direction.West:
					if (Dungeon.CanMove(MainPlayer, 0, -1))
						Dungeon.MoveEntity(MainPlayer, 0, -1);
					break;
			}
		}

		/// <summary>
		/// Initializes and places monsters 
		///	in rooms throughout the dungeon.
		/// </summary>
		private static void GenerateMonsters()
		{
			foreach (var r in Dungeon.Rooms)
			{
				var monsterAmount = GameManager.Random.Next(1, 4);

				for (var i = 0; i < monsterAmount; i++)
				{
					int ry;
					int rx;
					while (true)
					{
						ry = GameManager.Random.Next(r.Position[0], r.Position[0] + r.Height);
						rx = GameManager.Random.Next(r.Position[1], r.Position[1] + r.Width);

						if (!MonsterAt(ry, rx, ref _target) &&
							Dungeon.Grid[ry][rx].Type != '@')
							break;
					}

					var m = new Monster();
					m.ConstructMonster();
					m.Y = ry;
					m.X = rx;
					Dungeon.ResetRep(m);
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

		#endregion

		// ======================================
		// ============ Interaction =============
		// ======================================

		#region DungeonInteraction

		/// <summary>
		/// Determines if the player reached
		/// the current level's exit.
		/// </summary>
		/// <returns>Whether exit was reached or not</returns>
		public static bool EndReached()
		{
			return _pPos[0] == _ePos[0] && _pPos[1] == _ePos[1];
		}

		/// <summary>
		/// Sets the entity's position in the egrid.
		/// </summary>
		/// <param name="y">Initial y position</param>
		/// <param name="x">Initial x position</param>
		public static void SetEntity(Entity e, int y, int x)
		{
			e.Y = y;
			e.X = x;
			ResetRep(e);
		}

		/// <summary>
		/// Sets the entity's position relative
		/// to it's current position.
		/// </summary>
		/// <param name="e">The entity that is moving</param>
		/// <param name="y">The y modifying value</param>
		/// <param name="x">The x modifying value</param>
		public static void MoveEntity(Entity e, int y, int x)
		{
			Grid[e.Y][e.X].ClearLocal();
			e.Y += y;
			e.X += x;
			Grid[e.Y][e.X].NewLocal(e);

			// Update internal player position if necessary
			if (e is Player)
			{
				_pPos[0] += y;
				_pPos[1] += x;
			}
		}

		/// <summary>
		/// Determines if the location the player
		/// is trying to move to is a valid location.
		/// </summary>
		/// <param name="y">The y position to move to</param>
		/// <param name="x">The x position to move to</param>
		/// <returns>Whether or not the entity can move to that position</returns>
		public static bool CanMove(Entity e, int y, int x)
		{
			var ny = e.Y + y;
			var nx = e.X + x;

			if (ny < 0 || ny >= GridSize || nx < 0 || nx >= GridSize) return false;
			return Grid[ny][nx].Rep == '.' || Grid[ny][nx].Rep == '#' || Grid[ny][nx].Rep == '@';
		}

		/// <summary>
		/// Resets the representation of an entity
		/// in the game. ie they were damaged
		/// </summary>
		/// <param name="e"></param>
		public static void ResetRep(Entity e)
		{
			Grid[e.Y][e.X].Rep = GameManager.ConvertToChar(e.HealthRep);
		}

		#endregion

		// ======================================
		// ============== Drawing ===============
		// ======================================

		/// <summary>
		/// Returns a string representing the player's
		/// view in the game.
		/// </summary>
		/// <returns>The string that represents the entire player view</returns>
		public static void PlayerView()
		{
			var x = MainPlayer.X;
			var y = MainPlayer.Y;

			for (var i = y - ViewSize; i <= y + ViewSize; i++)
			{
				for (var j = x - ViewSize; j <= x + ViewSize; j++)
				{
					if (i >= 0 && i < GridSize && j >= 0 && j < GridSize)
					{
						
					}
					else
					{
						
					}
				}
			}
		}

		/// <summary>
		/// Draws the entire dungeon.
		/// </summary>
		/// <param name="sb">The spritebatch to draw with</param>
		public static void Draw(SpriteBatch sb)
		{
			
		}
	}
}
