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
			Shade = new Color(20, 20, 20);

			HandleType(type);
			DeterminePosition();
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
			Position = new Vector2(300 - ArtManager.HudFont.MeasureString(Type).X/2, 70);
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
				case "Sprinter":
					Sprinter();
					break;
				case "SuperSprinter":
					SuperSprinter();
					break;
				case "Energize":
					Energize();
					break;
				case "Revitalize":
					Revitalize();
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
			Shade *= 1.3f;
		}

		#region AwardTypes

		/// <summary>
		/// A single kill.
		/// </summary>
		private void SingleKill()
		{
			Type = "Meh";
			Value = 10;
		}

		/// <summary>
		/// A double kill in one second or less.
		/// </summary>
		private void DoubleKill()
		{
			Type = "Double Kill!";
			Value = 200;
		}

		/// <summary>
		/// A triple kill in one second or less.
		/// </summary>
		private void TripleKill()
		{
			Type = "Triple Kill!";
			Value = 600;
		}

		/// <summary>
		/// 10-19 steps in 1 second or less.
		/// </summary>
		private void Sprinter()
		{
			Type = "Sprinter";
			Value = 20;
		}

		/// <summary>
		/// 20+ steps in 1 second or less.
		/// </summary>
		private void SuperSprinter()
		{
			Type = "Super Sprinter!";
			Value = 400;
		}

		/// <summary>
		/// 10-19 regens in 1 second or less.
		/// </summary>
		private void Energize()
		{
			Type = "Energize";
			Value = 20;
		}

		/// <summary>
		/// 20+ regens in 1 second or less.
		/// </summary>
		private void Revitalize()
		{
			Type = "Revitalize";
			Value = 400;
		}

		#endregion
	}
}