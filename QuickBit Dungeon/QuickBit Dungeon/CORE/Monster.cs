using System;

namespace QuickBit_Dungeon.CORE
{
	public class Monster : Entity
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		public bool CanAttack { get; internal set; }
		public bool CanMove { get; internal set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Updates a Monster
		/// </summary>
		public void Update()
		{
			throw new NotImplementedException();
		}
	}
}
