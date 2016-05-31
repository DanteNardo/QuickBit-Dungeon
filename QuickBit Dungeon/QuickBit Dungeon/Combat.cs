using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickBit_Dungeon
{
	/*
		Handles all combat functions 
		between two entities.
	*/
	public static class Combat
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		


		// ======================================
		// ============== Methods ===============
		// ======================================

		/*
			Executes a player's attack on 
			a monster's health.
		*/
		public static void PlayerAttack(ref Player player, ref Monster monster)
		{
			int damage = player.Strength - monster.Armor;
			monster.Health -= damage;
			monster.CalculateHealthRep();
			Dungeon.Grid[monster.Y][monster.X].Rep = GameManager.ConvertChar(monster.HealthRep);
			player.CanAttack = false;
		}

		/*
			Executes a monster's attack on
			a player's health.
		*/
		public static void MonsterAttack(ref Monster monster, ref Player player)
		{
			int damage = monster.Strength - player.Armor;
			player.Health -= damage;
			player.CalculateHealthRep();
			Dungeon.Grid[player.Y][player.X].Rep = GameManager.ConvertChar(player.HealthRep);
			monster.CanAttack = false;
		}
	}
}
