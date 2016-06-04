using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QuickBit_Dungeon
{
	/*
		Handles all game functionality.
		Controls the flow of the game.
	*/
	public static class GameManager
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		
		private static Player player;				// Stores all player data
		private static List<Monster> monsters;		// Stores all monsters in the level
		private static Monster target;				// Stores the currently targeted enemy
		private static Random rnd;					// Random number generator
		private static StatBox statBox;				// Displays the player's stats
		private static Light light;					// Draws the lighting effect
		private static ProgressBar healthBar;		// Displays the player's health mana bar
		private static ProgressBar attackBar;		// Displays the player's attack mana bar

		// For drawing placement
		private static Vector2 screenCenter = new Vector2(300, 300);
		private static Vector2 dgPos;

		// Properties
		public static Player MainPlayer { get { return player; } set { player = value; } }

		// ======================================
		// =========== Main Methods =============
		// ======================================

		/*
			Initializes internal data.
		*/
		public static void Init()
		{
			player    = new Player();
			monsters  = new List<Monster>();
			rnd		  = new Random();
			light     = new Light();
			statBox   = new StatBox();
			healthBar = new ProgressBar("Health Mana");
			attackBar = new ProgressBar("Attack Mana");
			healthBar.Position = new Vector2(10, 10);
			attackBar.Position = new Vector2(10, 50);
			GenerateMonsters();
		}

		/*
			Initializes and places monsters 
			in rooms throughout the dungeon.
		*/
		private static void GenerateMonsters()
		{
			int ry;
			int rx;

			foreach (Room r in Dungeon.Rooms)
			{
				int monsterAmount = rnd.Next(1, 4);

				for (int i = 0; i < monsterAmount; i++)
				{
					while (true)
					{
						ry = rnd.Next(r.Position[0], r.Position[0]+r.Height);
						rx = rnd.Next(r.Position[1], r.Position[1]+r.Width);

						if (!MonsterAt(ry, rx)) break;
					}

					Monster m = new Monster();
					m.ConstructMonster();
					m.Y = ry;
					m.X = rx;
					Dungeon.Grid[ry][rx].Rep = ConvertChar(m.HealthRep);
					monsters.Add(m);
				}
			}
		}

		/*
			Handles all game updating.
		*/
		public static void Update()
		{
			// Update all entities
			player.Update();
			foreach (var m in monsters)
				m.Update();

			// Update all progress bars
			healthBar.Update();
			attackBar.Update();

			// Update the stats box
			statBox.GenerateStats(GameManager.MainPlayer);

			// Update all input
			Input.Update();
			MovePlayer();

			// Update all combat
			if (CombatExists())
			{
				MonstersAttack();
				PlayerAttack();
			}
		}

		/*
			Loads all content for the game.
		*/
		public static void LoadContent()
		{
			// Stats box
			statBox.LoadContent();
			Vector2 stringSize = ArtManager.DungeonFont.MeasureString(Dungeon.PlayerView())/2;
			dgPos = (screenCenter-stringSize);
			
			// Special Effects
			light.LoadContent();
			light.PositionLight(dgPos);
		}

		/*
			Moves the player in the dungeon using
			the input class, dungeon class, and
			the user's given input.
		*/
		private static void MovePlayer()
		{
			if (player.CanMove)
			{
				Input.GetInput();
				
				switch (Input.CurrentDirection)
				{
					case Input.Direction.NORTH:
						if (Dungeon.CanMove(-1, 0))
						{
							Dungeon.MovePlayer(-1, 0, ConvertChar(player.HealthRep));
							player.Y = Dungeon.PlayerY;
							player.X = Dungeon.PlayerX;
							player.CanMove = false;
						}
						break;

					case Input.Direction.SOUTH:
						if (Dungeon.CanMove(1, 0))
						{
							Dungeon.MovePlayer(1, 0, ConvertChar(player.HealthRep));
							player.Y = Dungeon.PlayerY;
							player.X = Dungeon.PlayerX;
							player.CanMove = false;
						}
						break;

					case Input.Direction.EAST:
						if (Dungeon.CanMove(0, 1))
						{
							Dungeon.MovePlayer(0, 1, ConvertChar(player.HealthRep));
							player.Y = Dungeon.PlayerY;
							player.X = Dungeon.PlayerX;
							player.CanMove = false;
						}
						break;

					case Input.Direction.WEST:
						if (Dungeon.CanMove(0, -1))
						{
							Dungeon.MovePlayer(0, -1, ConvertChar(player.HealthRep));
							player.Y = Dungeon.PlayerY;
							player.X = Dungeon.PlayerX;
							player.CanMove = false;
						}
						break;
				}

				// Reset input variables
				if (Input.CurrentDirection != Input.Direction.NONE)
					Input.LastDirection = Input.CurrentDirection;
				Input.CurrentDirection = Input.Direction.NONE;
			}

		}

		/*
			Returns the correct char relative
			to the integer given.
		*/ 
		public static char ConvertChar(int i)
		{
			return (char)(i+48);
		}
		

		// ======================================
		// =============== Combat ===============
		// ======================================

		/*
			Determines if combat is being exchanged.
		*/
		private static bool CombatExists()
		{
			// Check player's position vs all monsters
			foreach (var m in monsters)
			{
				if ((m.Y == player.Y && m.X == player.X+1) ||
					(m.Y == player.Y && m.X == player.X-1) ||
					(m.Y == player.Y+1 && m.X == player.X) ||
					(m.Y == player.Y-1 && m.X == player.X))
				{
					return true;
				}
			}

			// Default return
			return false;
		}

		/*
			Attacks the player with every possible
			monster.
		*/
		private static void MonstersAttack()
		{
			Monster m;
			for (int i = 0; i < monsters.Count; i++)
			{
				m = monsters[i];

				if ((m.Y == player.Y && m.X == player.X+1) ||
					(m.Y == player.Y && m.X == player.X-1) ||
					(m.Y == player.Y+1 && m.X == player.X) ||
					(m.Y == player.Y-1 && m.X == player.X))
				{
					if (m.CanAttack)
					{
						Combat.MonsterAttack(ref m, ref player);
					}
				}
			}
		}

		/*
			The player attacks the monster that is
			in the current player's direction.
		*/
		private static void PlayerAttack()
		{
			if (player.CanAttack)
			{
				switch (Input.LastDirection)
				{
					case Input.Direction.NORTH:
						if (MonsterAt(player.Y-1, player.X))
						{
							Combat.PlayerAttack(ref player, ref target);
							if (MonsterDied())
								KillMonster();
						}
						break;
					case Input.Direction.SOUTH:
						if (MonsterAt(player.Y+1, player.X))
						{ 
							Combat.PlayerAttack(ref player, ref target);
							if (MonsterDied())
								KillMonster();
						}
						break;
					case Input.Direction.EAST:
						if (MonsterAt(player.Y, player.X+1))
						{
							Combat.PlayerAttack(ref player, ref target);
							if (MonsterDied())
								KillMonster();
						}
						break;
					case Input.Direction.WEST:
						if (MonsterAt(player.Y, player.X-1))
						{ 
							Combat.PlayerAttack(ref player, ref target);
							if (MonsterDied())
								KillMonster();
						}
						break;
				}
			}
		}

		/*
			Determines whether or not the monster
			that was attacked by the player was killed.
		*/
		private static bool MonsterDied()
		{
			if (target.Health == 0)
				return true;
			else return false;
		}

		/*
			Kills a monster and takes away its data
			from the dungeon and GameManager.
		*/
		private static void KillMonster()
		{
			monsters.Remove(target);
			Dungeon.Grid[target.Y][target.X].Rep = Dungeon.Grid[target.Y][target.X].Type;
		}

		/*
			Determines if a monster exists at those
			coordinates.
		*/
		private static bool MonsterAt(int y, int x)
		{
			foreach (var m in monsters)
			{
				if (m.Y == y && m.X == x)
				{
					target = m;
					return true;
				}
			}
			return false;
		}

		// ======================================
		// ============== Drawing ===============
		// ======================================

		/*
			Handles all game drawing.
		*/
		public static void Draw(SpriteBatch sb)
		{
			DrawDungeon(sb);
			light.DrawLight(sb);
			statBox.DrawStats(sb);
			healthBar.DrawProgressBar(sb);
			attackBar.DrawProgressBar(sb);
		}

		/*
			Draws the player's view of the dungeon.
		*/
		private static void DrawDungeon(SpriteBatch sb)
		{
			sb.DrawString(ArtManager.DungeonFont,
						  Dungeon.PlayerView(),
						  dgPos,
						  Color.White);
		}
	}
}
