using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using QuickBit_Dungeon.MANAGERS;

namespace QuickBit_Dungeon.UI.MENUS
{
    class LevelUpMenu : Menu
    {
        // ======================================
		// ============== Members ===============
		// ======================================	

		private const int NumberOfButtons = 3;
		private const int RedIndex = 0;
		private const int GreenIndex = 1;
		private const int BlueIndex = 2;

		// ======================================
		// ============== Methods ===============
		// ======================================

        /// <summary>
		/// Main Menu constructor.
		/// </summary>
		public LevelUpMenu() : base("horizontal")
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
			var rec = new Rectangle(0, 325, 200, 200);
			for (int i = 0; i < NumberOfButtons; i++)
			{
				Button b = new Button(i, Color.Gray, rec);
				rec = new Rectangle(rec.X + rec.Width, rec.Y, rec.Width, rec.Height);
				MenuButtons.Add(b);
			}

			MenuButtons[RedIndex].GameState = StateManager.EGameState.RedLevelUp;
			MenuButtons[GreenIndex].GameState = StateManager.EGameState.GreenLevelUp;
			MenuButtons[BlueIndex].GameState = StateManager.EGameState.BlueLevelUp;
		}

		/// <summary>
		/// Loads all menu content.
		/// </summary>
		public void LoadContent()
		{
			BackgroundTexture = ArtManager.LevelUpMenuBackground;
			MenuButtons[RedIndex].Texture = ArtManager.RedAuraButton;
			MenuButtons[GreenIndex].Texture = ArtManager.GreenAuraButton;
			MenuButtons[BlueIndex].Texture = ArtManager.BlueAuraButton;
		}
    }
}
