using System.Collections.Generic;
using QuickBit_Dungeon.CORE;
using QuickBit_Dungeon.DUNGEON;
using QuickBit_Dungeon.MANAGERS;

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
					if (Dungeon.MonsterAt(_player.Y-1, _player.X, ref _target))
					{
						PlayerAttack();
						if (MonsterDied())
							KillMonster();
					}
					break;
				case Input.Direction.South:
					if (Dungeon.MonsterAt(_player.Y+1, _player.X, ref _target))
					{
						PlayerAttack();
						if (MonsterDied())
							KillMonster();
					}
					break;
				case Input.Direction.East:
					if (Dungeon.MonsterAt(_player.Y, _player.X+1, ref _target))
					{
						PlayerAttack();
						if (MonsterDied())
							KillMonster();
					}
					break;
				case Input.Direction.West:
					if (Dungeon.MonsterAt(_player.Y, _player.X-1, ref _target))
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
				if (m.AttackTimer.ActionReady)
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
		    AudioManager.NewPlayerHit();
			var damage = _player.Strength - _target.Armor;
			if (_player.IsCrit()) damage = _player.Strength*2;
		    if (damage < 0) damage = 0;
			_target.UpdateHealth(-damage);
		    _target.AttackColor();
			Dungeon.ResetRep(_target);
		}

		/// <summary>
		/// Executes a player's special attack
		///	on every adjacent monster's health.
		/// </summary>
		private void PlayerSpecialAttack()
		{
            // Play Sound
		    AudioManager.NewPlayerSpecial();

			// Determine targets
			_targets.Clear();
			SpecialAttackTargets();

			// Perform special attack
			foreach (var t in _targets)
			{
				var damage = _player.Wisdom + GameManager.Random.Next(1, _player.Wisdom*10);
				t.UpdateHealth(-damage);
			    t.AttackColor();
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
		    AudioManager.NewBitHit();
			m.AttackTimer.PerformAction();
			var damage = m.Strength - _player.Armor;
		    if (damage < 0) damage = 0;
			_player.UpdateHealth(-damage);
		    _player.AttackColor();
			Dungeon.ResetRep(_player);
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
		    Dungeon.KillEntity(_target);
		}

		/// <summary>
		/// Sets the GameManagers values to the
		///	post combat player and monster list values.
		/// </summary>
		private void EndCombat()
		{
			Dungeon.MainPlayer = _player;
			Dungeon.Monsters   = _monsters;
		}
	}
}
