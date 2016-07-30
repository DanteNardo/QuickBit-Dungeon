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

		public Timer AttackTimer { get; set; }
		public Timer MoveTimer { get; set; }

		public EMonsterState MonsterState { get; set; } = EMonsterState.Wander;
		public enum EMonsterState
		{
			Attack,
			Hunt,
			Wander
		}
		
		private Tuple<int, int> LastSeenPlayer { get; set; }
		private int Sight { get; set; } = 3;
		private int Range { get; set; } = 10;

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Constructor
		/// </summary>
		public Monster() : base()
		{
			AttackTimer = new Timer(60);
			MoveTimer = new Timer(90);
			LastSeenPlayer = new Tuple<int, int>(0, 0);
		}

		/// <summary>
		/// Updates a Monster
		/// </summary>
		public new void Update()
		{
			base.Update();
			AttackTimer.Update();
			MoveTimer.Update();

			LookForPlayer();
			CheckState();
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
		private void CheckState()
		{
			if (PlayerInSight())
				MonsterState = EMonsterState.Attack;
			else if (PlayerInRange())
				MonsterState = EMonsterState.Hunt;
			else
				MonsterState = EMonsterState.Wander;
		}

		/// <summary>
		/// If the player is in sight, the
		/// monster remembers their position.
		/// Used for movement.
		/// </summary>
		private void LookForPlayer()
		{
			if (PlayerInSight())
				LastSeenPlayer = new Tuple<int, int>(Dungeon.MainPlayer.Y, Dungeon.MainPlayer.X);
		}

		/// <summary>
		/// Returns whether or not the player
		/// is in the monster's line of sight.
		/// </summary>
		/// <returns>Whether the player is in sight or not</returns>
		private bool PlayerInSight()
		{
			return  Dungeon.MainPlayer.Y >= Y - Sight &&
					Dungeon.MainPlayer.Y <= Y + Sight &&
					Dungeon.MainPlayer.X >= X - Sight &&
					Dungeon.MainPlayer.X <= X + Sight;
		}

		/// <summary>
		/// Returns whether or not the player
		/// is in the monster's range.
		/// </summary>
		/// <returns>Whether the player is in range or not</returns>
		private bool PlayerInRange()
		{
			return  Dungeon.MainPlayer.Y >= Y - Range &&
					Dungeon.MainPlayer.Y <= Y + Range &&
					Dungeon.MainPlayer.X >= X - Range &&
					Dungeon.MainPlayer.X <= X + Range;
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
			if (PlayerInRange())
				MoveTowardPlayer();
		}

		/// <summary>
		/// AI, causes monster to attack
		/// the player.
		/// </summary>
		public void AttackPlayer()
		{
			if (PlayerInSight())
				MoveToPlayer();
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

		/// <summary>
		/// Moves the monster in the last
		/// direction that they saw the player
		/// followed by random directions until
		/// the player is no longer in range.
		/// </summary>
		private void MoveTowardPlayer()
		{
			// Find if there is a larger y or x difference
			// between the monster's position and the player's.
			// Whichever difference is larger is the the direction
			// that the monster moves in.
			var diffY = Math.Abs(LastSeenPlayer.Item1 - Y);
			var diffX = Math.Abs(LastSeenPlayer.Item2 - X);

			if (diffY > diffX && MoveTimer.ActionReady)
				MoveInYDirection(LastSeenPlayer.Item1);
			else if (diffY < diffX && MoveTimer.ActionReady)
				MoveInXDirection(LastSeenPlayer.Item2);
			else
				MoveInYDirection(LastSeenPlayer.Item1);
		}

		/// <summary>
		/// Moves the monster towards
		/// the player's next location.
		/// If they aren't there by the time
		/// you have moved towards it,
		/// move towards their exact location.
		/// </summary>
		private void MoveToPlayer()
		{
			// Find if there is a larger y or x difference
			// between the monster's position and the player's.
			// Whichever difference is larger is the the direction
			// that the monster moves in.
			var diffY = Math.Abs(Dungeon.MainPlayer.Y - Y);
			var diffX = Math.Abs(Dungeon.MainPlayer.X - X);

			if (diffY > diffX && MoveTimer.ActionReady)
				MoveInYDirection(Dungeon.MainPlayer.Y);
			else if (diffY < diffX && MoveTimer.ActionReady)
				MoveInXDirection(Dungeon.MainPlayer.X);
			else
				MoveInYDirection(Dungeon.MainPlayer.Y);
		}

		/// <summary>
		/// Moves the monster in the y direction.
		/// </summary>
		private void MoveInYDirection(int pointY)
		{
			if (pointY - Y > 0 && Dungeon.CanMove(this, 1, 0))
			{
				Dungeon.MoveEntity(this, 1, 0);
				MoveTimer.PerformAction();
			}
			else if (pointY - Y < 0 && Dungeon.CanMove(this, -1, 0))
			{
				Dungeon.MoveEntity(this, -1, 0);
				MoveTimer.PerformAction();
			}
		}

		/// <summary>
		/// Moves the monster in the x direction.
		/// </summary>
		private void MoveInXDirection(int pointX)
		{
			if (pointX - X > 0 && Dungeon.CanMove(this, 0, 1))
			{
				Dungeon.MoveEntity(this, 0, 1);
				MoveTimer.PerformAction();
			}
			else if (pointX - X < 0 && Dungeon.CanMove(this, 0, -1))
			{
				Dungeon.MoveEntity(this, 0, -1);
				MoveTimer.PerformAction();
			}
		}
	}
}
