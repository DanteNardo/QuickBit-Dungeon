using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.MANAGERS;

namespace QuickBit_Dungeon.UI.EFFECTS
{
	public class Light : IFlicker
	{
		// ======================================
		// ============== Members ===============
		// ======================================

        public int Y { get; private set; }
        public int X { get; private set; }
	    public int LightSize { get; set; } = 5;
	    private double LightAlpha { get; set; } = 1;
        public List<List<double>> LightData { get; set; }

        // For the light flicker effect
	    public double FlickerMin { get; set; } = 0.25;
	    public double FlickerFraction { get; set; } = .15;
        public double FlickerStart { get; set; }
        public double FlickerCurrent { get; set; }
        public double FlickerNext { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

        /// <summary>
        /// Constructor
        /// </summary>
	    public Light(int y, int x, int size)
        {
            Y = y;
            X = x;
            LightSize = size;
	        LightData = new List<List<double>>();
            GenerateLightStructure();
            BeginFlicker();
	    }

        /// <summary>
        /// Updates the light object.
        /// Flickers the light.
        /// </summary>
	    public void Update()
	    {
	        Flicker();
	        SetUpLightValues();
	    }

		/// <summary>
		/// Updates the light object.
		/// Flickers the light and moves
		/// the light based on input.
		/// </summary>
		/// <param name="y">The new y position</param>
		/// <param name="x">The new x position</param>
		public void Update(int y, int x)
		{
			Y = y;
			X = x;
			Flicker();
	        SetUpLightValues();
		}

		/// <summary>
		/// Creates the light data structure
		/// with empty values and then calls
		/// SetUpLightValues() to complete
		/// the data structure.
		/// </summary>
	    public void GenerateLightStructure()
	    {
		    for (var i = 0; i < LightSize; i++)
		    {
			    LightData.Add(new List<double>());
			    for (var j = 0; j < LightSize; j++)
				    LightData[i].Add(0);
		    }

		    SetUpLightValues();
	    }

		/// <summary>
		/// Incrementally create the light data
		/// structure values by creating diagonals
		/// and incrementing closer and closer
		/// (and brighter) towards the center.
		/// </summary>
		private void SetUpLightValues()
		{
			int minY = 0;
			int minX = 0;
			int maxY = LightSize-1;
			int maxX = LightSize-1;
			int half = (LightSize/2);
			int curX = half;
			int curY = minY;

			double currentLight = 0;

			while (true)
			{
				// Southeast diagnoal
				while (curX != maxX)
					LightData[curY++][curX++] = currentLight;

				// Southwest diagonal
				while (curX != half)
					LightData[curY++][curX--] = currentLight;

				// Northwest diagonal
				while (curX != minX)
					LightData[curY--][curX--] = currentLight;

				// Northeast diagonal
				while (curX != half)
					LightData[curY--][curX++] = currentLight;

				// Increment values
				minY++; minX++;
				maxY--; maxX--;
				curX = half;
				curY = minY;
				currentLight += (1/(double)(LightSize/2))*LightAlpha;

				if (curX == half & curY == half)
				{
					LightData[half][half] = 1;
					return;
				}
			}
		}
        
	    #region Flickering

	    /// <summary>
	    /// Initializes the flicker effect.
	    /// </summary>
	    public void BeginFlicker()
	    {
	        FlickerStart = LightAlpha;
	        FlickerCurrent = FlickerStart;
	        FlickerNext = FlickerStart;
	    }

	    /// <summary>
	    /// Continues interpolating towards a value
	    /// or starts a new flicker.
	    /// </summary>
	    public void Flicker()
	    {
	        if (Flickering())
	            FlickerCurrent = Interpolate();
	        else
	        {
	            FlickerNext = GameManager.Random.NextDouble();
		        if ((FlickerNext += .5) > 1)
					FlickerNext = 1;
	        }

	        SetFlick();
	    }

	    /// <summary>
	    /// Determines if we are still flickering
	    /// or if we have reached the next flicker value.
	    /// </summary>
	    /// <returns>Whether we are flickering or not</returns>
	    public bool Flickering()
	    {
	        return Math.Abs(FlickerCurrent - FlickerNext) > 0.01;
	    }

	    /// <summary>
	    /// Interpolates between two values
	    /// to transition from one value to another.
	    /// </summary>
	    /// <returns>The gradual change in value</returns>
	    public double Interpolate()
	    {
	        return FlickerCurrent + (FlickerNext - FlickerCurrent)*FlickerFraction;
	    }

	    /// <summary>
	    /// Sets the data we have been flickering
	    /// to our current flicker state. This method
	    /// is necessary since we want to use flicker
	    /// properties for readability.
	    /// </summary>
	    public void SetFlick()
	    {
	        LightAlpha = FlickerCurrent;
	    }

	    #endregion

	}
}