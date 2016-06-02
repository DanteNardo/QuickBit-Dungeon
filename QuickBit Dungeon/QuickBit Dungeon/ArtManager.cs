using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QuickBit_Dungeon
{
	public static class ArtManager
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		
		// Fonts
		static SpriteFont dungeonFont;
		static SpriteFont statsFont;
		static SpriteFont titleFont;

		// Stats box
		static Texture2D statsBoxTex;

		// Lighting effect
		static Texture2D lightTex;

		// ======================================
		// ============ Properties ==============
		// ======================================

		// Fonts
		public static SpriteFont DungeonFont	{ get; set; }
		public static SpriteFont StatsFont		{ get; set; }
		public static SpriteFont TitleFont		{ get; set; }

		// Stats box
		public static Texture2D  StatsBoxTex	{ get; set; }

		// Lighting effect
		public static Texture2D  LightTex		{ get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		/*
			Loads in all game content which is
			stored in this static class.
		*/
		public static void LoadContent(ContentManager cm)
		{
			// Fonts
			DungeonFont = cm.Load<SpriteFont>("DungeonFont");
			StatsFont   = cm.Load<SpriteFont>("StatsFont");
			TitleFont   = cm.Load<SpriteFont>("TitleFont");

			// Stats box
			StatsBoxTex = cm.Load<Texture2D>("StatsBoxTexture");

			// Lighting effect
			LightTex    = cm.Load<Texture2D>("LightTexture");
		}
	}
}
