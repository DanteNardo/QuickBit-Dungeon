using System.Collections.Generic;
using QuickBit_Dungeon.Characters;
using QuickBit_Dungeon.DungeonGeneration;
using QuickBit_Dungeon.Managers;
using QuickBit_Dungeon.UI.HUD;

namespace QuickBit_Dungeon.Interaction
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
		
		private static Player m_player;
		private static Monster m_target;
		private static List<Monster> m_targets;
		private static List<Monster> m_monsters;

		// ======================================
		// ============== Methods ===============
		// ======================================

		#region Combat Methods

		/// <summary>
		/// Overall combat handling method.
		/// </summary>
		/// <param name="player">Player object</param>
		/// <param name="monsters">List of monsters in current level</param>
		public void PerformCombat(Player player, List<Monster> monsters)
		{
			// Set data
			m_player = player;
			m_monsters = monsters;
			m_targets = new List<Monster>();

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
					if (Dungeon.MonsterAt(m_player.Y-1, m_player.X, ref m_target))
					{
						PlayerAttack();
						if (MonsterDied())
							KillMonster();
					}
					break;
				case Input.Direction.South:
					if (Dungeon.MonsterAt(m_player.Y+1, m_player.X, ref m_target))
					{
						PlayerAttack();
						if (MonsterDied())
							KillMonster();
					}
					break;
				case Input.Direction.East:
					if (Dungeon.MonsterAt(m_player.Y, m_player.X+1, ref m_target))
					{
						PlayerAttack();
						if (MonsterDied())
							KillMonster();
					}
					break;
				case Input.Direction.West:
					if (Dungeon.MonsterAt(m_player.Y, m_player.X-1, ref m_target))
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
			foreach (var t in m_monsters)
			{
				var m = t;

				if ((m.Y != m_player.Y || m.X != m_player.X + 1) && (m.Y != m_player.Y || m.X != m_player.X - 1) &&
				    (m.Y != m_player.Y + 1 || m.X != m_player.X) && (m.Y != m_player.Y - 1 || m.X != m_player.X)) continue;
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
					if (m_player.CanSpecial())
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
					if (m_player.CanHeal())
						m_player.RegenerateHealth();
					break;
				case Input.EPlayerState.Charging:
					m_player.RegenerateMana();
					AwardHandler.NewRegen();
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
			var damage = m_player.Strength - m_target.Armor;
			if (m_player.IsCrit()) damage = m_player.Strength*2;
		    if (damage < 0) damage = 0;
			m_target.UpdateHealth(-damage);
		    m_target.AttackColor();
			Dungeon.ResetRep(m_target);
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
			m_targets.Clear();
			SpecialAttackTargets();

			// Perform special attack
			foreach (var t in m_targets)
			{
				var damage = m_player.Wisdom + GameManager.Random.Next(1, m_player.Wisdom*10);
				t.UpdateHealth(-damage);
			    t.AttackColor();
				Dungeon.ResetRep(t);
			}
			
			// Take away mana from player for the attack
			m_player.AttackMana -= m_player.ManaCost;
		}

		/// <summary>
		/// Generates a list of valid targets
		///	that would be hit by the special attack.
		/// </summary>
		private void SpecialAttackTargets()
		{
			foreach (var m in m_monsters)
			{
				if ((m.Y == m_player.Y && m.X == m_player.X + 1) ||
				    (m.Y == m_player.Y && m.X == m_player.X - 1) ||
				    (m.Y == m_player.Y + 1 && m.X == m_player.X) ||
				    (m.Y == m_player.Y - 1 && m.X == m_player.X) ||
				    (m.Y == m_player.Y + 1 && m.X == m_player.X + 1) ||
				    (m.Y == m_player.Y + 1 && m.X == m_player.X - 1) ||
				    (m.Y == m_player.Y - 1 && m.X == m_player.X + 1) ||
				    (m.Y == m_player.Y - 1 && m.X == m_player.X - 1))
				{
					m_targets.Add(m);
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
			var damage = m.Strength - m_player.Armor;
		    if (damage < 0) damage = 0;
			m_player.UpdateHealth(-damage);
		    m_player.AttackColor();
			Dungeon.ResetRep(m_player);
		}

		/// <summary>
		/// Determines whether or not the monster
		///	that was attacked by the player was killed.
		/// </summary>
		/// <returns>Whether or not the monster last attacked died</returns>
		private bool MonsterDied()
		{
			return m_target.Health == 0;
		}

		/// <summary>
		/// Kills a monster and takes away its data
		///	from the dungeon and World.
		/// </summary>
		private void KillMonster()
		{
			// Add xp to the player
			m_player.Xp += m_target.Xp;
			m_monsters.Remove(m_target);
		    Dungeon.KillEntity(m_target);
			AwardHandler.NewKill();
		}

		/// <summary>
		/// Sets the GameManagers values to the
		///	post combat player and monster list values.
		/// </summary>
		private void EndCombat()
		{
			Dungeon.MainPlayer = m_player;
			Dungeon.Monsters   = m_monsters;
		}

		#endregion
	}
}
