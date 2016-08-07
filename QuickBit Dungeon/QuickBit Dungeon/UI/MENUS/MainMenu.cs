using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.MANAGERS;
using QuickBit_Dungeon.UI.MENUS;

namespace QuickBit_Dungeon.UI
{
	/// <summary>
	/// Represents the visuals for the main menu
	/// and also handles all interaction between
	/// menu and the user.
	/// </summary>
	internal class MainMenu : Menu
	{
		// ======================================
		// ============== Members ===============
		// ======================================	

		private const int NumberOfButtons = 3;
		private const int StartIndex = 0;
		private const int HowToIndex = 1;
		private const int ExitIndex = 2;

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Main Menu constructor.
		/// </summary>
		public MainMenu() : base("vertical")
		{
			MaxId = NumberOfButtons;
			MenuButtons = new List<Button>();
			MakeButtons();
		}

		/// <summary>
		/// Generates all of the buttons for the
		/// main menu.
		/// </summary>
		private void MakeButtons()
		{
			var rec = new Rectangle(150, 275, 300, 75);
			for (int i = 0; i < NumberOfButtons; i++)
			{
				Button b = new Button(i, Color.Gray, rec);
				rec = new Rectangle(rec.X, rec.Y + rec.Height + 20, rec.Width, rec.Height);
				MenuButtons.Add(b);
			}

			MenuButtons[StartIndex].SetPressedState(StateManager.EGameState.Game);
			MenuButtons[HowToIndex].SetPressedState(StateManager.EGameState.HowTo);
			MenuButtons[ExitIndex].SetPressedState(StateManager.EGameState.Exit);
		}

		/// <summary>
		/// Loads all menu content.
		/// </summary>
		public void LoadContent()
		{
			BackgroundTexture = ArtManager.MainMenuBackground;
			MenuButtons[StartIndex].Texture = ArtManager.StartButton;
			MenuButtons[HowToIndex].Texture = ArtManager.HowToButton;
			MenuButtons[ExitIndex].Texture = ArtManager.ExitButton;
		}
	}
}
