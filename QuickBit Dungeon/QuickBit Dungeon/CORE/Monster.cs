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

		private int Range { get; set; } = 1;

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

		    if (MoveTimer.ActionReady)
		    {
		        if (MoveToLowestWeight()) return;
		        if (GameManager.Random.Next(0, 100) == 0)
		        {
		            RegenHealth();
		            MoveRandomOne();
		        }
		    }
		}

		/// <summary>
		/// Passively regenerates health based
		/// off of the Monster's Wisdom.
		/// </summary>
		private void RegenHealth()
		{
		    Health += Wisdom/10;
		    if (Health > MaxHealth)
		        Health = MaxHealth;
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
						Dungeon.NotAClosedDoor(Y+1, X))
						Dungeon.MoveEntity(this, 1, 0);
					MoveTimer.PerformAction();
					break;
				case 1:
					if (Dungeon.CanMove(this, -1, 0) &&
						Dungeon.NotAClosedDoor(Y-1, X))
						Dungeon.MoveEntity(this, -1, 0);
					MoveTimer.PerformAction();
					break;
				case 2:
					if (Dungeon.CanMove(this, 0, 1) &&
						Dungeon.NotAClosedDoor(Y, X+1))
						Dungeon.MoveEntity(this, 0, 1);
					MoveTimer.PerformAction();
					break;
				case 3:
					if (Dungeon.CanMove(this, 0, -1) &&
						Dungeon.NotAClosedDoor(Y, X-1))
						Dungeon.MoveEntity(this, 0, -1);
					MoveTimer.PerformAction();
					break;
			}
		}

		/// <summary>
		/// Moves the current monster to the
		/// lowest neighboring weight.
		/// </summary>
		private bool MoveToLowestWeight()
		{
            // Find lowest
			var lowest = 10000;
			var pos = new int[2];
            var foundWeight = false;
			foreach (var n in Dungeon.Grid[Y][X].Neighbors)
				if (Dungeon.Grid[n[0]][n[1]].Weight != 0 &&
					Dungeon.Grid[n[0]][n[1]].Weight < lowest)
				{
					lowest = Dungeon.Grid[n[0]][n[1]].Weight;
					pos = n;
                    foundWeight = true;
				}

            if (!foundWeight)
                return false;

			if (Dungeon.CanMove(this, pos[0] - Y, pos[1] - X))
			{
				Dungeon.MoveEntity(this, pos[0] - Y, pos[1] - X);
				MoveTimer.PerformAction();
			}
			return true;
		}
	}
}
