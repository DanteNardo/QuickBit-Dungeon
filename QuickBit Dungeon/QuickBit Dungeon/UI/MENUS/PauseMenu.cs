using System.Collections.Generic;
using Microsoft.Xna.Framework;
using QuickBit_Dungeon.MANAGERS;

namespace QuickBit_Dungeon.UI.MENUS
{
	internal class PauseMenu : Menu
	{
		// ======================================
		// ============== Members ===============
		// ======================================	

		private const int NumberOfButtons = 4;
		private const int ResumeIndex = 0;
		private const int HowToIndex = 1;
		private const int MainMenuIndex = 2;
		private const int ExitIndex = 3;

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Pause Menu constructor.
		/// </summary>
		public PauseMenu() : base("vertical")
		{
			MaxId = NumberOfButtons;
			MenuButtons = new List<Button>();
			MakeButtons();
		}

		/// <summary>
		/// Generates all of the buttons for the
		/// pause menu.
		/// </summary>
		private void MakeButtons()
		{
			var rec = new Rectangle(150, 200, 300, 75);
			for (int i = 0; i < NumberOfButtons; i++)
			{
				Button b = new Button(i, Color.Gray, rec);
				rec = new Rectangle(rec.X, rec.Y + rec.Height + 20, rec.Width, rec.Height);
				MenuButtons.Add(b);
			}

			MenuButtons[ResumeIndex].SetPressedState(StateManager.EGameState.Game);
			MenuButtons[HowToIndex].SetPressedState(StateManager.EGameState.HowTo);
			MenuButtons[MainMenuIndex].SetPressedState(StateManager.EGameState.MainMenu);
			MenuButtons[ExitIndex].SetPressedState(StateManager.EGameState.Exit);
		}

		/// <summary>
		/// Loads all menu content.
		/// </summary>
		public void LoadContent()
		{
			BackgroundTexture = ArtManager.PauseMenuBackground;
			MenuButtons[ResumeIndex].Texture = ArtManager.ResumeButton;
			MenuButtons[HowToIndex].Texture = ArtManager.HowToButton;
			MenuButtons[MainMenuIndex].Texture = ArtManager.MainMenuButton;
			MenuButtons[ExitIndex].Texture = ArtManager.ExitButton;
		}
	}
}
