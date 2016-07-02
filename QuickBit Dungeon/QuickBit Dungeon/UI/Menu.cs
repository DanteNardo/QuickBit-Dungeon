using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuickBit_Dungeon.INTERACTION;

namespace QuickBit_Dungeon.UI
{
	/// <summary>
	/// An inherited class for all menus.
	/// </summary>
	internal class Menu
	{
		// ======================================
		// ============== Members ===============
		// ======================================	

		public int CurrentId { get; internal set; } = 0;
		public int MaxId { get; set; }
		public Texture2D BackgroundTexture { get; internal set; }
		public Rectangle BackgroundPosition { get; internal set; } = new Rectangle(0, 0, 600, 600);
		public List<Button> Buttons { get; internal set; }

		public const int ButtonWidth = 100;
		public const int ButtonHeight = 25;

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Changes the color of the button that
		/// is hovered over.
		/// </summary>
		public void Hover()
		{
			foreach (var button in Buttons)
				button.Hover(CurrentId);
		}

		/// <summary>
		/// Changes the gamestate based off of
		/// the button that was pressed.
		/// </summary>
		public void Released()
		{
			foreach (var button in Buttons)
				button.Released(CurrentId);
		}

		/// <summary>
		/// Updates the menu with user interaction.
		/// </summary>
		public void Update()
		{
			Input.GetInput();
			switch (Input.CurrentDirection)
			{
				case Input.Direction.North:
					UpdateId(-1);
					break;
				case Input.Direction.South:
					UpdateId(1);
					break;
			}

			if (Input.Released(Keys.Enter) ||
			    Input.Released(Keys.Space))
				Released();
		}

		/// <summary>
		/// Correctly updates and wraps the
		/// current id value.
		/// </summary>
		/// <param name="amount">The amount to change the id by</param>
		private void UpdateId(int amount)
		{
			CurrentId += amount;
			if (CurrentId < 0)
				CurrentId = MaxId;
			if (CurrentId >= MaxId)
				CurrentId = 0;
		}

		/// <summary>
		/// Draws all buttons and background
		/// image for the menu.
		/// </summary>
		/// <param name="sb">The spritebatch</param>
		public void Draw(SpriteBatch sb)
		{
			sb.Draw(BackgroundTexture, BackgroundPosition, Color.White);

			foreach (var button in Buttons)
				button.Draw(sb);
		}
	}
}
