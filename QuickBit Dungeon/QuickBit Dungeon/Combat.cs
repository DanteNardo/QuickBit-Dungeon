using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickBit_Dungeon
{
	/*
		Handles all combat functions 
		between two entities.
	*/
	public static class Combat
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		
		private static Player player;
		private static Monster target;
		private static List<Monster> targets;
		private static List<Monster> monsters;

		// ======================================
		// ============== Methods ===============
		// ======================================

		/*
			Overall combat handling method.
		*/
		public static void PerformCombat(Player player, List<Monster> monsters)
		{
			// Set data
			Combat.player = player;
			Combat.monsters = monsters;
			targets = new List<Monster>();

			// Player combat
			switch (Input.PlayerAttack)
			{
				case Input.Attack.PHYSICAL:
					PlayerCombat();
					break;
				case Input.Attack.SPECIAL:
					PlayerCombat();
					break;
			}

			// Monster combat
			MonstersCombat();

			// All combat processing finished
			EndCombat();
		}

		/*
			The player attacks the monster that is
			in the current player's direction.
		*/
		private static void PlayerCombat()
		{
			if (player.CanAttack)
			{
				switch (Input.LastDirection)
				{
					case Input.Direction.NORTH:
						if (GameManager.MonsterAt(player.Y-1, player.X, ref target) &&
							Input.PlayerAttack != Input.Attack.NONE)
						{
							PlayerAttack();
							if (MonsterDied())
								KillMonster();
						}
						break;
					case Input.Direction.SOUTH:
						if (GameManager.MonsterAt(player.Y+1, player.X, ref target) &&
							Input.PlayerAttack != Input.Attack.NONE)
						{
							PlayerAttack();
							if (MonsterDied())
								KillMonster();
						}
						break;
					case Input.Direction.EAST:
						if (GameManager.MonsterAt(player.Y, player.X+1, ref target) &&
							Input.PlayerAttack != Input.Attack.NONE)
						{
							PlayerAttack();
							if (MonsterDied())
								KillMonster();
						}
						break;
					case Input.Direction.WEST:
						if (GameManager.MonsterAt(player.Y, player.X-1, ref target) &&
							Input.PlayerAttack != Input.Attack.NONE)
						{
							PlayerAttack();
							if (MonsterDied())
								KillMonster();
						}
						break;
				}
			}

			// Reset player variables
			Input.PlayerAttack = Input.Attack.NONE;
		}

		/*
			Attacks the player with every possible
			monster.
		*/
		private static void MonstersCombat()
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
						MonsterAttack(ref m);
					}
				}
			}
		}

		/*
			Determines the player's attack and
			executes the correct method in response.
		*/
		private static void PlayerAttack()
		{
			switch (Input.PlayerAttack)
			{
				case Input.Attack.PHYSICAL:
					PlayerPhysicalAttack();
					break;
				case Input.Attack.SPECIAL:
					PlayerSpecialAttack();
					break;
			}

			
		}

		/*
			Executes a player's attack on 
			a monster's health.
		*/
		private static void PlayerPhysicalAttack()
		{
			int damage = player.Strength - target.Armor;
			target.Health -= damage;
			target.CalculateHealthRep();
			Dungeon.Grid[target.Y][target.X].Rep = GameManager.ConvertChar(target.HealthRep);
			player.CanAttack = false;
		}

		/*
			Executes a player's special attack
			on every adjacent monster's health.
		*/
		private static void PlayerSpecialAttack()
		{
			// Determine targets
			targets.Clear();
			SpecialAttackTargets();

			// Perform special attack
			for (int i = 0; i < targets.Count; i++)
			{
				int damage = player.Wisdom + GameManager.Rand.Next(1, player.Wisdom*10);
				targets[i].Health -= damage;
				targets[i].CalculateHealthRep();
				Dungeon.Grid[targets[i].Y][targets[i].X].Rep = GameManager.ConvertChar(targets[i].HealthRep);
				player.AttackMana -= player.ManaCost;
				player.CanAttack = false;
			}
		}

		/*
			Generates a list of valid targets
			that would be hit by the special attack.
		*/
		private static void SpecialAttackTargets()
		{
			Monster m;
			for (int i = 0; i < monsters.Count; i++)
			{
				m = monsters[i];

				if ((m.Y == player.Y && m.X == player.X + 1) ||
					(m.Y == player.Y && m.X == player.X - 1) ||
					(m.Y == player.Y + 1 && m.X == player.X) ||
					(m.Y == player.Y - 1 && m.X == player.X) ||
					(m.Y == player.Y + 1 && m.X == player.X + 1) ||
					(m.Y == player.Y + 1 && m.X == player.X - 1) ||
					(m.Y == player.Y - 1 && m.X == player.X + 1) ||
					(m.Y == player.Y - 1 && m.X == player.X - 1))
				{
					targets.Add(m);
				}
			}
		}

		/*
			Executes a monster's attack on
			a player's health.
		*/
		private static void MonsterAttack(ref Monster m)
		{
			int damage = target.Strength - player.Armor;
			player.Health -= damage;
			player.CalculateHealthRep();
			Dungeon.Grid[player.Y][player.X].Rep = GameManager.ConvertChar(player.HealthRep);
			target.CanAttack = false;
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
			Sets the GameManagers values to the
			post combat player and monster list values.
		*/
		private static void EndCombat()
		{
			GameManager.MainPlayer = player;
			GameManager.Monsters   = monsters;
		}
	}
}
