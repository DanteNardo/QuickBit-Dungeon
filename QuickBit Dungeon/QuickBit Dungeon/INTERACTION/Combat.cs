﻿using System.Collections.Generic;
using QuickBit_Dungeon.CORE;
using QuickBit_Dungeon.DUNGEON;

namespace QuickBit_Dungeon.INTERACTION
{
	/// <summary>
	/// Handles all combat functions 
	///	between two entities.
	/// </summary>
	public class Combat
	{
		// ======================================
		// ============== Members ===============
		// ======================================
		
		private static Player _player;
		private static Monster _target;
		private static List<Monster> _targets;
		private static List<Monster> _monsters;

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Overall combat handling method.
		/// </summary>
		/// <param name="player">Player object</param>
		/// <param name="monsters">List of monsters in current level</param>
		public void PerformCombat(Player player, List<Monster> monsters)
		{
			// Set data
			_player = player;
			_monsters = monsters;
			_targets = new List<Monster>();

			// Player combat
			PlayerCombat();

			// Monster combat
			MonstersCombat();

			// All combat processing finished
			EndCombat();
		}

		/// <summary>
		/// The player attacks the monster that is
		///	in the current player's direction.
		/// </summary>
		private void PlayerCombat()
		{
			if (Input.PlayerState == Input.EPlayerState.None) return;
			switch (Input.LastDirection)
			{
				case Input.Direction.North:
					if (World.MonsterAt(_player.Y-1, _player.X, ref _target))
					{
						PlayerAttack();
						if (MonsterDied())
							KillMonster();
					}
					break;
				case Input.Direction.South:
					if (World.MonsterAt(_player.Y+1, _player.X, ref _target))
					{
						PlayerAttack();
						if (MonsterDied())
							KillMonster();
					}
					break;
				case Input.Direction.East:
					if (World.MonsterAt(_player.Y, _player.X+1, ref _target))
					{
						PlayerAttack();
						if (MonsterDied())
							KillMonster();
					}
					break;
				case Input.Direction.West:
					if (World.MonsterAt(_player.Y, _player.X-1, ref _target))
					{
						PlayerAttack();
						if (MonsterDied())
							KillMonster();
					}
					break;
			}
		}

		/// <summary>
		/// Attacks the player with every possible
		///	monster.
		/// </summary>
		private void MonstersCombat()
		{
			foreach (var t in _monsters)
			{
				var m = t;

				if ((m.Y != _player.Y || m.X != _player.X + 1) && (m.Y != _player.Y || m.X != _player.X - 1) &&
				    (m.Y != _player.Y + 1 || m.X != _player.X) && (m.Y != _player.Y - 1 || m.X != _player.X)) continue;
				if (m.CanAttack)
					MonsterAttack(ref m);
			}
		}

		/// <summary>
		/// Determines the player's attack and
		///	executes the correct method in response.
		/// </summary>
		private void PlayerAttack()
		{
			switch (Input.PlayerState)
			{
				case Input.EPlayerState.Physical:
					PlayerPhysicalAttack();
					break;
				case Input.EPlayerState.Special:
					if (_player.CanSpecial())
						PlayerSpecialAttack();
					break;
			}
		}

		/// <summary>
		/// Determines if a player is currently
		///	regenerating health or mana.
		/// </summary>
		public void PlayerRegen()
		{
			switch (Input.PlayerState)
			{
				case Input.EPlayerState.Healing:
					if (_player.CanHeal())
						_player.RegenerateHealth();
					break;
				case Input.EPlayerState.Charging:
					_player.RegenerateMana();
					break;
			}

			Input.PlayerState = Input.EPlayerState.None;
		}

		/// <summary>
		/// Executes a player's attack on 
		///	a monster's health.
		/// </summary>
		private void PlayerPhysicalAttack()
		{
			var damage = _player.Strength - _target.Armor;
			if (_player.IsCrit())
				damage *= 2;
			_target.UpdateHealth(-damage);
			_target.CalculateHealthRep();
			Dungeon.ResetRep(_target);
		}

		/// <summary>
		/// Executes a player's special attack
		///	on every adjacent monster's health.
		/// </summary>
		private void PlayerSpecialAttack()
		{
			// Determine targets
			_targets.Clear();
			SpecialAttackTargets();

			// Perform special attack
			foreach (var t in _targets)
			{
				var damage = _player.Wisdom + World.Rand.Next(1, _player.Wisdom*10);
				t.UpdateHealth(-damage);
				t.CalculateHealthRep();
				Dungeon.ResetRep(t);
			}
			
			// Take away mana from player for the attack
			_player.AttackMana -= _player.ManaCost;
		}

		/// <summary>
		/// Generates a list of valid targets
		///	that would be hit by the special attack.
		/// </summary>
		private void SpecialAttackTargets()
		{
			foreach (var m in _monsters)
			{
				if ((m.Y == _player.Y && m.X == _player.X + 1) ||
				    (m.Y == _player.Y && m.X == _player.X - 1) ||
				    (m.Y == _player.Y + 1 && m.X == _player.X) ||
				    (m.Y == _player.Y - 1 && m.X == _player.X) ||
				    (m.Y == _player.Y + 1 && m.X == _player.X + 1) ||
				    (m.Y == _player.Y + 1 && m.X == _player.X - 1) ||
				    (m.Y == _player.Y - 1 && m.X == _player.X + 1) ||
				    (m.Y == _player.Y - 1 && m.X == _player.X - 1))
				{
					_targets.Add(m);
				}
			}
		}

		/// <summary>
		/// Executes a monster's attack on
		///	a player's health.
		/// </summary>
		/// <param name="m">The monster that is currently attacking</param>
		private void MonsterAttack(ref Monster m)
		{
			var damage = m.Strength - _player.Armor;
			_player.UpdateHealth(-damage);
			_player.CalculateHealthRep();
			Dungeon.ResetRep(_player);
			m.CanAttack = false;
		}

		/// <summary>
		/// Determines whether or not the monster
		///	that was attacked by the player was killed.
		/// </summary>
		/// <returns>Whether or not the monster last attacked died</returns>
		private bool MonsterDied()
		{
			return _target.Health == 0;
		}

		/// <summary>
		/// Kills a monster and takes away its data
		///	from the dungeon and World.
		/// </summary>
		private void KillMonster()
		{
			// Add exp to the player
			_player.Xp += _target.Xp;
			_monsters.Remove(_target);
			Dungeon.Grid[_target.Y][_target.X].Rep = Dungeon.Grid[_target.Y][_target.X].Type;
		}

		/// <summary>
		/// Sets the GameManagers values to the
		///	post combat player and monster list values.
		/// </summary>
		private void EndCombat()
		{
			World.MainPlayer = _player;
			World.Monsters   = _monsters;
		}
	}
}
