using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.CORE;
using QuickBit_Dungeon.INTERACTION;
using QuickBit_Dungeon.MANAGERS;
using QuickBit_Dungeon.UI.EFFECTS;

namespace QuickBit_Dungeon.DUNGEON
{
	public static class Dungeon
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		private const int GridSize = 30;
		private const int ViewSize = 5;
		private const int DrawGridSize = (ViewSize*2) + 1;
		
		public static Player MainPlayer { get; set; }		
		public static List<Monster> Monsters { get; set; }
		private static Monster _target;
		
		private static DungeonGenerator DungeonGeneration { get; set; }
		private static List<List<DrawCell>> DrawGrid { get; set; }
        private static Light MainLight { get; set; }
        private static Light ExitLight { get; set; }
		public static List<List<Cell>> Grid { get; private set; }
		public static List<Room> Rooms { get; set; }
		private static int LevelCount { get; set; } = 1;

		// ======================================
		// =============== Main =================
		// ======================================

		/// <summary>
		/// The update method for the entire dungeon.
		/// </summary>
		public static void Update()
		{
			UpdatePlayer();
			UpdateMonsters();
			UpdateLights();
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
			Construct();
			GenerateDrawCells();
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
			MainPlayer.Y = DungeonGeneration.Start.Item1;
			MainPlayer.X = DungeonGeneration.Start.Item2;
			SetEntity(MainPlayer, MainPlayer.Y, MainPlayer.X);
			Grid[MainPlayer.Y][MainPlayer.X].NewLocal(MainPlayer);
			GenerateMonsters();
			GenerateLights();
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
			LevelUpMonsters(LevelCount);
			LevelCount++;
		}

		#endregion

		// ======================================
		// ============= Entities ===============
		// ======================================

		#region Entities
		
		/// <summary>
		/// Updates the main player.
		/// </summary>
		private static void UpdatePlayer()
		{
			MainPlayer.Update();
			UpdatePlayerMovement();
		}

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
					if (CanMove(MainPlayer, -1, 0))
						MoveEntity(MainPlayer, -1, 0);
					break;

				case Input.Direction.South:
					if (CanMove(MainPlayer, 1, 0))
						MoveEntity(MainPlayer, 1, 0);
					break;

				case Input.Direction.East:
					if (CanMove(MainPlayer, 0, 1))
						MoveEntity(MainPlayer, 0, 1);
					break;

				case Input.Direction.West:
					if (CanMove(MainPlayer, 0, -1))
						MoveEntity(MainPlayer, 0, -1);
					break;
			}
		}

		/// <summary>
		/// Initializes and places monsters 
		///	in rooms throughout the dungeon.
		/// </summary>
		private static void GenerateMonsters()
		{
			foreach (var r in Rooms)
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
							(Grid[ry][rx].Type != '@' || Grid[ry][rx].Type != ' '))
							break;
					}

					var m = new Monster();
					m.ConstructMonster();
					SetEntity(m, ry, rx);
					Monsters.Add(m);
					Grid[ry][rx].NewLocal(m);
				}
			}
		}

		/// <summary>
		/// Updates all monsters.
		/// </summary>
		private static void UpdateMonsters()
		{
			foreach (var m in Monsters)
				m.Update();
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
		/// Kills an entity and removes it from 
		/// the game data and visuals.
		/// </summary>
		/// <param name="e"></param>
		public static void KillEntity(Entity e)
		{
			Grid[e.Y][e.X].ClearLocal();
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
			return MainPlayer.Y == DungeonGeneration.Exit.Item1 &&
				   MainPlayer.X == DungeonGeneration.Exit.Item2;
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
		}

		/// <summary>
		/// Determines if the location the player
		/// is trying to move to is a valid location.
		/// </summary>
		/// <param name="e">The entity we want to move</param>
		/// <param name="y">The y modifying value</param>
		/// <param name="x">The x modifying value</param>
		/// <returns>Whether or not the entity can move to that position</returns>
		public static bool CanMove(Entity e, int y, int x)
		{
			var ny = e.Y + y;
			var nx = e.X + x;

			if (ny < 0 || ny >= GridSize || nx < 0 || nx >= GridSize)
				return false;
			else
				return Grid[ny][nx].Rep == '.' || 
					   Grid[ny][nx].Rep == '#' || 
					   Grid[ny][nx].Rep == '@';
		}

		/// <summary>
		/// Resets the representation of an entity
		/// in the game. ie they were damaged
		/// </summary>
		/// <param name="e">The entity to reset the rep of</param>
		public static void ResetRep(Entity e)
		{
			Grid[e.Y][e.X].Rep = GameManager.ConvertToChar(e.HealthRep);
		}

		/// <summary>
		/// Determines if the location in the grid
		/// is part of a room or not.
		/// </summary>
		/// <param name="y">The y coordinate</param>
		/// <param name="x">The x coordinate</param>
		/// <returns>Whether the location is a room</returns>
		public static bool IsARoom(int y, int x)
		{
			return Grid[y][x].Rep == '#';
		}

		#endregion

		// ======================================
		// ============== Drawing ===============
		// ======================================

		#region DungeonDrawing

		/// <summary>
		/// Generates the drawgrid and draw cells
		/// to handle all dungeon drawing.
		/// </summary>
		private static void GenerateDrawCells()
		{
			const int startX = 100;
			const int startY = 100;
			const int xIncr = 30;
			const int yIncr = 30;
			var newX = startX;
			var newY = startY;

			DrawGrid = new List<List<DrawCell>>();

			for (var i = 0; i < DrawGridSize; i++)
			{
				DrawGrid.Add(new List<DrawCell>());
				for (var j = 0; j < DrawGridSize; j++)
				{
					newX += xIncr;
					var dc = new DrawCell {Position = new Vector2(newX, newY)};
					DrawGrid[i].Add(dc);
				}
				newX = startX;
				newY += yIncr;
			}
		}

		/// <summary>
		/// Returns a string representing the player's
		/// view in the game.
		/// </summary>
		/// <returns>The string that represents the entire player view</returns>
		private static void GeneratePlayerView()
		{
			int x = MainPlayer.X;
			int y = MainPlayer.Y;
			int i1 = 0;
			int i2 = 0;

			DrawCell dcell;
			Cell cell;

			for (var i = y - ViewSize; i <= y + ViewSize; i++)
			{
				for (var j = x - ViewSize; j <= x + ViewSize; j++)
				{
					dcell = DrawGrid[i1][i2];

					if (i >= 0 && i < GridSize && j >= 0 && j < GridSize)
					{
						cell = Grid[i][j];
						dcell.GameObject = cell.Rep;
						dcell.Shade = cell.Local?.EntityColor ?? Color.White;
						dcell.Shade = dcell.Shade*cell.Alpha;
					}
					else
					{
						dcell.GameObject = ' ';
						dcell.Shade = Color.White;
					}
					i2++;
				}
				i2 = 0;
				i1++;
			}
		}

		/// <summary>
		/// Generates the main light and exit light.
		/// </summary>
		private static void GenerateLights()
		{
			// NOTE: Light size values must be ODD
			MainLight = new Light(MainPlayer.Y,
				MainPlayer.X,
				11);
			ExitLight = new Light(DungeonGeneration.Exit.Item1,
				DungeonGeneration.Exit.Item2,
				23);
			SetLight(MainLight);
			SetLight(ExitLight);
		}

		/// <summary>
		/// Updates all dungeon lights.
		/// </summary>
		private static void UpdateLights()
		{
			ClearAllCellsAlpha();
			MainLight.Update(MainPlayer.Y, MainPlayer.X);
			ExitLight.Update();
			SetLight(MainLight);
			SetLight(ExitLight);
		}

		/// <summary>
		/// Sets the light data in each cell.
		/// </summary>
		/// <param name="light">The light to place</param>
		private static void SetLight(Light light)
		{
			// For readability
			int hsize = light.LightSize/2;
			int x = light.X;
			int y = light.Y;
			int i1 = 0, i2 = 0;

			for (int i = y - hsize; i <= y + hsize; i++)
			{
				for (int j = x - hsize; j <= x + hsize; j++)
				{
					if (i >= 0 && i < GridSize && j >= 0 && j < GridSize)
					{
						Grid[i][j].Alpha += (float) light.LightData[i1][i2];
						if (Grid[i][j].Alpha > 1) Grid[i][j].Alpha = 1;
					}
					i2++;
				}
				i2 = 0;
				i1++;
			}
		}

		/// <summary>
		/// Clears every cell's alpha.
		/// </summary>
		private static void ClearAllCellsAlpha()
		{
			foreach (var row in Grid)
				foreach (var cell in row)
					cell.Alpha = 0;
		}

		/// <summary>
		/// Draws the entire dungeon.
		/// </summary>
		/// <param name="sb">The spritebatch to draw with</param>
		public static void Draw(SpriteBatch sb)
		{
			GeneratePlayerView();

			foreach (var row in DrawGrid)
				foreach (var cell in row)
				{
					sb.DrawString(ArtManager.DungeonFont,
						cell.GameObject.ToString(),
						cell.Position,
						cell.Shade);
				}
		}

		#endregion

	}
}
