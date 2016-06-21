using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuickBit_Dungeon.UI
{
	internal class ProgressBar
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		
		private const int BarSize = 20;
		private readonly string _name;
		private int _percent;
		private string _progressBar;

		public Vector2 Position { get; set; }
		public float MaxValue { get; set; } = 100;
		public float CurValue { get; set; } = 100;

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">Name of the bar</param>
		public ProgressBar(string name)
		{
			_name = name;
		}

		/// <summary>
		/// Updates the progress bar so that it
		///	loads and appears to load to the user.
		/// </summary>
		public void Update()
		{
			DeterminePercent();
			GenerateProgressBar();
		}

		/// <summary>
		/// Update the values for the progress
		///	bars.
		/// </summary>
		/// <param name="max">The max value of the data source</param>
		/// <param name="cur">The current value of the data source</param>
		public void UpdateValues(int max, int cur)
		{
			MaxValue = max;
			CurValue = cur;
		}

		/// <summary>
		/// Determines the percent loaded from
		/// the max and current values.
		/// </summary>
		private void DeterminePercent()
		{
			_percent = (int) (CurValue/(MaxValue/BarSize));
		}

		/// <summary>
		/// Generates the string that represents
		/// the progress bar.
		/// </summary>
		private void GenerateProgressBar()
		{
			_progressBar = _name + ": |";

			for (var i = 0; i < _percent; i++)
				_progressBar += "#";
			for (var j = _percent; j < BarSize; j++)
				_progressBar += " ";

			_progressBar += "|";
		}

		/// <summary>
		/// Draws the progress bar at the
		/// initial position that was set.
		/// </summary>
		/// <param name="sb">The spritebatch</param>
		public void DrawProgressBar(SpriteBatch sb)
		{
			sb.DrawString(ArtManager.StatsFont, _progressBar, Position, Color.White);
		}
	}
}