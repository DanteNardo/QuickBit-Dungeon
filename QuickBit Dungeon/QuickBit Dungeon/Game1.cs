using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuickBit_Dungeon.CORE;
using QuickBit_Dungeon.DUNGEON;
using QuickBit_Dungeon.INTERACTION;
using QuickBit_Dungeon.UI;

namespace QuickBit_Dungeon
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		// Window size variables
		private const int ScreenWidth = 600;
		private const int ScreenHeight = 600;
		private readonly GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		// User Interface Objects
		private MainMenu _mainMenu;
		private HowToMenu _howToMenu;
		private PauseMenu _pauseMenu;
		private GameOverMenu _gameOverMenu;

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
			_graphics.PreferredBackBufferWidth = ScreenWidth;
			_graphics.PreferredBackBufferHeight = ScreenHeight;
			_graphics.ApplyChanges();

			// GameManager initialization
			GameManager.Init();

			// Dungeon initialiazation
			Dungeon.Construct();

			// World initialization
			World.Init();

			// User Interface initialization
			_mainMenu = new MainMenu();
			_howToMenu = new HowToMenu();
			_pauseMenu = new PauseMenu();
			_gameOverMenu = new GameOverMenu();

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
			_mainMenu.LoadContent();
			_howToMenu.LoadContent();
			_pauseMenu.LoadContent();
			_gameOverMenu.LoadContent();
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

			// Update all input
			Input.Update();

			switch (StateManager.GameState)
			{
				case StateManager.EGameState.MainMenu:
					_mainMenu.Update();
					_mainMenu.Hover();
					break;
				case StateManager.EGameState.Game:
					World.Update();
					break;
				case StateManager.EGameState.HowTo:
					_howToMenu.SetLastState();
					_howToMenu.Update();
					_howToMenu.Hover();
					break;
				case StateManager.EGameState.Pause:
					_pauseMenu.Update();
					_pauseMenu.Hover();
					break;
				case StateManager.EGameState.GameOver:
					_gameOverMenu.Update();
					_gameOverMenu.Hover();
					break;
				case StateManager.EGameState.Exit:
					Exit();
					break;
			}

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

			switch (StateManager.GameState)
			{
				case StateManager.EGameState.MainMenu:
					_mainMenu.Draw(_spriteBatch);
					break;
				case StateManager.EGameState.Game:
					World.Draw(_spriteBatch);
					break;
				case StateManager.EGameState.HowTo:
					_howToMenu.Draw(_spriteBatch);
					break;
				case StateManager.EGameState.Pause:
					_pauseMenu.Draw(_spriteBatch);
					break;
				case StateManager.EGameState.GameOver:
					_gameOverMenu.Draw(_spriteBatch);
					break;
			}

			_spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}