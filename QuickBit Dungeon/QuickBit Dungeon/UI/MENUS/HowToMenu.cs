using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.INTERACTION;
using QuickBit_Dungeon.MANAGERS;
using QuickBit_Dungeon.UI.MENUS;

namespace QuickBit_Dungeon.UI
{
	internal class HowToMenu : Menu
	{
		// ======================================
		// ============== Members ===============
		// ======================================	

		private const int NumberOfButtons = 2;
		private const int FirstIndex = 0;
		private const int SecondIndex = 1;

	    public int SlideIndex { get; private set; } = 0;
	    private int LastIndex { get; set; } = 7;

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Pause Menu constructor.
		/// </summary>
		public HowToMenu() : base("horizontal")
		{
			MaxId = NumberOfButtons;
			MenuButtons = new List<Button>();
			MakeButtons();
		}

        /// <summary>
        /// Updates the HowTo menu to make sure it
        /// displays the correct slide and has the
        /// corret buttons pointing to the correct
        /// data.
        /// </summary>
	    public void CheckSlides()
	    {
	        if (SlideIndex == 0)
	        {
			    MenuButtons[FirstIndex].Texture = ArtManager.ReturnButton;
			    MenuButtons[SecondIndex].Texture = ArtManager.NextButton;
			    SetReturnState(FirstIndex);
		        NullState(SecondIndex);
	        }
            else if (SlideIndex == LastIndex)
            {
                MenuButtons[FirstIndex].Texture = ArtManager.LastButton;
                MenuButtons[SecondIndex].Texture = ArtManager.ReturnButton;
			    SetReturnState(SecondIndex);
		        NullState(FirstIndex);  
            }
            else
            {
                MenuButtons[FirstIndex].Texture = ArtManager.LastButton;
                MenuButtons[SecondIndex].Texture = ArtManager.NextButton;
		        NullState(FirstIndex);
		        NullState(SecondIndex);
            }

            switch (SlideIndex)
            {
                case 0: BackgroundTexture = ArtManager.HowToMenuSlide01; break;
                case 1: BackgroundTexture = ArtManager.HowToMenuSlide02; break;
                case 2: BackgroundTexture = ArtManager.HowToMenuSlide03; break;
                case 3: BackgroundTexture = ArtManager.HowToMenuSlide04; break;
                case 4: BackgroundTexture = ArtManager.HowToMenuSlide05; break;
                case 5: BackgroundTexture = ArtManager.HowToMenuSlide06; break;
                case 6: BackgroundTexture = ArtManager.HowToMenuSlide07; break;
            }
	    }

	    public new void Update()
	    {
	        Input.GetInput();
	        if ((Input.Released(Keys.Enter) ||
	            Input.Released(Keys.Space) ||
	            Input.Released(Buttons.A)) && 
                MenuButtons[CurrentId].GameState == StateManager.EGameState.None)
	        {
	            if (MenuButtons[CurrentId].Texture == ArtManager.NextButton)
	                SlideIndex++;
	            if (MenuButtons[CurrentId].Texture == ArtManager.LastButton)
	                SlideIndex--;
	            CheckSlides();
	        }

	        base.Update();
	    }

		/// <summary>
		/// Generates all of the buttons for the
		/// pause menu.
		/// </summary>
		private void MakeButtons()
		{
			var rec = new Rectangle(25, 500, 250, 75);
			for (int i = 0; i < NumberOfButtons; i++)
			{
				Button b = new Button(i, Color.Gray, rec);
				rec = new Rectangle(rec.X + rec.Width + 50, rec.Y, rec.Width, rec.Height);
				MenuButtons.Add(b);
			}

			SetReturnState(FirstIndex);
		    NullState(SecondIndex);
		}

		/// <summary>
		/// Loads all menu content.
		/// </summary>
		public void LoadContent()
		{
		    BackgroundTexture = ArtManager.HowToMenuSlide01;
			MenuButtons[FirstIndex].Texture = ArtManager.ReturnButton;
			MenuButtons[SecondIndex].Texture = ArtManager.NextButton;
		}

		/// <summary>
		/// Sets the previous game state so the
		/// how to menu knows which state to return to,
		/// ie pause menu, or main menu.
		/// </summary>
		/// <param name="index">The index to set</param>
		public void SetReturnState(int index)
		{
		    MenuButtons[index].GameState = StateManager.LastState;
		}

        /// <summary>
        /// Nullifies the game state for the button.
        /// This button does not change game state.
        /// </summary>
        /// <param name="index">The index to set</param>
	    private void NullState(int index)
	    {
	        MenuButtons[index].GameState = StateManager.EGameState.None;
	    }
	}
}
