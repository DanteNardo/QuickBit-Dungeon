using System;

namespace QuickBit_Dungeon.CORE
{
	public class Monster : Entity
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		// Timing variables
		private const int MaxMoveTime   = 30;
		private int _moveTime           = 30;
		private const int MaxAttackTime = 40;
		private int _attackTime         = 40;

		public bool CanAttack { get; internal set; }
		public bool CanMove { get; internal set; }

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
			CanAttack = true;
			CanMove = true;
		}

		/// <summary>
		/// Updates a Monster
		/// </summary>
		public void Update()
		{
			// Handle move time
			if (!CanMove) _moveTime--;
			if (_moveTime == 0)
			{
				_moveTime = MaxMoveTime;
				CanMove  = true;
			}

			// Handle attack time
			if (!CanAttack) _attackTime--;
			if (_attackTime == 0)
			{
				_attackTime = MaxAttackTime;
				CanAttack  = true;
			}
		}

		/// <summary>
		/// AI, causes monster to move around
		/// </summary>
		public void Wander()
		{
			
		}

		/// <summary>
		/// AI, causes monster to move towards
		/// the player.
		/// </summary>
		public void HuntPlayer()
		{
			
		}
	}
}
