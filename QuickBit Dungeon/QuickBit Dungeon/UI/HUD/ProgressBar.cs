using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.Managers;

namespace QuickBit_Dungeon.UI
{
	internal class ProgressBar
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		
		private const int BarSize = 20;
		private readonly string m_name;
		private int m_percent;
		private string m_progressBar;

		public Vector2 Position { get; set; }
		public float MaxValue { get; set; } = 100;
		public float CurValue { get; set; } = 100;

		// ======================================
		// ============== Methods ===============
		// ======================================

		#region Progress Bar Methods

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">Name of the bar</param>
		public ProgressBar(string name)
		{
			m_name = name;
		}

		/// <summary>
		/// Initializes all progress bar data.
		/// </summary>
		/// <param name="max">The max value of the data source</param>
		/// <param name="cur">The current value of the data source</param>
		public void Init(int max, int cur)
		{
			MaxValue = max;
			CurValue = cur;
			DeterminePercent();
			GenerateProgressBar();
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
			m_percent = (int) (CurValue/(MaxValue/BarSize));
		}

		/// <summary>
		/// Generates the string that represents
		/// the progress bar.
		/// </summary>
		private void GenerateProgressBar()
		{
			m_progressBar = m_name + ": |";

			for (var i = 0; i < m_percent; i++)
				m_progressBar += "#";
			for (var j = m_percent; j < BarSize; j++)
				m_progressBar += " ";

			m_progressBar += "|";
		}

		/// <summary>
		/// Draws the progress bar at the
		/// initial position that was set.
		/// </summary>
		/// <param name="sb">The spritebatch</param>
		public void DrawProgressBar(SpriteBatch sb)
		{
			sb.DrawString(ArtManager.HudFont, m_progressBar, Position, Color.White);
		}

		#endregion
	}
}