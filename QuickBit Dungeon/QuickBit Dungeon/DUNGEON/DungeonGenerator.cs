using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickBit_Dungeon.MANAGERS;

namespace QuickBit_Dungeon.DUNGEON
{
	class DungeonGenerator
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		private const int GridSize = 20;
		private List<List<Cell>> Grid;
		private List<Room> Rooms;
		public Tuple<int, int> Start { get; set; }
		public Tuple<int, int> Exit { get; set; }

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
		private bool RoomNeighbor(int y, int x)
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
		private bool RoomNearNeighbor(int y, int x)
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
		private bool ValidSquare(int y, int x)
		{
			return y >= 0 && x >= 0 && y < GridSize && x < GridSize;
		}

		/// <summary>
		/// Determines if a cell is valid.
		/// (Prim's algorithm conditions)
		/// </summary>
		/// <param name="cell">The cell to check</param>
		/// <returns>Whether it is valid for Prims or not</returns>
		private bool ValidMazeCell(Cell cell)
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
		private int[] FindPockets()
		{
			var count = 0;
			for (var i = 0; i < GridSize; i++)
				for (var j = 0; j < GridSize; j++)
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
		private bool DeadEnd(int[] pos)
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
		private void Shuffle<T>(IList<T> list)
		{
			var n = list.Count;
			while (n > 1)
			{
				n--;
				var k = GameManager.Random.Next(0, n + 1);
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
		private void RemoveArray(ref List<int[]> list, int[] array)
		{
			for (var i = 0; i < list.Count; i++)
				if (list[i][0] == array[0] &&
				    list[i][1] == array[1])
				{
					list.RemoveAt(i);
					return;
				}
		}

		#endregion

		// ======================================
		// ============ Generation ==============
		// ======================================
		
		#region DungeonGeneration

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="grid">The entire dungeon grid to generate</param>
		/// <param name="rooms">The list of each level's rooms</param>
		public DungeonGenerator(List<List<Cell>> grid, List<Room> rooms)
		{
			Grid = grid;
			Rooms = rooms;
		}

		/// <summary>
		/// Generates the entire dungeon.
		/// Calls all six parts of the
		/// algorithm.
		/// </summary>
		public void GenerateDungeon()
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

			// Sets the random starting position
			FindStart();

			// Sets the random ending position
			FindEnd();
		}

		/// <summary>
		/// Generates the grid data structure.
		/// Iterates through the two dimensional 
		/// list and creates each cell and initializes
		/// its position. Then generate that cell's
		/// neighboring cell positions.
		/// </summary>
		private void GenerateGrid()
		{
			for (var i = 0; i < GridSize; i++)
			{
				Grid.Add(new List<Cell>());
				for (var j = 0; j < GridSize; j++)
				{
					var c = new Cell();
					c.Construct(GridSize, i, j);
					c.GenerateNeighbors();
					Grid[i].Add(c);
				}
			}
		}

		/// <summary>
		/// First step of GenerateDungeon()
		///	Generate rooms of random size and place
		///	them throughout the grid. No overlapping
		///	allowed. Set amount of room placement 
		///	attempts.
		/// </summary>
		/// <param name="attempts">The amount of room creation attempts</param>
		private void GenerateRooms(int attempts)
		{
			while (attempts > 0)
			{
				// Generate width and height (3, 5)
				var w = GameManager.Random.Next(3, 5);
				var h = GameManager.Random.Next(3, 5);

				// Generate x and y position (0, gridSize)
				var x = GameManager.Random.Next(0, GridSize);
				var y = GameManager.Random.Next(0, GridSize);

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
						{
							Grid[i][j].Type = '#';
							Grid[i][j].Rep  = '#';
						}
				}
			}
		}

		/// <summary>
		/// Second step of GenerateDungeon()
		///	Fill the gaps between rooms with random
		///	passageways. Does not connect passageways
		///	to the rooms.
		/// </summary>
		private void FillMaze()
		{
			var startx = 0;
			var starty = 0;

			// Find a valid starting location
			while (true)
			{
				startx = GameManager.Random.Next(0, GridSize - 1);
				starty = GameManager.Random.Next(0, GridSize - 1);

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
					var rcell = GameManager.Random.Next(0, cells.Count - 1);
					cc = Grid[cells[rcell][0]][cells[rcell][1]];

					if (ValidMazeCell(cc) && !RoomNearNeighbor(cc.Y, cc.X))
					{
						Grid[cc.Y][cc.X].Type = '.';
						Grid[cc.Y][cc.X].Rep  = '.';
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
		private void GenerateCorridors()
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
							Grid[n[0]][n[1]].Rep  = '.';

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
		private void RemoveDeadEnds()
		{
			// Necessary variable to determine looping
			var cannotBreak = true;

			while (cannotBreak)
			{
				cannotBreak = false;

				for (var i = 0; i < GridSize; i++)
					for (var j = 0; j < GridSize; j++)

						/*
							If it is a dead end:
							Delete from the maze. We cannot
							break because we must continue 
							until there are no more dead ends
						*/
						if (DeadEnd(new int[2] {i, j}))
						{
							Grid[i][j].Type = ' ';
							Grid[i][j].Rep  = ' ';
							cannotBreak = true;
						}
			}
		}

		/// <summary>
		/// Fifth step of GenerateDungeon()
		/// Finds a random starting coordinate
		/// for the player.
		/// </summary>
		private void FindStart()
		{
			while (true)
			{
				var y = GameManager.Random.Next(0, GridSize - 1);
				var x = GameManager.Random.Next(0, GridSize - 1);

				if (Grid[y][x].Type == '.')
				{
					SetStart(y, x);
					return;
				}
			}
		}

		/// <summary>
		/// Sets the start position in the grid.
		/// </summary>
		/// <param name="y">Starting y position</param>
		/// <param name="x">Starting x position</param>
		private void SetStart(int y, int x)
		{
			Start = new Tuple<int, int>(y, x);
		}

		/// <summary>
		/// Sixth step of GenerateDungeon()
		/// Finds a random ending coordinate
		/// for the level.
		/// </summary>
		public void FindEnd()
		{
			while (true)
			{
				var y = GameManager.Random.Next(0, GridSize - 1);
				var x = GameManager.Random.Next(0, GridSize - 1);

				if (Grid[y][x].Type == '#')
				{
					SetEnd(y, x);
					return;
				}
			}
		}

		/// <summary>
		/// Sets the exit position in the grid.
		/// </summary>
		/// <param name="y">Final y position</param>
		/// <param name="x">Final x position</param>
		public void SetEnd(int y, int x)
		{
			Exit = new Tuple<int, int>(y, x);
			Grid[y][x].Type = '@';
			Grid[y][x].Rep = '@';
		}

		#endregion
	}
}
