using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QuickBit_Dungeon
{
	class StatBox
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		
		Texture2D boxTex;
		Rectangle boxRec;
		string stats;
		const int PADDING = 10;

		// Properties
		Texture2D BoxTex { get { return boxTex; } set { boxTex = value; } }
		Rectangle BoxRec { get { return boxRec; } set { boxRec = value; } }

		// ======================================
		// ============== Methods ===============
		// ======================================

		// Constructor
		public StatBox()
		{
			boxRec = new Rectangle(0, 450, 600, 150);
		}

		/*
			This method passes in and saves the
			box's background texture.
		*/
		public void LoadContent()
		{
			boxTex = ArtManager.StatsBoxTex;
		}

		/*
			This gives the stats box the current
			player's stats.
		*/
		public void GenerateStats(Player p)
		{
			stats += "Strength: "  + p.Strength  + "\n";
			stats += "Dexterity: " + p.Dexterity + "\n";
			stats += "Wisdom: "    + p.Wisdom    + "\n";
		}

		/*
			Performs the drawing operations for
			the stats box.
		*/
		public void DrawStats(SpriteBatch sb)
		{
			sb.Draw(boxTex, boxRec, Color.White);
			sb.DrawString(ArtManager.DungeonFont, stats, 
						  new Vector2(boxRec.X+PADDING, boxRec.Y+PADDING), 
						  Color.White);
		}
	}
}
