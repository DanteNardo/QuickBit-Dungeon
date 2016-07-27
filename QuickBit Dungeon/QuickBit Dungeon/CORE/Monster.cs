using System;
using QuickBit_Dungeon.DUNGEON;
using QuickBit_Dungeon.INTERACTION;
using QuickBit_Dungeon.MANAGERS;

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

		public EMonsterState MonsterState { get; set; } = EMonsterState.Wander;
		public enum EMonsterState
		{
			Attack,
			Hunt,
			Wander
		}

		private int Sight { get; set; } = 5;
		private int Range { get; set; } = 15;

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

			switch (MonsterState)
			{
				case EMonsterState.Attack:
					AttackPlayer();
					break;
				case EMonsterState.Hunt:
					HuntPlayer();
					break;
				case EMonsterState.Wander:
					Wander();
					break;
			}
		}

		/// <summary>
		/// Determines what state this monster
		/// should currently be in.
		/// </summary>
		public void CheckState()
		{
			if (PlayerInSight())
				MonsterState = EMonsterState.Attack;
			else if (PlayerInRange())
				MonsterState = EMonsterState.Hunt;
			else
				MonsterState = EMonsterState.Wander;
		}

		private bool PlayerInSight()
		{
			return false;
		}

		private bool PlayerInRange()
		{
			return false;
		}

		/// <summary>
		/// AI, causes monster to move around
		/// </summary>
		public void Wander()
		{
			RegenHealth();

			if (MoveTimer.ActionReady &&
				GameManager.Random.Next(0, 100) == 0)
			{
				MoveRandomOne();
			}
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

		/// <summary>
		/// Moves the monster one unit in a
		/// random direction.
		/// </summary>
		private void MoveRandomOne()
		{
			var result = GameManager.Random.Next(0, 4);
			switch (result)
			{
				case 0:
					if (Dungeon.CanMove(this, 1, 0) &&
						Dungeon.IsARoom(Y+1, X))
						Dungeon.MoveEntity(this, 1, 0);
					MoveTimer.PerformAction();
					break;
				case 1:
					if (Dungeon.CanMove(this, -1, 0) &&
						Dungeon.IsARoom(Y-1, X))
						Dungeon.MoveEntity(this, -1, 0);
					MoveTimer.PerformAction();
					break;
				case 2:
					if (Dungeon.CanMove(this, 0, 1) &&
						Dungeon.IsARoom(Y, X+1))
						Dungeon.MoveEntity(this, 0, 1);
					MoveTimer.PerformAction();
					break;
				case 3:
					if (Dungeon.CanMove(this, 0, -1) &&
						Dungeon.IsARoom(Y, X-1))
						Dungeon.MoveEntity(this, 0, -1);
					MoveTimer.PerformAction();
					break;
			}
		}
	}
}
