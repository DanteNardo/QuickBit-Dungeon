using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickBit_Dungeon.INTERACTION;

namespace QuickBit_Dungeon.UI.HUD
{
	public static class AwardHandler
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		public static List<Award> Awards { get; private set; }

		private static bool Killing { get; set; } = false;
		private static int KillCount { get; set; } = 0;
		private static Timer KillTimer { get; set; }

		// ======================================
		// =============== Main =================
		// ======================================

		/// <summary>
		/// Initializes the AwardHandler.
		/// </summary>
		public static void Init()
		{
			Awards = new List<Award>();
			KillTimer = new Timer(1*60);
		}

		/// <summary>
		/// Updates the AwardHandler.
		/// </summary>
		public static void Update()
		{
			KillTimer.Update();
			HandleKillAwards();
		}

		/// <summary>
		/// Determines the type of award.
		/// </summary>
		/// <param name="category">Category of award</param>
		/// <param name="count">Count relative to award type</param>
		/// <returns>The award type</returns>
		private static string DetermineAward(string category, int count)
		{
			switch (category)
			{
				case "Kill": return DetermineKillAwards(count);
				default: return "NULL";
			}
		}

		/// <summary>
		/// Determines the kill award.
		/// </summary>
		/// <param name="count">The number of kills</param>
		/// <returns>The award type</returns>
		private static string DetermineKillAwards(int count)
		{
			switch (count)
			{
				case 1: return "SingleKill";
				case 2: return "DoubleKill";
				case 3: return "TripleKill";
				default: return "Null";
			}
		}

		/// <summary>
		/// Handles the logic for kill awards.
		/// </summary>
		private static void HandleKillAwards()
		{
			if (KillTimer.ActionReady && Killing)
			{
				var award = new Award(DetermineAward("Kill", KillCount));
				Awards.Add(award);
				KillCount = 0;
				Killing = false;
			}
		}

		/// <summary>
		/// Marks a new kill.
		/// </summary>
		public static void NewKill()
		{
			Killing = true;
			KillTimer.PerformAction();
			KillCount++;
		}
	}
}
