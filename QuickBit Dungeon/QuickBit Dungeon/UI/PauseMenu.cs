using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace QuickBit_Dungeon.UI
{
	internal class PauseMenu : Menu
	{
		// ======================================
		// ============== Members ===============
		// ======================================	

		private const int NumberOfButtons = 3;
		private const int ResumeIndex = 0;
		private const int MainMenuIndex = 1;
		private const int ExitIndex = 2;

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Main Menu constructor.
		/// </summary>
		public PauseMenu()
		{
			MaxId = NumberOfButtons;
			Buttons = new List<Button>();
			MakeButtons();
		}

		/// <summary>
		/// Generates all of the buttons for the
		/// main menu.
		/// </summary>
		private void MakeButtons()
		{
			var rec = new Rectangle(100, 200, 400, 100);
			for (int i = 0; i < NumberOfButtons; i++)
			{
				Button b = new Button(i, Color.White, rec);
				rec = new Rectangle(rec.X, rec.Y + rec.Height + 20, rec.Width, rec.Height);
				Buttons.Add(b);
			}

			Buttons[ResumeIndex].SetPressedState(StateManager.EGameState.Game);
			Buttons[MainMenuIndex].SetPressedState(StateManager.EGameState.MainMenu);
			Buttons[ExitIndex].SetPressedState(StateManager.EGameState.Exit);
		}

		/// <summary>
		/// Loads all menu content.
		/// </summary>
		public void LoadContent()
		{
			BackgroundTexture = ArtManager.PauseMenuBackground;
			Buttons[ResumeIndex].Texture = ArtManager.ResumeButton;
			Buttons[MainMenuIndex].Texture = ArtManager.MainMenuButton;
			Buttons[ExitIndex].Texture = ArtManager.ExitButton;
		}
	}
}
