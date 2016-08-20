using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.INTERACTION;
using QuickBit_Dungeon.MANAGERS;

namespace QuickBit_Dungeon.UI.HUD
{
	public class Award
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		
		public int Value { get; private set; }
		private string Type { get; set; }
		private Vector2 Position { get; set; }
		private Color Shade { get; set; }
		private Timer AnimTime { get; set; }
		public bool Dead { get; private set; } = false;

		// ======================================
		// =============== Main =================
		// ======================================

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="type">The type of award</param>
		public Award(string type)
		{
			AnimTime = new Timer((int)(.75*60));
			Position = new Vector2(200, 75);
			Shade = new Color(20, 20, 20);

			HandleType(type);
			AnimTime.PerformAction();
		}

		/// <summary>
		/// Updates the Award
		/// </summary>
		public void Update()
		{
			AnimTime.Update();
			IncreaseOpacity();
			if (AnimTime.ActionReady)
				Destroy();
		}

		/// <summary>
		/// Draws the award to the screen.
		/// </summary>
		/// <param name="sb">The spritebatch to draw with</param>
		public void Draw(SpriteBatch sb)
		{
			sb.DrawString(ArtManager.HudFont, Type, Position, Shade);
		}

		/// <summary>
		/// Sets the position correctly no matter the type.
		/// </summary>
		private void DeterminePosition()
		{
			
		}

		/// <summary>
		/// Handles the award's typing.
		/// </summary>
		/// <param name="type">Type of award</param>
		private void HandleType(string type)
		{
			switch (type)
			{
				case "SingleKill":
					SingleKill();
					break;
				case "DoubleKill":
					DoubleKill();
					break;
				case "TripleKill":
					TripleKill();
					break;
			}
		}

		/// <summary>
		/// Sets the award to dead.
		/// </summary>
		private void Destroy()
		{
			Dead = true;
		}

		/// <summary>
		/// Increases the opacity slightly.
		/// </summary>
		private void IncreaseOpacity()
		{
			Shade *= 1.4f;
		}

		#region AwardTypes

		/// <summary>
		/// A single kill.
		/// </summary>
		private void SingleKill()
		{
			Type = "Single Kill";
			Value = 100;
		}

		/// <summary>
		/// A double kill.
		/// </summary>
		private void DoubleKill()
		{
			Type = "Double Kill!";
			Value = 200;
		}

		/// <summary>
		/// A triple kill.
		/// </summary>
		private void TripleKill()
		{
			Type = "Triple Kill!";
			Value = 300;
		}

		#endregion
	}
}