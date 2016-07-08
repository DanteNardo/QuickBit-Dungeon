using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickBit_Dungeon.INTERACTION
{
	public class Timer
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		// Timing variables
		private int MaxActionTime { get; set; }
		private int ActionTime { get; set; }

		public bool ActionReady { get; private set; } = true;

		// ======================================
		// ============== Methods ===============
		// ======================================

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
	}
}
