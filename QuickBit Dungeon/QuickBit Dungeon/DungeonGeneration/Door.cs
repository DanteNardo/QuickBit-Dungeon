namespace QuickBit_Dungeon.DungeonGeneration
{
	public class Door
	{
		// ======================================
		// ============== Members ===============
		// ======================================
	
		public bool Open { get; set; } = false;
		public bool Closed { get; set; } = true;

		// ======================================
		// ============== Methods ===============
		// ======================================

		#region Door Methods

		/// <summary>
		/// Opens the current door object.
		/// </summary>
		public void OpenDoor()
		{
			Open = true;
			Closed = false;
		}

		#endregion
	}
}
