using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuickBit_Dungeon.UI.EFFECTS
{
    class TvLines
    {
        // ======================================
		// ============== Members ===============
		// ======================================	

        private const int Length = 600;
        private const int Height = 1;
        private const int Steps = 6;
		private List<Texture2D> Textures { get; set; }
        private List<Rectangle> Rectangles { get; set; }
        private Color[] Data { get; set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="graphicsDevice">The game's graphics device</param>
        public TvLines(GraphicsDevice graphicsDevice)
        {
            Data = new Color[Length*Height];
            Textures = new List<Texture2D>();
            Rectangles = new List<Rectangle>();

            GenerateColorData();
            GenerateTvLines(graphicsDevice);
        }

        /// <summary>
        /// Generates the color data structure.
        /// </summary>
        private void GenerateColorData()
        {
            for (int i = 0; i < Data.Length; i++)
                Data[i] = Color.Black;
        }

        /// <summary>
        /// Generates the data behind all of the
        /// black Tv lines that fill the screen.
        /// </summary>
        /// <param name="graphicsDevice">The game's graphics device</param>
        private void GenerateTvLines(GraphicsDevice graphicsDevice)
        {
            int step = 0;
            for (int i = 0; i < 600; i++)
            {
                if (step == Steps)
                {
                    step = 0;
                    var tex = new Texture2D(graphicsDevice, Length, Height);
                    tex.SetData(Data);
                    Textures.Add(tex);
                    Rectangles.Add(new Rectangle(0, i, Length, Height));
                    i += 2;
                }
                step++;
            }
        }

        /// <summary>
        /// Draws the Tv Lines effect.
        /// </summary>
        /// <param name="sb">The spritebatch to draw with</param>
        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < Textures.Count; i++)
                sb.Draw(Textures[i], Rectangles[i], Color.White);
        }
    }
}
