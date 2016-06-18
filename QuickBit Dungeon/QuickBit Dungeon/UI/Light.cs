using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QuickBit_Dungeon
{
	class Light
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		
		Texture2D lightTex;
		Rectangle lightPos;

		const float LIGHT_SCALE_WIDTH = 1f;
		const float LIGHT_SCALE_HEIGHT = .91f;

		// Properties
		Texture2D LightTex { get { return lightTex; } set { lightTex = value; } }
		Rectangle LightPos { get { return lightPos; } set { lightPos = value; } }

		// ======================================
		// ============== Methods ===============
		// ======================================

		// Constructor
		public Light()
		{
			
		}

		/*
			This method loads all content for the
			light object.
		*/
		public void LoadContent()
		{
			lightTex = ArtManager.LightTex;
		}

		/*
			This method generates a light
			object's position data.
		*/
		public void PositionLight(Vector2 dungeonPosition)
		{
			lightPos = new Rectangle((int)dungeonPosition.X, 
									 (int)dungeonPosition.Y, 
									 (int)(lightTex.Width*LIGHT_SCALE_WIDTH), 
									 (int)(lightTex.Height*LIGHT_SCALE_HEIGHT));
		}

		/*
			This method draws the lighting
			effect to the screen.
		*/
		public void DrawLight(SpriteBatch sb)
		{
			sb.Draw(lightTex, lightPos, Color.White);
		}
	}
}
