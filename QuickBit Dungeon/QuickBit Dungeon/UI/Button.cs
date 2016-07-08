using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QuickBit_Dungeon.UI
{
	/// <summary>
	/// A class structure that contains all data
	/// and methods that a button needs in the UI.
	/// </summary>
	internal class Button
	{
		// ======================================
		// ============== Members ===============
		// ======================================	
		
		private int Id { get; set; }
		public Color Alpha { get; internal set; }
		public Rectangle Position { get; internal set; }
		public Texture2D Texture { get; internal set; }
		private StateManager.EGameState GameState { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// A constructor that passes in all
		/// relevent button info.
		/// </summary>
		/// <param name="id">Button ID</param>
		/// <param name="c">Alpha</param>
		/// <param name="r">Rectangle</param>
		/// <param name="t">Texture</param>
		/// <param name="gs">GameState</param>
		public Button(int id, Color c, Rectangle r)
		{
			Id = id;
			Alpha = c;
			Position = r;
		}

		/// <summary>
		/// Sets the state to change to when the
		/// button is pressed.
		/// </summary>
		/// <param name="gs">The gamestate to change to</param>
		public void SetPressedState(StateManager.EGameState gs)
		{
			GameState = gs;
		}
		
		/// <summary>
		/// Reacts if the player hovers over this
		/// current button.
		/// </summary>
		/// <param name="id">The ID to cross check</param>
		public void Hover(int id)
		{
			Alpha = Id == id ? Color.White : Color.Gray;
		}

		/// <summary>
		/// Reacts if the player presses this
		/// current button.
		/// </summary>
		/// <param name="id">The ID to cross check</param>
		public void Released(int id)
		{
			if (Id == id)
				StateManager.SetState(GameState);
		}

		/// <summary>
		/// Draws the button on the screen.
		/// </summary>
		/// <param name="sb">The spritebatch</param>
		public void Draw(SpriteBatch sb)
		{
			sb.Draw(Texture, Position, Alpha);
		}
	}
}
