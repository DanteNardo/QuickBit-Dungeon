using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace QuickBit_Dungeon
{
	public static class ArtManager
	{
		// ======================================
		// ============== Members ===============
		// ======================================

		// Fonts
		public static SpriteFont DungeonFont { get; set; }
		public static SpriteFont StatsFont { get; set; }
		public static SpriteFont TitleFont { get; set; }

		// Stats box
		public static Texture2D StatsBoxTex { get; set; }

		// Lighting effect
		public static Texture2D LightTex { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Loads in all game content which is
		/// stored in this static class.
		/// </summary>
		/// <param name="cm">The content manager</param>
		public static void LoadContent(ContentManager cm)
		{
			// Fonts
			DungeonFont = cm.Load<SpriteFont>("DungeonFont");
			StatsFont = cm.Load<SpriteFont>("StatsFont");
			TitleFont = cm.Load<SpriteFont>("TitleFont");

			// Stats box
			StatsBoxTex = cm.Load<Texture2D>("StatsBoxTexture");

			// Lighting effect
			LightTex = cm.Load<Texture2D>("LightTexture");
		}
	}
}