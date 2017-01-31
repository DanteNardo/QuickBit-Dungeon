using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QuickBit_Dungeon.DungeonGeneration;
using QuickBit_Dungeon.Interaction;
using QuickBit_Dungeon.Managers;
using QuickBit_Dungeon.UI;
using QuickBit_Dungeon.UI.Effects;
using QuickBit_Dungeon.UI.Menus;

namespace QuickBit_Dungeon
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		// ======================================
		// ============== Members ===============
		// ======================================

		// Window size variables
		private const int ScreenWidth = 600;
		private const int ScreenHeight = 600;
		private readonly GraphicsDeviceManager m_graphics;
		private SpriteBatch m_spriteBatch;

		// User Interface Objects
		private MainMenu m_mainMenu;
		private HowToMenu m_howToMenu;
		private OptionsMenu m_optionsMenu;
		private PauseMenu m_pauseMenu;
	    private LevelUpMenu m_levelUpMenu;
		private GameOverMenu m_gameOverMenu;

        // Effects
	    private TvLines _tvLines;

		// ======================================
		// ============== Methods ===============
		// ======================================

		#region Game Methods

		public Game1()
		{
			m_graphics = new GraphicsDeviceManager(this);

            //_graphics.IsFullScreen = true;
		    //_graphics.ToggleFullScreen();
            if (m_graphics.IsFullScreen)
            {
				Window.IsBorderless = true;
				m_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
				m_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			}

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
			m_graphics.PreferredBackBufferWidth = ScreenWidth;
			m_graphics.PreferredBackBufferHeight = ScreenHeight;
			m_graphics.ApplyChanges();

			// GameManager initialization
			GameManager.Init();
			AudioManager.Init();

			// World initialization
			World.Init();

			// User Interface initialization
			m_mainMenu = new MainMenu();
			m_howToMenu = new HowToMenu();
			m_optionsMenu = new OptionsMenu();
			m_pauseMenu = new PauseMenu();
		    m_levelUpMenu = new LevelUpMenu();
			m_gameOverMenu = new GameOverMenu();

            // Effect Initialization
		    _tvLines = new TvLines(GraphicsDevice);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			m_spriteBatch = new SpriteBatch(GraphicsDevice);

			// Load and save all game content
			ArtManager.LoadContent(Content);
		    AudioManager.LoadContent(Content);
			World.LoadContent();
			m_mainMenu.LoadContent();
			m_howToMenu.LoadContent();
			m_optionsMenu.LoadContent();
			m_pauseMenu.LoadContent();
		    m_levelUpMenu.LoadContent();
			m_gameOverMenu.LoadContent();

		    AudioManager.PlayMainMusic();
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
					m_mainMenu.Update();
					m_mainMenu.Hover();
					break;
				case StateManager.EGameState.Game:
					World.Update();
					break;
				case StateManager.EGameState.HowTo:
					m_howToMenu.Update();
					m_howToMenu.Hover();
					break;
				case StateManager.EGameState.Options:
					m_optionsMenu.Update();
					m_optionsMenu.Hover();
					m_optionsMenu.VolumeBar.Update();
					break;
				case StateManager.EGameState.Pause:
					m_pauseMenu.Update();
					m_pauseMenu.Hover();
					break;
                case StateManager.EGameState.LevelUp:
			        m_levelUpMenu.Update();
			        m_levelUpMenu.Hover();
                    break;
                case StateManager.EGameState.RedLevelUp:
			        Dungeon.MainPlayer.LevelUp("red");
			        StateManager.SetState(StateManager.EGameState.Game);
                    break;
                case StateManager.EGameState.GreenLevelUp:
			        Dungeon.MainPlayer.LevelUp("green");
			        StateManager.SetState(StateManager.EGameState.Game);
                    break;
                case StateManager.EGameState.BlueLevelUp:
			        Dungeon.MainPlayer.LevelUp("blue");
			        StateManager.SetState(StateManager.EGameState.Game);
                    break;
				case StateManager.EGameState.GameOver:
					m_gameOverMenu.Update();
					m_gameOverMenu.Hover();
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
		    m_spriteBatch.Begin();

			switch (StateManager.GameState)
			{
				case StateManager.EGameState.MainMenu:
					m_mainMenu.Draw(m_spriteBatch);
					break;
				case StateManager.EGameState.Game:
					World.Draw(m_spriteBatch);
					break;
				case StateManager.EGameState.HowTo:
					m_howToMenu.Draw(m_spriteBatch);
					break;
				case StateManager.EGameState.Options:
					m_optionsMenu.Draw(m_spriteBatch);
					break;
				case StateManager.EGameState.Pause:
					m_pauseMenu.Draw(m_spriteBatch);
					break;
                case StateManager.EGameState.LevelUp:
			        m_levelUpMenu.Draw(m_spriteBatch);
			        break;
				case StateManager.EGameState.GameOver:
					m_gameOverMenu.Draw(m_spriteBatch);
					break;
			}

            // Effects
		    //_tvLines.Draw(_spriteBatch);

			m_spriteBatch.End();
			base.Draw(gameTime);
		}

		#endregion
	}
}