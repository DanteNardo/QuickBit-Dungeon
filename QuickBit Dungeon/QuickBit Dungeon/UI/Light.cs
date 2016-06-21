using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuickBit_Dungeon.UI
{
	internal class Light
	{
		// ======================================
		// ============== Members ===============
		// ======================================

		private const float LightScaleWidth = 1f;
		private const float LightScaleHeight = .91f;

		private Texture2D LightTex { get; set; }
		private Rectangle LightPos { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// This method loads all content for the
		///	light object.
		/// </summary>
		public void LoadContent()
		{
			LightTex = ArtManager.LightTex;
		}

		/// <summary>
		/// This method generates a light
		///	object's position data.
		/// </summary>
		/// <param name="dungeonPosition">The position of the dungeon on screen</param>
		public void PositionLight(Vector2 dungeonPosition)
		{
			LightPos = new Rectangle((int) dungeonPosition.X,
				(int) dungeonPosition.Y,
				(int) (LightTex.Width*LightScaleWidth),
				(int) (LightTex.Height*LightScaleHeight));
		}

		/// <summary>
		/// This method draws the lighting
		///	effect to the screen.
		/// </summary>
		/// <param name="sb">The spritebatch</param>
		public void DrawLight(SpriteBatch sb)
		{
			sb.Draw(LightTex, LightPos, Color.White);
		}
	}
}