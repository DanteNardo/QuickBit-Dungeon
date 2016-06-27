using System;

namespace QuickBit_Dungeon.CORE
{
	public class Monster : Entity
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		// Timing variables
		private int maxMoveTime   = 20;
		private int moveTime      = 20;
		private int maxAttackTime = 10;
		private int attackTime    = 10;

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
			if (!CanMove) moveTime--;
			if (moveTime == 0)
			{
				moveTime = maxMoveTime;
				CanMove  = true;
			}

			// Handle attack time
			if (!CanAttack) attackTime--;
			if (attackTime == 0)
			{
				attackTime = maxAttackTime;
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
