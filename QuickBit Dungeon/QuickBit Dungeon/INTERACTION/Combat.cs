﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickBit_Dungeon
{
	/*
		Handles all combat functions 
		between two entities.
	*/
	public class Combat
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
		public void PerformCombat(Player player, List<Monster> monsters)
		{
			// Set data
			Combat.player = player;
			Combat.monsters = monsters;
			targets = new List<Monster>();

			// Player combat
			PlayerCombat();

			// Monster combat
			MonstersCombat();

			// All combat processing finished
			EndCombat();
		}

		/*
			The player attacks the monster that is
			in the current player's direction.
		*/
		private void PlayerCombat()
		{
			if (player.CanAttack)
			{
				switch (Input.LastDirection)
				{
					case Input.Direction.NORTH:
						if (GameManager.MonsterAt(player.Y-1, player.X, ref target) &&
							Input.PlayerState != Input.ePlayerState.NONE)
						{
							PlayerAttack();
							if (MonsterDied())
								KillMonster();
						}
						break;
					case Input.Direction.SOUTH:
						if (GameManager.MonsterAt(player.Y+1, player.X, ref target) &&
							Input.PlayerState != Input.ePlayerState.NONE)
						{
							PlayerAttack();
							if (MonsterDied())
								KillMonster();
						}
						break;
					case Input.Direction.EAST:
						if (GameManager.MonsterAt(player.Y, player.X+1, ref target) &&
							Input.PlayerState != Input.ePlayerState.NONE)
						{
							PlayerAttack();
							if (MonsterDied())
								KillMonster();
						}
						break;
					case Input.Direction.WEST:
						if (GameManager.MonsterAt(player.Y, player.X-1, ref target) &&
							Input.PlayerState != Input.ePlayerState.NONE)
						{
							PlayerAttack();
							if (MonsterDied())
								KillMonster();
						}
						break;
				}
			}

			// Reset player variables
			Input.PlayerState = Input.ePlayerState.NONE;
		}

		/*
			Attacks the player with every possible
			monster.
		*/
		private void MonstersCombat()
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
		private void PlayerAttack()
		{
			switch (Input.PlayerState)
			{
				case Input.ePlayerState.PHYSICAL:
					PlayerPhysicalAttack();
					break;
				case Input.ePlayerState.SPECIAL:
					if (player.CanSpecial())
						PlayerSpecialAttack();
					break;
			}
		}

		/*
			Determines if a player is currently
			regenerating health or mana.
		*/
		public void PlayerRegen()
		{
			switch (Input.PlayerState)
			{
				case Input.ePlayerState.HEALING:
					if (player.CanHeal())
						player.RegenerateHealth();
					break;
				case Input.ePlayerState.CHARGING:
					player.RegenerateMana();
					break;
			}
			
			Input.PlayerState = Input.ePlayerState.NONE;
		}

		/*
			Executes a player's attack on 
			a monster's health.
		*/
		private void PlayerPhysicalAttack()
		{
			int damage = player.Strength - target.Armor;
			target.Health -= damage;
			target.CalculateHealthRep();
			Dungeon.Grid[target.Y][target.X].Rep = GameManager.ConvertToChar(target.HealthRep);
			player.CanAttack = false;
		}

		/*
			Executes a player's special attack
			on every adjacent monster's health.
		*/
		private void PlayerSpecialAttack()
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
				Dungeon.Grid[targets[i].Y][targets[i].X].Rep = GameManager.ConvertToChar(targets[i].HealthRep);
			}
			
			player.AttackMana -= player.ManaCost;
			player.CanAttack = false;
		}

		/*
			Generates a list of valid targets
			that would be hit by the special attack.
		*/
		private void SpecialAttackTargets()
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
		private void MonsterAttack(ref Monster m)
		{
			int damage = m.Strength - player.Armor;
			player.Health -= damage;
			player.CalculateHealthRep();
			Dungeon.Grid[player.Y][player.X].Rep = GameManager.ConvertToChar(player.HealthRep);
			m.CanAttack = false;
		}

		/*
			Determines whether or not the monster
			that was attacked by the player was killed.
		*/
		private bool MonsterDied()
		{
			if (target.Health == 0)
				return true;
			else return false;
		}

		/*
			Kills a monster and takes away its data
			from the dungeon and GameManager.
		*/
		private void KillMonster()
		{
			// Add exp to the player
			player.XP += target.XP;
			monsters.Remove(target);
			Dungeon.Grid[target.Y][target.X].Rep = Dungeon.Grid[target.Y][target.X].Type;
		}

		/*
			Sets the GameManagers values to the
			post combat player and monster list values.
		*/
		private void EndCombat()
		{
			GameManager.MainPlayer = player;
			GameManager.Monsters   = monsters;
		}
	}
}