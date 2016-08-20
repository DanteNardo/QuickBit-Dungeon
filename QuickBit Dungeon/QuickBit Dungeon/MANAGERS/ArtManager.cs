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
		public static SpriteFont HudFont { get; set; }
		public static SpriteFont TitleFont { get; set; }

		// Stats box
		public static Texture2D StatsBoxTex { get; set; }

		// Lighting effect
		public static Texture2D LightTex { get; set; }

		// Menu Backgrounds
		public static Texture2D MainMenuBackground { get; set; }
		public static Texture2D HowToMenuBackground { get; set; }
		public static Texture2D PauseMenuBackground { get; set; }
		public static Texture2D LevelUpMenuBackground { get; set; }
		public static Texture2D GameOverBackground { get; set; }

        // How To Menu Slides
		public static Texture2D HowToMenuSlide01 { get; set; }
		public static Texture2D HowToMenuSlide02 { get; set; }
		public static Texture2D HowToMenuSlide03 { get; set; }
		public static Texture2D HowToMenuSlide04 { get; set; }
		public static Texture2D HowToMenuSlide05 { get; set; }
		public static Texture2D HowToMenuSlide06 { get; set; }
		public static Texture2D HowToMenuSlide07 { get; set; }

		// Menu MenuButtons
		public static Texture2D MainMenuButton { get; set; }
		public static Texture2D StartButton { get; set; }
		public static Texture2D HowToButton { get; set; }
		public static Texture2D ReturnButton { get; set; }
		public static Texture2D ResumeButton { get; set; }
		public static Texture2D NextButton { get; set; }
		public static Texture2D LastButton { get; set; }
		public static Texture2D RedAuraButton { get; set; }
		public static Texture2D GreenAuraButton { get; set; }
		public static Texture2D BlueAuraButton { get; set; }
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
			HudFont = cm.Load<SpriteFont>("Fonts/HudFont");
			TitleFont = cm.Load<SpriteFont>("Fonts/TitleFont");

			// Stats box
			StatsBoxTex = cm.Load<Texture2D>("UI/StatsBoxTexture");

			// Lighting effect
			LightTex = cm.Load<Texture2D>("UI/LightTexture");

			// Menu Backgrounds
			MainMenuBackground = cm.Load<Texture2D>("UI/MainMenuTexture");
			HowToMenuBackground = cm.Load<Texture2D>("UI/HowToMenuTexture");
			PauseMenuBackground = cm.Load<Texture2D>("UI/PauseMenuTexture");
			LevelUpMenuBackground = cm.Load<Texture2D>("UI/LevelUpMenuTexture");
			GameOverBackground = cm.Load<Texture2D>("UI/GameOverTexture");

            // How To Menu Slides
			HowToMenuSlide01 = cm.Load<Texture2D>("UI/HowToMenu/HowToMenuTexture01");
			HowToMenuSlide02 = cm.Load<Texture2D>("UI/HowToMenu/HowToMenuTexture02");
			HowToMenuSlide03 = cm.Load<Texture2D>("UI/HowToMenu/HowToMenuTexture03");
			HowToMenuSlide04 = cm.Load<Texture2D>("UI/HowToMenu/HowToMenuTexture04");
			HowToMenuSlide05 = cm.Load<Texture2D>("UI/HowToMenu/HowToMenuTexture05");
			HowToMenuSlide06 = cm.Load<Texture2D>("UI/HowToMenu/HowToMenuTexture06");
			HowToMenuSlide07 = cm.Load<Texture2D>("UI/HowToMenu/HowToMenuTexture07");

			// Menu MenuButtons
			MainMenuButton = cm.Load<Texture2D>("Buttons/MainMenuButtonTexture");
			StartButton = cm.Load<Texture2D>("Buttons/StartButtonTexture");
			HowToButton = cm.Load<Texture2D>("Buttons/HowToButtonTexture");
			ReturnButton = cm.Load<Texture2D>("Buttons/ReturnButtonTexture");
			ResumeButton = cm.Load<Texture2D>("Buttons/ResumeButtonTexture");
			NextButton = cm.Load<Texture2D>("Buttons/NextButtonTexture");
			LastButton = cm.Load<Texture2D>("Buttons/LastButtonTexture");
			RedAuraButton = cm.Load<Texture2D>("Buttons/RedAuraButtonTexture");
			GreenAuraButton = cm.Load<Texture2D>("Buttons/GreenAuraButtonTexture");
			BlueAuraButton = cm.Load<Texture2D>("Buttons/BlueAuraButtonTexture");
			ExitButton = cm.Load<Texture2D>("Buttons/ExitButtonTexture");
		}
	}
}