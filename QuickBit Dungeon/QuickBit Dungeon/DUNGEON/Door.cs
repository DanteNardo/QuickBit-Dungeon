using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickBit_Dungeon.DUNGEON
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

		/// <summary>
		/// Opens the current door object.
		/// </summary>
		public void OpenDoor()
		{
			Open = true;
			Closed = false;
		}
	}
}
