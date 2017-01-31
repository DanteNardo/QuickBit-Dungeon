using System.Collections.Generic;
using Microsoft.Xna.Framework;
using QuickBit_Dungeon.Managers;
using QuickBit_Dungeon.UI.Menus;

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

		private const int NumberOfButtons = 4;
		private const int StartIndex = 0;
		private const int HowToIndex = 1;
		private const int OptionsIndex = 2;
		private const int ExitIndex = 3;

		// ======================================
		// ============== Methods ===============
		// ======================================

		#region Main Menu Methods

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
			var rec = new Rectangle(175, 250, 250, 65);
			for (int i = 0; i < NumberOfButtons; i++)
			{
				Button b = new Button(i, Color.Gray, rec);
				rec = new Rectangle(rec.X, rec.Y + rec.Height + 20, rec.Width, rec.Height);
				MenuButtons.Add(b);
			}

			MenuButtons[StartIndex].GameState = StateManager.EGameState.Game;
			MenuButtons[HowToIndex].GameState = StateManager.EGameState.HowTo;
			MenuButtons[OptionsIndex].GameState = StateManager.EGameState.Options;
			MenuButtons[ExitIndex].GameState = StateManager.EGameState.Exit;
		}

		/// <summary>
		/// Loads all menu content.
		/// </summary>
		public void LoadContent()
		{
			BackgroundTexture = ArtManager.MainMenuBackground;
			MenuButtons[StartIndex].Texture = ArtManager.StartButton;
			MenuButtons[HowToIndex].Texture = ArtManager.HowToButton;
			MenuButtons[OptionsIndex].Texture = ArtManager.OptionsButton;
			MenuButtons[ExitIndex].Texture = ArtManager.ExitButton;
		}

		#endregion
	}
}
