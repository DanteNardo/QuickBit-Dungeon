using System;

namespace QuickBit_Dungeon.MANAGERS
{
	public static class GameManager
	{
		// ======================================
		// ============== Members ===============
		// ======================================
		
		public static Random Random { get; private set; }

		// ======================================
		// ============== Methods ===============
		// ======================================

		/// <summary>
		/// Initializes all GameManager data
		/// </summary>
		public static void Init()
		{
			Random = new Random();
		}

		/// <summary>
		/// Returns the correct char relative
		///	to the integer given.
		/// </summary>
		/// <param name="i">The integer to convert</param>
		/// <returns>The integer converted to character</returns>
		public static char ConvertToChar(int i)
		{
			return (char) (i + 48);
		}
	}
}
