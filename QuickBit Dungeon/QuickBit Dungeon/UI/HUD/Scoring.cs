using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.Managers;

namespace QuickBit_Dungeon.UI.HUD
{
	class Scoring
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		private List<Award> Awards { get; set; }
		private int Score { get; set; } = 0;
		private Vector2 Position { get; set; }

		// ======================================
		// =============== Main =================
		// ======================================

		#region Scoring Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public Scoring()
		{
			Awards = new List<Award>();
			Position = new Vector2(10, 70);
		}

		/// <summary>
		/// Updates the score and all of the
		/// awards that the player is earning.
		/// </summary>
		public void Update()
		{
			foreach (var award in Awards)
				award.Update();

			if (Awards.Count > 0 && Awards[0].Dead)
			{
				Score += Awards[0].Value;
				Awards.RemoveAt(0);
			}
		}

		/// <summary>
		/// Draws the score and awards.
		/// </summary>
		/// <param name="sb"></param>
		public void Draw(SpriteBatch sb)
		{
			DrawScore(sb);
			if (Awards.Count > 0)
				Awards[0].Draw(sb);
		}

		/// <summary>
		/// Draws the player's current score.
		/// </summary>
		/// <param name="sb">The spritebatch to draw with</param>
		private void DrawScore(SpriteBatch sb)
		{
			sb.DrawString(ArtManager.HudFont, 
						  "Score:" + Score, 
						  Position, 
						  Color.White);
		}

		/// <summary>
		/// Adds an award to the list of awards.
		/// </summary>
		/// <param name="award">The award to add</param>
		public void AddAward(Award award)
		{
			Awards.Add(award);
		}

		#endregion
	}
}
