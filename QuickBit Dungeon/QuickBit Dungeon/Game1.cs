using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuickBit_Dungeon.CORE;
using QuickBit_Dungeon.DUNGEON;

namespace QuickBit_Dungeon
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		// Window size variables
		private const int SCREEN_WIDTH = 600;
		private const int SCREEN_HEIGHT = 600;
		private readonly GraphicsDeviceManager _graphics;
		private Vector2 _screenCenter;
		private SpriteBatch _spriteBatch;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
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
			/*
				Determine the center of the screen. Initialize
				the size of the window.
			*/
			_screenCenter = new Vector2(SCREEN_WIDTH/2, SCREEN_HEIGHT/2);
			_graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
			_graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
			_graphics.ApplyChanges();


			// Dungeon initialiazation
			Dungeon.Construct();

			// World initialization
			World.Init();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// Load and save all game content
			ArtManager.LoadContent(Content);
			World.LoadContent();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{

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

			World.Update();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			_spriteBatch.Begin();
			World.Draw(_spriteBatch);
			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}