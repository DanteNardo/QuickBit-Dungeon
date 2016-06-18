using System;
using System.Collections;
using System.Collections.Generic;

public class Cell {

	// ======================================
	// ============= Variables ==============
	// ======================================

	private char type;				// Represents the type of cell
	private char rep;				// The current visual representation
	private int size;				// The size of the dungeon
	private int x;					// The x coordinate in dungeon
	private int y;					// The y coordinate in dungeon
	private List<int[]> neighbors;	// This cell's neighboring locations

	// Properties
	public char Type			 { get { return type; } set { type = value; rep = value; } }
	public char Rep				 { get { return rep; }  set { rep  = value; } }
	public int X				 { get { return x; } }
	public int Y				 { get { return y; } }
	public List<int[]> Neighbors { get { return neighbors; } }


	// ======================================
	// ============== Methods ===============
	// ======================================

	// Constructor
	public void Construct(int size, int y, int x)
	{
		// Initialize variables
		type      = ' ';
		rep	      = type;
		this.size = size;
		this.x    = x;
		this.y    = y;
		neighbors = new List<int[]>();
	}

	// Generates a list of int arrays
	// The arrays correspond to x,y cooridnates
	public void GenerateNeighbors()
	{
		if (x+1 < size)
			neighbors.Add(new int[2] { y, x+1 });
		if (x-1 >= 0)
			neighbors.Add(new int[2] { y, x-1 });
		if (y+1 < size)
			neighbors.Add(new int[2] { y+1, x });
		if (y-1 >= 0)
			neighbors.Add(new int[2] { y-1, x });
	}
}
