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
	internal class HowToMenu : Menu
	{
		// ======================================
		// ============== Members ===============
		// ======================================	

		private const int NumberOfButtons = 1;
		private const int ReturnIndex = 0;

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Pause Menu constructor.
		/// </summary>
		public HowToMenu()
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
			var rec = new Rectangle(150, 500, 300, 75);
			for (int i = 0; i < NumberOfButtons; i++)
			{
				Button b = new Button(i, Color.Gray, rec);
				rec = new Rectangle(rec.X, rec.Y + rec.Height + 20, rec.Width, rec.Height);
				Buttons.Add(b);
			}

			SetLastState();
		}

		/// <summary>
		/// Loads all menu content.
		/// </summary>
		public void LoadContent()
		{
			BackgroundTexture = ArtManager.HowToMenuBackground;
			Buttons[ReturnIndex].Texture = ArtManager.ReturnButton;
		}

		/// <summary>
		/// Sets the previous game state so the
		/// how to menu knows which state to return to,
		/// ie pause menu, or main menu.
		/// </summary>
		/// <param name="s">The state to return to</param>
		public void SetLastState()
		{
			Buttons[ReturnIndex].SetPressedState(StateManager.LastState);
		}
	}
}
