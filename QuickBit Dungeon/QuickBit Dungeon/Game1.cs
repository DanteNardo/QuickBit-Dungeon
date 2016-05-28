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

		SpriteFont DUNGEON_FONT;

		const float DEFAULT_SCREEN_WIDTH  = 800f;
		const float DEFAULT_SCREEN_HEIGHT = 800f;
		float SCREEN_WIDTH  = DEFAULT_SCREEN_WIDTH;
		float SCREEN_HEIGHT = DEFAULT_SCREEN_HEIGHT;
		Vector2 SCREEN_CENTER;
		float GAME_SCALE = 1f;

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

			// Dungeon initialiazation
			Dungeon.Construct();

			/*
				Get the size of the screen. Then determine the center. Then determine
				the scale based off of what the screen should be.
			*/
			SCREEN_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			SCREEN_HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			GAME_SCALE = SCREEN_HEIGHT/DEFAULT_SCREEN_HEIGHT;
			SCREEN_CENTER = new Vector2(SCREEN_WIDTH/2, SCREEN_HEIGHT/2);
			graphics.PreferredBackBufferWidth = (int)SCREEN_WIDTH;
			graphics.PreferredBackBufferHeight = (int)SCREEN_HEIGHT;

			graphics.ApplyChanges();

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
			DUNGEON_FONT = Content.Load<SpriteFont>("dungeonFont");
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

			// Handles all game input and dungeon updating
			Input.Update();

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
			spriteBatch.End();

			base.Draw(gameTime);
		}

		/*
			Draws the player's view of the dungeon.
		*/
		private void DrawDungeon(SpriteBatch sb)
		{
			string s = Dungeon.PlayerView();

			sb.DrawString(DUNGEON_FONT,
						  s,
						  (SCREEN_CENTER-(DUNGEON_FONT.MeasureString(s)/2)) * (1f/GAME_SCALE),
						  Color.White,
						  0,
						  Vector2.Zero,
						  GAME_SCALE,
						  SpriteEffects.None,
						  0);
		}
	}
}
