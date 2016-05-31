using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QuickBit_Dungeon
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		// For drawing the dungeon
		SpriteFont DUNGEON_FONT;
		Vector2 dgPos;
		
		// For special effects
		Texture2D light;
		Rectangle lightPos;
		const float LIGHT_SCALE = 1.1f;

		// Window size variables
		const int SCREEN_WIDTH  = 600;
		const int SCREEN_HEIGHT = 600;
		Vector2 SCREEN_CENTER;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			/*
				Determine the center of the screen. Initialize
				the size of the window.
			*/
			SCREEN_CENTER = new Vector2(SCREEN_WIDTH/2, SCREEN_HEIGHT/2);
			graphics.PreferredBackBufferWidth= SCREEN_WIDTH;
			graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
			graphics.ApplyChanges();


			// Dungeon initialiazation
			Dungeon.Construct();

			// GameManager intialization
			GameManager.Init();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here

			// Dungeon Content
			DUNGEON_FONT = Content.Load<SpriteFont>("DungeonFont");
			dgPos = (SCREEN_CENTER-(DUNGEON_FONT.MeasureString(Dungeon.PlayerView())/2));

			// Special Effects
			light = Content.Load<Texture2D>("LightingEffect");
			lightPos = new Rectangle((int)((SCREEN_CENTER.X-light.Width/2*LIGHT_SCALE)),
									 (int)((SCREEN_CENTER.Y-light.Height/2*LIGHT_SCALE)),
									 (int)(light.Width*LIGHT_SCALE),
									 (int)(light.Height*LIGHT_SCALE));
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
				Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here
			GameManager.Update();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here
			spriteBatch.Begin();
			DrawDungeon(spriteBatch);
			DrawLight(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}

		/*
			Draws the player's view of the dungeon.
		*/
		private void DrawDungeon(SpriteBatch sb)
		{
			sb.DrawString(DUNGEON_FONT,
						  Dungeon.PlayerView(),
						  dgPos,
						  Color.White);
		}

		/*
			Draws the lighting effect over the
			dungeon view.
		*/
		private void DrawLight(SpriteBatch sb)
		{
			sb.Draw(light, lightPos, Color.White);
		}
	}
}
