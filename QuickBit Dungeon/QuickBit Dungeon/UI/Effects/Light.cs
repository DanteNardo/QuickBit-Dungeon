using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickBit_Dungeon.MANAGERS;

namespace QuickBit_Dungeon.UI.Effects
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
        /// Constructor
        /// </summary>
	    public Light()
	    {
	        PositionLight();
	    }

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
		private void PositionLight()
		{
		    LightPos = new Rectangle(130, 100, 330, 330);
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