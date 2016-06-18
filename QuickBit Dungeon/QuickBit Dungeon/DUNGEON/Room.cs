using System;
using System.Collections;
using System.Collections.Generic;

public class Room {

	// ======================================
	// ============= Variables ==============
	// ======================================

	private int width;
	private int height;
	private List<int[]> edges;
	private int[] position;

	// Properties
	public int Width		 { get { return width; }  set { width  = value; } }
	public int Height		 { get { return height; } set { height = value; } }
	public List<int[]> Edges { get { return edges; } }
	public int[] Position	 { get { return position; } }


	// ======================================
	// ============== Methods ===============
	// ======================================

	// Constructor
	public void Construct(int width, int height, int[] position)
	{
		// Initialize variables
		this.width    = width;
		this.height   = height;
		this.position = position;
		edges         = new List<int[]>();
	}

	// Generates a list of int arrays
	// The arrays correspond to x,y cooridnates
	public void GenerateEdges()
	{
		// Top and Bottom row
		for (int i = 0; i < width; i++)
		{
			edges.Add(new int[2] { position[0], position[1]+i });
			edges.Add(new int[2] { position[0]+height-1, position[1]+i });
		}

		// Left and Right side
		for (int i = 0; i < width; i++)
		{
			edges.Add(new int[2] { position[0], position[1]+i });
			edges.Add(new int[2] { position[0]+height-1, position[1]+i });
		}
	}
}
