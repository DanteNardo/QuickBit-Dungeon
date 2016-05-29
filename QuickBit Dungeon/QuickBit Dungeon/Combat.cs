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
		public static void PlayerAttack(Entity player, Entity monster)
		{
			int damage = player.Strength - monster.Armor;
			monster.Health -= damage;
			monster.CalculateHealthRep();
			player.CanAttack = false;
		}

		/*
			Executes a monster's attack on
			a player's health.
		*/
		public static void MonsterAttack(Entity monster, Entity player)
		{
			int damage = monster.Strength - player.Armor;
			player.Health -= damage;
			player.CalculateHealthRep();
			monster.CanAttack = false;
		}
	}
}
