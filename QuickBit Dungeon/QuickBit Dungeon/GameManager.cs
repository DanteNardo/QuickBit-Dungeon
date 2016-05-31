using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		
		private static Player player;
		private static List<Monster> monsters;
		private static Monster target;
		private static Random rnd;

		// ======================================
		// ============== Methods ===============
		// ======================================

		/*
			Initializes internal data.
		*/
		public static void Init()
		{
			player   = new Player();
			monsters = new List<Monster>();
			rnd		 = new Random();
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

		/*
			Returns the correct char relative
			to the integer given.
		*/ 
		public static char ConvertChar(int i)
		{
			return (char)(i+48);
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
	}
}
