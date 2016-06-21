using System;
using System.Collections.Generic;
using System.Linq;
using QuickBit_Dungeon.CORE;

namespace QuickBit_Dungeon.DUNGEON
{
	public static class Dungeon
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		private const int _gridSize = 50;
		private static int[] _pPos;
		private const int _viewSize = 5;
		private static Random _rnd;

		public static int PlayerY
		{
			get { return _pPos[0]; }
		}
		public static int PlayerX
		{
			get { return _pPos[1]; }
		}

		public static List<List<Cell>> Grid { get; private set; }
		public static List<Room> Rooms { get; set; }

		// ======================================
		// ============= Utilities ==============
		// ======================================

		#region Utilities

		/// <summary>
		/// Checks to see if a given cell's neighbors 
		/// have neighboring cells that are already 
		/// part of rooms. If so, return true since this
		/// cell is neighboring a room
		/// </summary>
		/// <param name="y">The y cell coordinate</param>
		/// <param name="x">The x cell coordinate</param>
		/// <returns>Whether or not the room has neighbors</returns>
		private static bool RoomNeighbor(int y, int x)
		{
			foreach (var n in Grid[y][x].Neighbors)
				foreach (var location in Grid[n[0]][n[1]].Neighbors)
					if (Grid[location[0]][location[1]].Type == '#')
						return true;
			return false;
		}

		/// <summary>
		/// Checks to see if a given cell's neighbors
		/// are already part of rooms. If so, return
		/// true since this cell is near neighboring
		/// a room.
		/// </summary>
		/// <param name="y">The y coordinate</param>
		/// <param name="x">The x coordinate</param>
		/// <returns>Whether or not there the cell neighbors a room</returns>
		private static bool RoomNearNeighbor(int y, int x)
		{
			return Grid[y][x].Neighbors.Any(n => Grid[n[0]][n[1]].Type == '#');
		}

		/// <summary>
		/// Determines if a square's board position
		/// is a valid square. If so, return true.
		/// </summary>
		/// <param name="y">The y coordinate</param>
		/// <param name="x">The x coordinate</param>
		/// <returns>Whether or not a cell exists within the dungeon</returns>
		private static bool ValidSquare(int y, int x)
		{
			return y >= 0 && x >= 0 && y < _gridSize && x < _gridSize;
		}

		/// <summary>
		/// Determines if a cell is valid.
		/// (Prim's algorithm conditions)
		/// </summary>
		/// <param name="cell">The cell to check</param>
		/// <returns>Whether it is valid for Prims or not</returns>
		private static bool ValidMazeCell(Cell cell)
		{
			if (cell.Type == '.')
				return false;
			
			// Valid cells have less than two open neighbors
			var count = cell.Neighbors.Count(neighbor => Grid[neighbor[0]][neighbor[1]].Type == '.');
			return count < 2;
		}

		/// <summary>
		/// Finds the x and y location of pockets.
		/// Also returns the amount of iterations
		/// it took to find the pocket.
		/// </summary>
		/// <returns>The starting location of a pocket</returns>
		private static int[] FindPockets()
		{
			var count = 0;
			for (var i = 0; i < _gridSize; i++)
				for (var j = 0; j < _gridSize; j++)
				{
					if (ValidMazeCell(Grid[i][j]) &&
					    RoomNearNeighbor(i, j) == false)

						// Select new starting coordinates
						return new int[3] {i, j, count};

					count++;
				}

			// Default return
			return new int[3] {0, 0, -1};
		}

		/// <summary>
		/// Returns true if position is a dead
		/// end. A cell having one neighbor or
		/// less means it is a dead end.
		/// </summary>
		/// <param name="pos">The position to check</param>
		/// <returns>Returns whether or not a position is a dead end</returns>
		private static bool DeadEnd(int[] pos)
		{
			if (Grid[pos[0]][pos[1]].Type != '.')
				return false;

			var count = 0;
			foreach (var neighbor in Grid[pos[0]][pos[1]].Neighbors)
				if (Grid[neighbor[0]][neighbor[1]].Type == '.' ||
				    Grid[neighbor[0]][neighbor[1]].Type == '#')
					count++;

			return count <= 1;
		}

		/// <summary>
		/// Shuffles a random List of type T
		/// </summary>
		/// <typeparam name="T">The type of list</typeparam>
		/// <param name="list">The list to shuffle</param>
		private static void Shuffle<T>(this IList<T> list)
		{
			var n = list.Count;
			while (n > 1)
			{
				n--;
				var k = _rnd.Next(0, n + 1);
				var value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		/// <summary>
		/// Searches through a list of int arrays
		/// and removes an array if it matches the
		/// input paramater.
		/// </summary>
		/// <param name="list">The list to modify</param>
		/// <param name="array">The array to remove from the list</param>
		private static void RemoveArray(ref List<int[]> list, int[] array)
		{
			for (var i = 0; i < list.Count; i++)
				if (list[i][0] == array[0] &&
				    list[i][1] == array[1])
				{
					list.RemoveAt(i);
					return;
				}
		}

		/// <summary>
		/// Returns a string representing the entire
		/// dungeon.
		/// </summary>
		/// <returns>The string that represents the entire dungeon</returns>
		public static string DungeonView()
		{
			var s = "";
			for (var i = 0; i < _gridSize; i++)
			{
				for (var j = 0; j < _gridSize; j++)
					s += Grid[i][j].Rep + " ";
				s += "\n";
			}
			return s;
		}

		/// <summary>
		/// Returns a string representing the player's
		/// view in the game.
		/// </summary>
		/// <returns>The string that represents the entire player view</returns>
		public static string PlayerView()
		{
			var s = "";
			for (var i = _pPos[0] - _viewSize; i <= _pPos[0] + _viewSize; i++)
			{
				for (var j = _pPos[1] - _viewSize; j <= _pPos[1] + _viewSize; j++)
				{
					if (i >= 0 && i < _gridSize && j >= 0 && j < _gridSize)
					{
						s += Grid[i][j].Rep + " ";
					}
					else
					{
						s += "  ";
					}
				}
				s += "\n";
			}
			return s;
		}

		#endregion

		// ======================================
		// ============ Generation ==============
		// ======================================

		#region DungeonGeneration

		/// <summary>
		/// Use this for initialization.
		/// Also, generates the dungeon.
		/// </summary>
		public static void Construct()
		{
			Grid = new List<List<Cell>>();
			Rooms = new List<Room>();
			_rnd = new Random();
			_pPos = new int[2] {0, 0};
			GenerateDungeon();
			FindStart();
		}

		/// <summary>
		/// Generates the grid data structure.
		/// Iterates through the two dimensional 
		/// list and creates each cell and initializes
		/// its position. Then generate that cell's
		/// neighboring cell positions.
		/// </summary>
		private static void GenerateGrid()
		{
			for (var i = 0; i < _gridSize; i++)
			{
				Grid.Add(new List<Cell>());
				for (var j = 0; j < _gridSize; j++)
				{
					var c = new Cell();
					c.Construct(_gridSize, i, j);
					c.GenerateNeighbors();
					Grid[i].Add(c);
				}
			}
		}

		/// <summary>
		/// Generates the entire dungeon.
		/// Calls all four parts of the
		/// algorithm.
		/// </summary>
		public static void GenerateDungeon()
		{
			// Instantiate the grid data structure
			GenerateGrid();

			// Generate rooms of random size in the empty maze throughout
			GenerateRooms(1000);

			// Fill the gaps between rooms with random passageways
			// Does not connect the passageways to the rooms
			FillMaze();

			// Connect the random passageways to the rooms
			GenerateCorridors();

			// Remove all dead ends from the maze
			RemoveDeadEnds();
		}

		/// <summary>
		/// First step of GenerateDungeon()
		///	Generate rooms of random size and place
		///	them throughout the grid. No overlapping
		///	allowed. Set amount of room placement 
		///	attempts.
		/// </summary>
		/// <param name="attempts">The amount of room creation attempts</param>
		private static void GenerateRooms(int attempts)
		{
			while (attempts > 0)
			{
				// Generate width and height (3, 7)
				var w = _rnd.Next(3, 7);
				var h = _rnd.Next(3, 7);

				// Generate x and y position (0, gridSize)
				var x = _rnd.Next(0, _gridSize);
				var y = _rnd.Next(0, _gridSize);

				var validRoom = true;

				// Determine if room is valid
				for (var i = y; i < y + h; i++)
					for (var j = x; j < x + w; j++)
						if (!ValidSquare(i, j) || // If it isn't a valid square
						    Grid[i][j].Type != ' ' || // If it isn't empty
						    RoomNeighbor(i, j)) // If its neighbor is part of a room
						{
							attempts--;
							validRoom = false;
							break;
						}

				// Place room if valid
				if (validRoom)
				{
					var r = new Room();
					r.Construct(w, h, new int[2] {y, x});
					r.GenerateEdges();
					Rooms.Add(r);

					for (var i = y; i < y + h; i++)
						for (var j = x; j < x + w; j++)
							Grid[i][j].Type = '#';
				}
			}
		}

		/// <summary>
		/// Second step of GenerateDungeon()
		///	Fill the gaps between rooms with random
		///	passageways. Does not connect passageways
		///	to the rooms.
		/// </summary>
		private static void FillMaze()
		{
			var startx = 0;
			var starty = 0;

			// Find a valid starting location
			while (true)
			{
				startx = _rnd.Next(0, _gridSize - 1);
				starty = _rnd.Next(0, _gridSize - 1);

				if (RoomNeighbor(starty, startx) == false)
					break;
			}

			// ======================================
			// Generate a maze using prim's algorithm
			// ======================================

			/*
				Rerun maze algorithm for each pocket in dungeon
				Without this the maze may be blocked by certain rooms
				Pockets of nothing will fill the dungeon instead of maze
			*/
			while (true)
			{
				var cells = new List<int[]>();
				var count = 0;

				// Select starting cell
				var cc = Grid[starty][startx];
				cells.Add(new int[2] {starty, startx});

				// Add current cell's neighbors to cell list
				cells.AddRange(cc.Neighbors);

				// While we still have walls in our list
				while (cells.Count > 0)
				{
					// Select a random cell from the cell list
					var rcell = _rnd.Next(0, cells.Count - 1);
					cc = Grid[cells[rcell][0]][cells[rcell][1]];

					if (ValidMazeCell(cc) && !RoomNearNeighbor(cc.Y, cc.X))
					{
						Grid[cc.Y][cc.X].Type = '.';
						cells.AddRange(cc.Neighbors);
					}

					var temp = new int[2] {cc.Y, cc.X};
					RemoveArray(ref cells, temp);
				}

				// ============================================
				// ========== End of Maze Algorithm ===========
				// ============================================
				// Search for pockets and repeat maze algorithm

				// Find pocket, save pocket data
				var pocket = FindPockets();
				starty = pocket[0];
				startx = pocket[1];
				count = pocket[2];

				/* 
					Check to see if we have checked every square
					Condition: count == -1 when we can't find a pocket
					If we have then break out of the loop (no pockets)
					Else reset count
				*/
				if (count == -1) return;
				count = 0;
			}
		}

		/// <summary>
		/// Third step of GenerateDungeon()
		/// Connect the random passageways to the rooms.
		/// </summary>
		private static void GenerateCorridors()
		{
			foreach (var r in Rooms)
			{
				Shuffle(r.Edges);

				var connected = false;
				var connectedCount = 0;

				// Iterate through all edges
				foreach (var edge in r.Edges)
				{
					// Try to connect it with a 1 cell away section of maze
					foreach (var n in Grid[edge[0]][edge[1]].Neighbors)
					{
						if (connected) break;

						// If neighbor cell is empty, then it's valid
						if (Grid[n[0]][n[1]].Type != ' ') continue;
						int[] dir = {n[0] - edge[0], n[1] - edge[1]};

						// Check to see if marking it would connect to passageway
						// Make sure connections are not placed next to each other
						if (ValidSquare(n[0] + dir[0], n[1] + dir[1]) &&
						    ValidSquare(n[0] + dir[1], n[1] + dir[0]) &&
						    ValidSquare(n[0] - dir[1], n[1] + dir[0]) &&
						    Grid[n[0] + dir[0]][n[1] + dir[1]].Type == '.' &&
						    Grid[n[0] + dir[1]][n[1] + dir[0]].Type != '.' &&
						    Grid[n[0] - dir[1]][n[1] + dir[0]].Type != '.')
						{
							// Connect room
							Grid[n[0]][n[1]].Type = '.';

							connectedCount++;
							if (connectedCount < 3) continue;
							connected = true;
							break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Fourth step of GenerateDungeon()
		/// Remove all dead ends from the maze
		/// </summary>
		private static void RemoveDeadEnds()
		{
			// Necessary variable to determine looping
			var cannotBreak = true;

			while (cannotBreak)
			{
				cannotBreak = false;

				for (var i = 0; i < _gridSize; i++)
					for (var j = 0; j < _gridSize; j++)

						/*
							If it is a dead end:
							Delete from the maze. We cannot
							break because we must continue 
							until there are no more dead ends
						*/
						if (DeadEnd(new int[2] {i, j}))
						{
							Grid[i][j].Type = ' ';
							cannotBreak = true;
						}
			}
		}

		#endregion

		// ======================================
		// ============ Interaction =============
		// ======================================

		#region DungeonInteraction

		/// <summary>
		///     Finds a random starting coordinate
		///     for the player.
		/// </summary>
		public static void FindStart()
		{
			while (true)
			{
				var y = _rnd.Next(0, _gridSize - 1);
				var x = _rnd.Next(0, _gridSize - 1);

				if (Grid[y][x].Type != '.') continue;
				SetPlayer(y, x);
				return;
			}
		}

		/// <summary>
		///     Sets the player's position in the grid.
		/// </summary>
		/// <param name="y">Initial y position</param>
		/// <param name="x">Initial x position</param>
		public static void SetPlayer(int y, int x)
		{
			_pPos[0] = y;
			_pPos[1] = x;
			Grid[y][x].Rep = '0';
		}

		/// <summary>
		///     Sets the entity's position relative
		///     to it's current position.
		/// </summary>
		/// <param name="e">The entity that is moving</param>
		/// <param name="y">The y modifying value</param>
		/// <param name="x">The x modifying value</param>
		/// <param name="rep">The representation of the entity</param>
		public static void MoveEntity(Entity e, int y, int x, char rep)
		{
			Grid[e.Y][e.X].Rep = Grid[e.Y][e.X].Type;
			e.Y += y;
			e.X += x;
			Grid[e.Y][e.X].Rep = rep;
		}

		/// <summary>
		///     Determines if the location the player
		///     is trying to move to is a valid location.
		/// </summary>
		/// <param name="y">The y position to move to</param>
		/// <param name="x">The x position to move to</param>
		/// <returns>Whether or not the entity can move to that position</returns>
		public static bool CanMove(Entity e, int y, int x)
		{
			var ny = e.Y + y;
			var nx = e.X + x;

			if (ny < 0 || ny >= _gridSize || nx < 0 || nx >= _gridSize) return false;
			return Grid[ny][nx].Rep == '.' || Grid[ny][nx].Rep == '#';
		}

		#endregion
	}
}
