using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.Managers;

namespace QuickBit_Dungeon.Interaction
{
	public class Timer
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		// Timing variables
		private const int FRAME_RATE = 60;
		private int MaxActionTime { get; set; }
		private int ActionTime { get; set; }

		public bool ActionReady { get; private set; } = true;

		// ======================================
		// ============== Methods ===============
		// ======================================

		#region Timer Methods

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="max">The time for the action to reset</param>
		public Timer(int max)
		{
			MaxActionTime = max;
			ActionTime = MaxActionTime;
		}

		/// <summary>
		/// Tells the timer the action was
		/// performed and time should begin.
		/// </summary>
		public void PerformAction()
		{
			ActionReady = false;
			ActionTime = MaxActionTime;
		}

		/// <summary>
		/// Updates the timer and resets the
		/// action to ready at end of time.
		/// </summary>
		public void Update()
		{
			if (!ActionReady) ActionTime--;
			if (ActionTime == 0)
			{
				ActionTime = MaxActionTime;
				ActionReady  = true;
			}
		}

		/// <summary>
		/// Draws the timer at a location on the screen.
		/// </summary>
		/// <param name="sb">The spritebatch to draw with</param>
		/// <param name="x">The x coordinate</param>
		/// <param name="y">The y coordinate</param>
		public void Draw(SpriteBatch sb, int x, int y)
		{
			sb.DrawString(ArtManager.TitleFont, 
						  (ActionTime/FRAME_RATE).ToString(), 
						  new Vector2(x, y), 
						  Color.White);
		}

		#endregion
	}
}
