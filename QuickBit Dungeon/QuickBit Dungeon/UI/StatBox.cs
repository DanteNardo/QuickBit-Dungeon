using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.CORE;
using QuickBit_Dungeon.MANAGERS;

namespace QuickBit_Dungeon.UI
{
	internal class StatBox
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		
		private const int Padding = 40;
		private string _stats;
		
		private Texture2D BoxTex { get; set; }
		private Rectangle BoxRec { get; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Constructor
		/// </summary>
		public StatBox()
		{
			BoxRec = new Rectangle(0, 450, 600, 150);
		}

		/// <summary>
		/// This method passes in and saves the
		/// box's background texture.
		/// </summary>
		public void LoadContent()
		{
			BoxTex = ArtManager.StatsBoxTex;
		}

		/// <summary>
		/// This gives the stats box the current
		/// player's stats.
		/// </summary>
		/// <param name="p">The player to generate stats from</param>
		public void GenerateStats(Player p)
		{
			_stats = "";
			_stats += "Strength:  " + p.Strength;
			_stats += "    Current XP: " + p.Xp + "\n";
			_stats += "Dexterity: " + p.Dexterity;
			_stats += "    XP Needed:  " + p.XpNeeded + "\n";
			_stats += "Wisdom:    " + p.Wisdom;
			_stats += "    Level:      " + p.Level + "\n";
		}

		/// <summary>
		/// Performs the drawing operations for
		/// the stats box.
		/// </summary>
		/// <param name="sb">The spritebatch</param>
		public void DrawStats(SpriteBatch sb)
		{
			sb.Draw(BoxTex, BoxRec, Color.White);
			sb.DrawString(ArtManager.StatsFont, _stats,
				new Vector2(BoxRec.X + Padding, BoxRec.Y + Padding),
				Color.White);
		}
	}
}