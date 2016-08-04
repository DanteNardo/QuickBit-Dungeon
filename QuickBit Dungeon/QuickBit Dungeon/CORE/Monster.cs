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
			MoveTimer = new Timer(30);
		}

		/// <summary>
		/// Updates a Monster
		/// </summary>
		public new void Update()
		{
			base.Update();
			AttackTimer.Update();
			MoveTimer.Update();

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
			if (PlayerInRange())
				MonsterState = EMonsterState.Attack;
			else
				MonsterState = EMonsterState.Wander;
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
			MoveToLowestWeight();
		}

		/// <summary>
		/// AI, causes monster to attack
		/// the player.
		/// </summary>
		public void AttackPlayer()
		{
			MoveToLowestWeight();
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
		/// Moves the current monster to the
		/// lowest neighboring weight.
		/// </summary>
		private void MoveToLowestWeight()
		{
			if (!MoveTimer.ActionReady)
				return;

			var lowest = 10000;
			var pos = new int[2];
			foreach (var n in Dungeon.Grid[Y][X].Neighbors)
				if (Dungeon.Grid[n[0]][n[1]].Weight != 0 &&
					Dungeon.Grid[n[0]][n[1]].Weight < lowest)
				{
					lowest = Dungeon.Grid[n[0]][n[1]].Weight;
					pos = n;
				}

			if (Dungeon.CanMove(this, pos[0] - Y, pos[1] - X))
			{
				Dungeon.MoveEntity(this, pos[0] - Y, pos[1] - X);
				MoveTimer.PerformAction();
			}
		}

		#region Outdated

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
			{
				if (MoveInYDirection(LastSeenPlayer.Item1) == false)
					MoveInXDirection(LastSeenPlayer.Item2);
			}
			else if (diffY < diffX && MoveTimer.ActionReady)
			{
				if (MoveInXDirection(LastSeenPlayer.Item2) == false)
					MoveInYDirection(LastSeenPlayer.Item1);
			}

			// If equally weighted, move in y direction (default)
			else if (MoveTimer.ActionReady)
			{
				if (MoveInYDirection(LastSeenPlayer.Item1) == false)
					MoveInXDirection(LastSeenPlayer.Item2);
			}
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
			{
				if (MoveInYDirection(Dungeon.MainPlayer.Y) == false)
					MoveInXDirection(Dungeon.MainPlayer.X);
			}
			else if (diffY < diffX && MoveTimer.ActionReady)
			{
				if (MoveInXDirection(Dungeon.MainPlayer.X) == false)
					MoveInYDirection(Dungeon.MainPlayer.Y);
			}

			// If equally weighted, move in y direction (default)
			else if (MoveTimer.ActionReady)
			{
				if (MoveInYDirection(Dungeon.MainPlayer.Y) == false)
					MoveInXDirection(Dungeon.MainPlayer.X);
			}

		}

		/// <summary>
		/// Moves the monster in the y direction.
		/// </summary>
		/// <param name="pointY">The point to move towards</param>
		/// <returns>Whether or not the movement was successful</returns>
		private bool MoveInYDirection(int pointY)
		{
			if (pointY - Y > 0 && Dungeon.CanMove(this, 1, 0))
			{
				Dungeon.MoveEntity(this, 1, 0);
				MoveTimer.PerformAction();
				return true;
			}
			if (pointY - Y < 0 && Dungeon.CanMove(this, -1, 0))
			{
				Dungeon.MoveEntity(this, -1, 0);
				MoveTimer.PerformAction();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Moves the monster in the x direction.
		/// </summary>
		/// <param name="pointX">The point to move towards</param>
		/// <returns>Whether or not the movement was successful</returns>
		private bool MoveInXDirection(int pointX)
		{
			if (pointX - X > 0 && Dungeon.CanMove(this, 0, 1))
			{
				Dungeon.MoveEntity(this, 0, 1);
				MoveTimer.PerformAction();
				return true;
			}
			if (pointX - X < 0 && Dungeon.CanMove(this, 0, -1))
			{
				Dungeon.MoveEntity(this, 0, -1);
				MoveTimer.PerformAction();
				return true;
			}
			return false;
		}

		#endregion
	}
}
