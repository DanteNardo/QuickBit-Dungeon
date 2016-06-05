using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickBit_Dungeon
{
	public class Player : Entity
	{
		// ======================================
		// ============= Variables ==============
		// ======================================
		
		private int x;				// The x coordinate of the player's position
		private int y;				// The y coordinate of the player's position
		private int xpNeeded;		// The amount of xp necessary to level up
		private int manaCost;		// The amount of mana the player loses each use
		
		// Properties
		public int X		{ get { return x; }		   set { x		  = value; } }
		public int Y		{ get { return y; }		   set { y		  = value; } }
		public int ManaCost { get { return manaCost; } set { manaCost = value; } }

		// ======================================
		// ============== Methods ===============
		// ======================================

		// Constructor
		public Player() : base()
		{
			ConstructPlayer();
			xpNeeded = 100;
			manaCost = 50;
		}

		/*
			Regenerates the player's mana based
			off of their wisdom.
		*/
		public void RegenerateMana()
		{
			HealthMana += Wisdom/10;
			AttackMana += Wisdom/10;
		}

		/*
			Regenerates the player's health based
			off of their mana.
		*/
		public void RegenerateHealth()
		{
			if (HealthMana > ManaCost)
			{
				Health += Wisdom;
				HealthMana -= ManaCost;
			}
		}
	}
}
