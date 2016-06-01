﻿using System;
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

		const float LIGHT_SCALE = 1.1f;

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
		public void PositionLight(Vector2 SCREEN_CENTER)
		{
			lightPos = new Rectangle((int)((SCREEN_CENTER.X-lightTex.Width/2*LIGHT_SCALE)),
									 (int)((SCREEN_CENTER.Y-lightTex.Height/2*LIGHT_SCALE)),
									 (int)(lightTex.Width*LIGHT_SCALE),
									 (int)(lightTex.Height*LIGHT_SCALE));
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
