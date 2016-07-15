using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.MANAGERS;

namespace QuickBit_Dungeon.UI
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
		public PauseMenu()
		{
			MaxId = NumberOfButtons;
			Buttons = new List<Button>();
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
				Buttons.Add(b);
			}

			Buttons[ResumeIndex].SetPressedState(StateManager.EGameState.Game);
			Buttons[HowToIndex].SetPressedState(StateManager.EGameState.HowTo);
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
			Buttons[HowToIndex].Texture = ArtManager.HowToButton;
			Buttons[MainMenuIndex].Texture = ArtManager.MainMenuButton;
			Buttons[ExitIndex].Texture = ArtManager.ExitButton;
		}
	}
}
