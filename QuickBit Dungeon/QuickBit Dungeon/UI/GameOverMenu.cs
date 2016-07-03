using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace QuickBit_Dungeon.UI
{
	internal class GameOverMenu : Menu
	{
		// ======================================
		// ============== Members ===============
		// ======================================	

		private const int NumberOfButtons = 2;
		private const int MainMenuIndex = 0;
		private const int ExitIndex = 1;

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Main Menu constructor.
		/// </summary>
		public GameOverMenu()
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
			var rec = new Rectangle(100, 300, 400, 100);
			for (int i = 0; i < NumberOfButtons; i++)
			{
				Button b = new Button(i, Color.White, rec);
				rec = new Rectangle(rec.X, rec.Y + rec.Height + 25, rec.Width, rec.Height);
				Buttons.Add(b);
			}

			Buttons[MainMenuIndex].SetPressedState(StateManager.EGameState.MainMenu);
			Buttons[ExitIndex].SetPressedState(StateManager.EGameState.Exit);
		}

		/// <summary>
		/// Loads all menu content.
		/// </summary>
		public void LoadContent()
		{
			BackgroundTexture = ArtManager.GameOverBackground;
			Buttons[MainMenuIndex].Texture = ArtManager.MainMenuButton;
			Buttons[ExitIndex].Texture = ArtManager.ExitButton;
		}
	}
}
