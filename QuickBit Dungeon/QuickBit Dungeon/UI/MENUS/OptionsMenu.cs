using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuickBit_Dungeon.INTERACTION;
using QuickBit_Dungeon.MANAGERS;

namespace QuickBit_Dungeon.UI.MENUS
{
	class OptionsMenu : Menu
	{
		// ======================================
		// ============== Members ===============
		// ======================================	

		private const int NumberOfButtons = 4;
		private const int VolumeIndex = 0;
		private const int EasyIndex = 1;
		private const int HardIndex = 2;
		private const int MenuIndex = 3;

		public ProgressBar VolumeBar { get; private set; }
		public string Difficulty { get; private set; } = "easy";
		public float Volume { get; private set; } = .75F;

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Pause Menu constructor.
		/// </summary>
		public OptionsMenu() : base("vertical")
		{
			MaxId = NumberOfButtons;
			MenuButtons = new List<Button>();
			VolumeBar = new ProgressBar("Volume");
			VolumeBar.Position = new Vector2(110, 150);
			VolumeBar.Init(100, (int)(Volume*100));
			MakeButtons();
		}

		/// <summary>
		/// Updates the Options menu.
		/// </summary>
	    public new void Update()
		{
			UpdateVolume();
			UpdateDifficulty();

			base.Update();
	    }

		/// <summary>
		/// Draws the options menu and the difficulty
		/// and the progress bar.
		/// </summary>
		/// <param name="sb">The spritebatch to draw with</param>
		public new void Draw(SpriteBatch sb)
		{
			base.Draw(sb);

			VolumeBar.DrawProgressBar(sb);
			sb.DrawString(ArtManager.HudFont, 
						  "Difficulty: " + Difficulty, 
						  new Vector2(110, 180), 
						  Color.White);
		}

		/// <summary>
		/// Updates the volume bar and values
		/// based off of user input.
		/// </summary>
		private void UpdateVolume()
		{
			Input.GetInput();

	        if (Input.Released(Keys.A) || 
				Input.Released(Buttons.DPadLeft))
	        {
		        Volume -= .1F;
		        if (Volume < 0) Volume = 0;
		        VolumeBar.UpdateValues(100, (int)(Volume*100));
				MediaPlayer.Volume = Volume;
				SoundEffect.MasterVolume = Volume;
				AudioManager.NewMenuSound();
	        }

			if (Input.Released(Keys.D) || 
				Input.Released(Buttons.DPadRight))
	        {
		        Volume += .1F;
		        if (Volume > 1) Volume = 1;
		        VolumeBar.UpdateValues(100, (int)(Volume*100));
				MediaPlayer.Volume = Volume;
				SoundEffect.MasterVolume = Volume;
				AudioManager.NewMenuSound();
	        }
		}

		/// <summary>
		/// Updates the difficulty display and value
		/// based off of user input.
		/// </summary>
		private void UpdateDifficulty()
		{
			if ((Input.Released(Keys.Enter) ||
			     Input.Released(Keys.Space) ||
			     Input.Released(Buttons.A)) &&
			    MenuButtons[CurrentId].GameState == StateManager.EGameState.None)
			{
				if (CurrentId == HardIndex)
					SetHardDifficulty();
				else if (CurrentId == EasyIndex)
					SetEasyDifficulty();
			}
		}

		/// <summary>
		/// Generates all of the buttons for the
		/// pause menu.
		/// </summary>
		private void MakeButtons()
		{
			//var rec = new Rectangle(25, 500, 250, 75);
			var rec = new Rectangle(175, 250, 250, 60);
			for (int i = 0; i < NumberOfButtons; i++)
			{
				Button b = new Button(i, Color.Gray, rec);
				rec = new Rectangle(rec.X, rec.Y + rec.Height + 20, rec.Width, rec.Height);
				MenuButtons.Add(b);
			}

			NullState(VolumeIndex);
			NullState(EasyIndex);
			NullState(HardIndex);
			MenuButtons[MenuIndex].GameState = StateManager.EGameState.MainMenu;
		}

		/// <summary>
		/// Loads all menu content.
		/// </summary>
		public void LoadContent()
		{
		    BackgroundTexture = ArtManager.OptionsMenuBackground;
			MenuButtons[VolumeIndex].Texture = ArtManager.VolumeButton;
			MenuButtons[EasyIndex].Texture = ArtManager.EasyButton;
			MenuButtons[HardIndex].Texture = ArtManager.HardButton;
			MenuButtons[MenuIndex].Texture = ArtManager.MainMenuButton;
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

		/// <summary>
		/// Sets the game to the hard difficulty
		/// and updates the options menu to display
		/// that.
		/// </summary>
		private void SetHardDifficulty()
		{
			Difficulty = "hard";
			GameManager.Difficulty = Difficulty;
		}

		/// <summary>
		/// Sets the game to the easy difficulty
		/// and updates the options menu to display
		/// that.
		/// </summary>
		private void SetEasyDifficulty()
		{
			Difficulty = "easy";
			GameManager.Difficulty = Difficulty;
		}
	}
}
