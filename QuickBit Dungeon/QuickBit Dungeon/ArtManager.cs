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

		// Menu Backgrounds
		public static Texture2D MainMenuBackground { get; set; }

		// Menu Buttons
		public static Texture2D StartButton { get; set; }
		public static Texture2D HowToButton { get; set; }
		public static Texture2D ExitButton { get; set; }

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
			DungeonFont = cm.Load<SpriteFont>("Fonts/DungeonFont");
			StatsFont = cm.Load<SpriteFont>("Fonts/StatsFont");
			TitleFont = cm.Load<SpriteFont>("Fonts/TitleFont");

			// Stats box
			StatsBoxTex = cm.Load<Texture2D>("UI/StatsBoxTexture");

			// Lighting effect
			LightTex = cm.Load<Texture2D>("UI/LightTexture");

			// Menu Backgrounds
			MainMenuBackground = cm.Load<Texture2D>("UI/MainMenuTexture");

			// Menu Buttons
			StartButton = cm.Load<Texture2D>("Buttons/StartButtonTexture");
			HowToButton = cm.Load<Texture2D>("Buttons/HowToButtonTexture");
			ExitButton = cm.Load<Texture2D>("Buttons/ExitButtonTexture");
		}
	}
}