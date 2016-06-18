using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QuickBit_Dungeon
{
	class ProgressBar
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		
		private Vector2 position;
		private string name;
		private string progressBar;
		private float maxValue = 100;
		private float curValue = 100;
		private int percent;
		const int BAR_SIZE = 20;

		// Properties
		public Vector2 Position { get { return position; } set { position = value; } }
		public float MaxValue {	  get { return maxValue; } set { maxValue = value; } }
		public float CurValue {   get { return curValue; } set { curValue = value; } }

		// ======================================
		// ============== Methods ===============
		// ======================================

		// Constructor
		public ProgressBar(string name)
		{
			this.name = name;
		}

		/*
			Updates the progress bar so that it
			loads and appears to load to the user.
		*/
		public void Update()
		{
			DeterminePercent();
			GenerateProgressBar();
		}

		/*
			Update the values for the progress
			bars.
		*/
		public void UpdateValues(int max, int cur)
		{
			maxValue = max;
			curValue = cur;
		}

		/*
			Determines the percent loaded from
			the max and current values.
		*/
		private void DeterminePercent()
		{
			percent = (int)(curValue/(maxValue/BAR_SIZE));
		}

		/*
			Generates the string that represents
			the progress bar.
		*/
		private void GenerateProgressBar()
		{
			progressBar = name + ": |";

			for (int i = 0; i < percent; i++)
				progressBar += "#";
			for (int j = percent; j < BAR_SIZE; j++)
				progressBar += " ";

			progressBar += "|";
		}

		/*
			Draws the progress bar at the
			initial position that was set.
		*/
		public void DrawProgressBar(SpriteBatch sb)
		{
			sb.DrawString(ArtManager.StatsFont, progressBar, position, Color.White);
		}
	}
}
