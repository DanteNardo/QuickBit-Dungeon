using System.Collections.Generic;
using Microsoft.Xna.Framework;
using QuickBit_Dungeon.Managers;
using QuickBit_Dungeon.UI.Menus;

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

		#region Game Over Menu Methods

		/// <summary>
		/// Game Over Menu constructor.
		/// </summary>
		public GameOverMenu() : base("vertical")
		{
			MaxId = NumberOfButtons;
			MenuButtons = new List<Button>();
			MakeButtons();
		}

		/// <summary>
		/// Generates all of the buttons for the
		/// game over menu.
		/// </summary>
		private void MakeButtons()
		{
			var rec = new Rectangle(100, 300, 400, 100);
			for (int i = 0; i < NumberOfButtons; i++)
			{
				Button b = new Button(i, Color.Gray, rec);
				rec = new Rectangle(rec.X, rec.Y + rec.Height + 20, rec.Width, rec.Height);
				MenuButtons.Add(b);
			}

			MenuButtons[MainMenuIndex].GameState = StateManager.EGameState.MainMenu;
			MenuButtons[ExitIndex].GameState = StateManager.EGameState.Exit;
		}

		/// <summary>
		/// Loads all menu content.
		/// </summary>
		public void LoadContent()
		{
			BackgroundTexture = ArtManager.GameOverBackground;
			MenuButtons[MainMenuIndex].Texture = ArtManager.MainMenuButton;
			MenuButtons[ExitIndex].Texture = ArtManager.ExitButton;
		}

		#endregion
	}
}
