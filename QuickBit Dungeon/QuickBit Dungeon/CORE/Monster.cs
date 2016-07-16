using System;
using QuickBit_Dungeon.INTERACTION;

namespace QuickBit_Dungeon.CORE
{
	public class Monster : Entity
	{
		// ======================================
		// ============== Members ===============
		// ======================================

		// Timing variables
		public Timer AttackTimer { get; set; }
		public Timer MoveTimer { get; set; }

		public EMonsterState MonsterState { get; set; }
		public enum EMonsterState
		{
			Attack,
			Hunt,
			Wander
		}

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Constructor
		/// </summary>
		public Monster() : base()
		{
			AttackTimer = new Timer(60);
			MoveTimer = new Timer(80);
		}

		/// <summary>
		/// Updates a Monster
		/// </summary>
		public new void Update()
		{
			base.Update();
			AttackTimer.Update();
			MoveTimer.Update();
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
