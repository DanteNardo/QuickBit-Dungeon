using System;
using System.Collections;
using System.Collections.Generic;

public static class Dungeon {

	// ======================================
	// ============= Variables ==============
	// ======================================
	
	private static int gridSize = 50;
	private static int[] pPos;
	private static int viewSize = 5;
	private static List<List<Cell>> grid;
	private static List<Room> rooms;
	private static Random rnd;

	public static int PlayerY
	{
		get { return pPos[0]; }
	}
	public static int PlayerX
	{
		get { return pPos[1]; }
	}

	// ======================================
	// ============= Utilities ==============
	// ======================================

	#region Utilities
	/*
		Checks to see if a given cell's neighbors 
		have neighboring cells that are already 
		part of rooms. If so, return true since this
		cell is neighboring a room
	*/
	private static bool RoomNeighbor(int y, int x)
	{
		foreach (var n in grid[y][x].Neighbors)
			foreach (var location in grid[n[0]][n[1]].Neighbors)
				if (grid[location[0]][location[1]].Type == '#')
					return true;
		return false;
	}

	/*
		Checks to see if a given cell's neighbors
		are already part of rooms. If so, return
		true since this cell is near neighboring
		a room.
	*/
	private static bool RoomNearNeighbor(int y, int x)
	{
		foreach (var n in grid[y][x].Neighbors)
			if (grid[n[0]][n[1]].Type == '#')
				return true;
		return false;
	}

	/*
		Determines if a square's board position
		is a valid square. If so, return true.
	*/
	private static bool ValidSquare(int y, int x)
	{
		if ((y < 0 || x < 0) || (y >= gridSize || x >= gridSize))
			return false;
		return true;
	}

	/*
		Determines if a cell is valid.
		(Prim's algorithm conditions)
	*/
	private static bool ValidMazeCell(Cell cell)
	{
		if (cell.Type == '.')
			return false;

		int count = 0;
		foreach (var neighbor in cell.Neighbors)
			if (grid[neighbor[0]][neighbor[1]].Type == '.')
				count += 1;

		// Valid cells have less than two open neighbors
		if (count < 2) return true;
		else return false;
	}

	/*
		Finds the x and y location of pockets.
		Also returns the amount of iterations
		it took to find the pocket.
	*/
	private static int[] FindPockets()
	{
		int count = 0;
		for (int i = 0; i < gridSize; i++)
			for (int j = 0; j < gridSize; j++)
			{
				if (ValidMazeCell(grid[i][j]) &&
					RoomNearNeighbor(i, j) == false)

					// Select new starting coordinates
					return new int[3] { i, j, count };

				count++;	
			}

		// Default return
		return new int[3] { 0, 0, -1 };
	}

	/*
		Returns true if position is a dead
		end. A cell having one neighbor or
		less means it is a dead end.
	*/
	private static bool DeadEnd(int[] pos)
	{
		if (grid[pos[0]][pos[1]].Type != '.')
			return false;

		int count = 0;
		foreach (var neighbor in grid[pos[0]][pos[1]].Neighbors)
			if (grid[neighbor[0]][neighbor[1]].Type == '.' ||
				grid[neighbor[0]][neighbor[1]].Type == '#')
				count++;

		if (count <= 1) return true;
		else return false;
	}

	/*
		Shuffles a random List of type T
	*/
	private static void Shuffle<T>(this IList<T> list)
	{  
		int n = list.Count;  
		while (n > 1)
		{  
			n--;  
			int k = rnd.Next(0, n+1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	/*
		Searches through a list of int arrays
		and removes an array if it matches the
		input paramater.
	*/
	private static void RemoveArray(ref List<int[]> list, int[] array)
	{
		for (int i = 0; i < list.Count; i++)
			if (list[i][0] == array[0] &&
				list[i][1] == array[1])
			{
				list.RemoveAt(i);
				return;
			}
	}

	/*
		Returns a string representing the entire
		dungeon.
	*/
	public static string DungeonView()
	{
		string s = "";
		for (int i = 0; i < gridSize; i++)
		{
			for (int j = 0; j < gridSize; j++)
				s += grid[i][j].Rep + " ";
			s += "\n";
		}
		return s;
	}

	/*
		Returns a string representing the player's
		view in the game.
	*/
	public static string PlayerView()
	{
		string s = "";
		for (int i = pPos[0]-viewSize; i <= pPos[0]+viewSize; i++)
		{
			for (int j = pPos[1]-viewSize; j <= pPos[1]+viewSize; j++)
			{
				if (i >= 0 && i < gridSize && j >= 0 && j < gridSize)
				{
					s += grid[i][j].Rep + " ";
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

	/*
		Use this for initialization.
		Also, generates the dungeon.
	*/
	public static void Construct ()
	{
		grid      = new List<List<Cell>>();
		rooms     = new List<Room>();
		rnd       = new Random();
		pPos = new int[2] { 0, 0 };
		GenerateDungeon();
		FindStart();
	}

	/*
		Generates the grid data structure.
		Iterates through the two dimensional 
		list and creates each cell and initializes
		its position. Then generate that cell's
		neighboring cell positions.
	*/
	private static void GenerateGrid()
	{
		for (int i = 0; i < gridSize; i++)
		{
			grid.Add(new List<Cell>());
			for (int j = 0; j < gridSize; j++)
			{
				Cell c = new Cell();
				c.Construct(gridSize, i, j);
				c.GenerateNeighbors();
				grid[i].Add(c);
			}
		}
	}

	/*
		Generates the entire dungeon.
		Calls all four parts of the
		algorithm.
	*/
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

	/*
		First step of GenerateDungeon()
		Generate rooms of random size and place
		them throughout the grid. No overlapping
		allowed. Set amount of room placement 
		attempts.
	*/
	private static void GenerateRooms(int attempts)
	{
		while (attempts > 0)
		{
			// Generate width and height (3, 7)
			int w = rnd.Next(3, 7);
			int h = rnd.Next(3, 7);

			// Generate x and y position (0, gridSize)
			int x = rnd.Next(0, gridSize);
			int y = rnd.Next(0, gridSize);

			bool validRoom = true;

			// Determine if room is valid
			for (int i = y; i < y+h; i++)
				for (int j = x; j < x+w; j++)
					if (!ValidSquare(i, j) ||		// If it isn't a valid square
						grid[i][j].Type != ' ' ||	// If it isn't empty
						RoomNeighbor(i, j))			// If its neighbor is part of a room
					{
						attempts--;
						validRoom = false;
						break;
					}

			// Place room if valid
			if (validRoom)
			{
				Room r = new Room();
				r.Construct(w, h, new int[2] { y, x });
				r.GenerateEdges();
				rooms.Add(r);

				for (int i = y; i < y+h; i++)
					for (int j = x; j < x+w; j++)
						grid[i][j].Type = '#';
			}
		}
	}

	/*
		Second step of GenerateDungeon()
		Fill the gaps between rooms with random
		passageways. Does not connect passageways
		to the rooms.
	*/
	private static void FillMaze()
	{
		int startx = 0;
		int starty = 0;

		// Find a valid starting location
		while (true)
		{
			startx = rnd.Next(0, gridSize-1);
			starty = rnd.Next(0, gridSize-1);

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
			List<int[]> cells = new List<int[]>();
			int count = 0;

			// Select starting cell
			Cell cc = grid[starty][startx];
			cells.Add(new int[2] { starty, startx });

			// Add current cell's neighbors to cell list
			foreach (var cell in cc.Neighbors)
				cells.Add(cell);

			// While we still have walls in our list
			while (cells.Count > 0)
			{
				// Select a random cell from the cell list
				int rcell = rnd.Next(0, cells.Count-1);
				cc = grid[cells[rcell][0]][cells[rcell][1]];

				if (ValidMazeCell(cc) && !RoomNearNeighbor(cc.Y, cc.X))
				{
					grid[cc.Y][cc.X].Type = '.';
					foreach (var cell in cc.Neighbors)
						cells.Add(cell);
				}

				int[] temp = new int[2] { cc.Y, cc.X };
				RemoveArray(ref cells, temp);
			}

			// ============================================
			// ========== End of Maze Algorithm ===========
			// ============================================
			// Search for pockets and repeat maze algorithm
			
			// Find pocket, save pocket data
			int[] pocket = FindPockets();
			starty = pocket[0];
			startx = pocket[1];
			count  = pocket[2];

			/* 
				Check to see if we have checked every square
				Condition: count == -1 when we can't find a pocket
				If we have then break out of the loop (no pockets)
				Else reset count
			*/
			if (count == -1) return;
			else count = 0;
		}
	}

	/*
		Third step of GenerateDungeon()
		Connect the random passageways to the rooms.
	*/
	private static void GenerateCorridors()
	{
		for (int i = 0; i < rooms.Count; i++)
		{
			// Select room and shuffle it's edges
			Room r = rooms[i];
			Shuffle(r.Edges);

			bool connected = false;
			int connectedCount = 0;

			// Iterate through all edges
			foreach (var edge in r.Edges)
			{
				// Try to connect it with a 1 cell away section of maze
				foreach (var n in grid[edge[0]][edge[1]].Neighbors)
				{
					if (connected) break;

					// If neighbor cell is empty, then it's valid
					if (grid[n[0]][n[1]].Type == ' ')
					{
						int[] dir = { n[0]-edge[0], n[1]-edge[1] };

						// Check to see if marking it would connect to passageway
						// Make sure connections are not placed next to each other
						if (ValidSquare(n[0]+dir[0], n[1]+dir[1]) && 
							ValidSquare(n[0]+dir[1], n[1]+dir[0]) &&
							ValidSquare(n[0]-dir[1], n[1]+dir[0]) &&
							grid[n[0]+dir[0]][n[1]+dir[1]].Type == '.' &&
							grid[n[0]+dir[1]][n[1]+dir[0]].Type != '.' &&
							grid[n[0]-dir[1]][n[1]+dir[0]].Type != '.')
						{
							// Connect room
							grid[n[0]][n[1]].Type = '.';

							connectedCount++;
							if (connectedCount >= 3)
							{
								connected = true;
								break;
							}
						}
					}
				}
			}
		}
	}

	/* 
		Fourth step of GenerateDungeon()
		Remove all dead ends from the maze
	*/
	private static void RemoveDeadEnds()
	{
		// Necessary variable to determine looping
		bool cannotBreak = true;

		while (cannotBreak)
		{
			cannotBreak = false;

			for (int i = 0; i < gridSize; i++)
				for (int j = 0; j < gridSize; j++)
					/*
						If it is a dead end:
						Delete from the maze. We cannot
						break because we must continue 
						until there are no more dead ends
					*/
					if (DeadEnd(new int[2] { i, j }))
					{
						grid[i][j].Type = ' ';
						cannotBreak = true;
					}
		}
	}

	#endregion

	// ======================================
	// ============ Interaction =============
	// ======================================

	#region DungeonInteraction
	/*
		Finds a random starting coordinate 
		for the player.
	*/
	public static void FindStart()
	{
		while (true)
		{
			int y = rnd.Next(0, gridSize-1);
			int x = rnd.Next(0, gridSize-1);

			if (grid[y][x].Type == '.')
			{
				SetPlayer(y, x);
				return;
			}
		}
	}

	/*
		Sets the player's position in the grid.
	*/
	public static void SetPlayer(int y, int x)
	{
		pPos[0] = y;
		pPos[1] = x;
		grid[y][x].Rep = '0';
	}

	/*
		Sets the player's position relative
		to it's current position.
	*/
	public static void MovePlayer(int y, int x)
	{
		grid[pPos[0]][pPos[1]].Rep = grid[pPos[0]][pPos[1]].Type;
		pPos[0] += y;
		pPos[1] += x;
		grid[pPos[0]][pPos[1]].Rep = '0';
	}

	/*
		Determines if the location the player
		is trying to move to is a valid location.
	*/
	public static bool CanMove(int y, int x)
	{
		int ny = (int)(pPos[0] + y);
		int nx = (int)(pPos[1] + x);

		if (ny >= 0 && ny < gridSize &&
			nx >= 0 && nx < gridSize)
		{
			if (grid[ny][nx].Type == ' ')
				return false;
			else
				return true;
		}
		else return false;
	}

	#endregion
}
