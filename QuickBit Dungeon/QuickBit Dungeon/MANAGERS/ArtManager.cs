using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace QuickBit_Dungeon.MANAGERS
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
		public static Texture2D HowToMenuBackground { get; set; }
		public static Texture2D PauseMenuBackground { get; set; }
		public static Texture2D GameOverBackground { get; set; }

		// Menu MenuButtons
		public static Texture2D MainMenuButton { get; set; }
		public static Texture2D StartButton { get; set; }
		public static Texture2D HowToButton { get; set; }
		public static Texture2D ReturnButton { get; set; }
		public static Texture2D ResumeButton { get; set; }
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
			HowToMenuBackground = cm.Load<Texture2D>("UI/HowToMenuTexture");
			PauseMenuBackground = cm.Load<Texture2D>("UI/PauseMenuTexture");
			GameOverBackground = cm.Load<Texture2D>("UI/GameOverTexture");

			// Menu MenuButtons
			MainMenuButton = cm.Load<Texture2D>("Buttons/MainMenuButtonTexture");
			StartButton = cm.Load<Texture2D>("Buttons/StartButtonTexture");
			HowToButton = cm.Load<Texture2D>("Buttons/HowToButtonTexture");
			ReturnButton = cm.Load<Texture2D>("Buttons/ReturnButtonTexture");
			ResumeButton = cm.Load<Texture2D>("Buttons/ResumeButtonTexture");
			ExitButton = cm.Load<Texture2D>("Buttons/ExitButtonTexture");
		}
	}
}