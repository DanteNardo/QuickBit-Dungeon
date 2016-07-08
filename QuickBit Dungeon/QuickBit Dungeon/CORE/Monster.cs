using System;
using QuickBit_Dungeon.INTERACTION;

namespace QuickBit_Dungeon.CORE
{
	public class Monster : Entity
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		// Timing variables
		public Timer _attackTimer;
		public Timer _moveTimer;

		public EMonsterState MonsterState { get; set; }
		public enum EMonsterState
		{
			Attack,
			Hunt,
			Idle
		}

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Constructor
		/// </summary>
		public Monster() : base()
		{
			_attackTimer = new Timer(30);
			_moveTimer = new Timer(40);
		}

		/// <summary>
		/// Updates a Monster
		/// </summary>
		public void Update()
		{
			_attackTimer.Update();
			_moveTimer.Update();
		}

		/// <summary>
		/// AI, causes monster to move around
		/// </summary>
		public void Wander()
		{
			RegenHealth();
		}

		/// <summary>
		/// AI, causes monster to move towards
		/// the player.
		/// </summary>
		public void HuntPlayer()
		{
			RegenHealth();
		}

		/// <summary>
		/// AI, causes monster to attack
		/// the player.
		/// </summary>
		public void AttackPlayer()
		{
			
		}

		/// <summary>
		/// Passively regenerates health based
		/// off of the Monster's Wisdom.
		/// </summary>
		private void RegenHealth()
		{
			
		}
	}
}
