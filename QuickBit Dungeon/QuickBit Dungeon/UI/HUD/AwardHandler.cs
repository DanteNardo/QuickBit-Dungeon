using System.Collections.Generic;
using QuickBit_Dungeon.Interaction;

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

		private static bool Stepping { get; set; } = false;
		private static int StepCount { get; set; } = 0;
		private static Timer StepTimer { get; set; }

		private static bool Regenerating { get; set; } = false;
		private static int RegenCount { get; set; } = 0;
		private static Timer RegenTimer { get; set; }

		// ======================================
		// =============== Main =================
		// ======================================

		#region Award Handler Methods

		/// <summary>
		/// Initializes the AwardHandler.
		/// </summary>
		public static void Init()
		{
			Awards = new List<Award>();
			KillTimer = new Timer(1*60);
			StepTimer = new Timer(1*60);
			RegenTimer = new Timer(1*60);
		}

		/// <summary>
		/// Updates the AwardHandler.
		/// </summary>
		public static void Update()
		{
			KillTimer.Update();
			HandleKillAwards();

			StepTimer.Update();
			RegenTimer.Update();
			HandleSpeedAwards();
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
				case "Step": return DetermineStepAwards(count);
				case "Regen": return DetermineRegenAwards(count);
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
				default: return "NULL";
			}
		}

		/// <summary>
		/// Determines the step award.
		/// </summary>
		/// <param name="count">The number of steps</param>
		/// <returns>The award type</returns>
		private static string DetermineStepAwards(int count)
		{
			if (count >= 20)
				return "SuperSprinter";
			if (count >= 10)
				return "Sprinter";
			return "NULL";
		}

		/// <summary>
		/// Determines the regen award.
		/// </summary>
		/// <param name="count">The number of regenerations</param>
		/// <returns>The award type</returns>
		private static string DetermineRegenAwards(int count)
		{
			if (count >= 20)
				return "Revitalize";
			if (count >= 10)
				return "Energize";
			return "NULL";
		}

		/// <summary>
		/// Handles the logic for kill awards.
		/// </summary>
		private static void HandleKillAwards()
		{
			if (KillTimer.ActionReady && Killing)
			{
				var type = DetermineAward("Kill", KillCount);
				if (type == "NULL") return;
				var award = new Award(type);
				Awards.Add(award);
				KillCount = 0;
				Killing = false;
			}
		}

		/// <summary>
		/// Handles the logic for speed awards.
		/// </summary>
		private static void HandleSpeedAwards()
		{
			if (StepTimer.ActionReady && Stepping)
			{
				var type = DetermineAward("Step", StepCount);
				if (type != "NULL")
				{
					var award = new Award(type);
					Awards.Add(award);
					StepCount = 0;
					Stepping = false;
				}
			}

			if (RegenTimer.ActionReady && Regenerating)
			{
				var type = DetermineAward("Regen", RegenCount);
				if (type != "NULL")
				{
					var award = new Award(type);
					Awards.Add(award);
					RegenCount = 0;
					Regenerating = false;
				}
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

		/// <summary>
		/// Marks a new step.
		/// </summary>
		public static void NewStep()
		{
			Stepping = true;
			StepCount++;
			if (StepTimer.ActionReady)
				StepTimer.PerformAction();
		}

		/// <summary>
		/// Marks a new regeneration of mana.
		/// </summary>
		public static void NewRegen()
		{
			Regenerating = true;
			RegenCount++;
			if (RegenTimer.ActionReady)
				RegenTimer.PerformAction();
		}

		#endregion
	}
}
