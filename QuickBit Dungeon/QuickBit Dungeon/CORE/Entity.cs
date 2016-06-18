using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickBit_Dungeon
{
	/*
		Every object that attacks or moves
		inherits from the entity class.
	*/
	public class Entity
	{
		// ======================================
		// ============= Variables ==============
		// ======================================

		private int eMaxHealth;		// The maximum amount of health
		private int eHealth;		// The current amount of health
		private int eHealthRep;		// The percentage of health to show
		private int eArmor;			// The current amount of armor
		private int eStrength;		// The entitie's strength level: used for health and damage
		private int eDexterity;		// The entitie's dexterity level: used for speed and dodge
		private int eWisdom;		// The entitie's wisdom level: used for regen/magic
		private float eMaxMana;		// The entitie's maximum amount of mana
		private float eHealthMana;	// The entitie's current health mana
		private float eAttackMana;	// The entitie's current attack mana
		private int eXP;			// The xp amount that the player will receive if they defeat the entity
		private Random rnd;

		// Timing variables
		private bool canMove      = true;
		private int maxMoveTime   = 20;
		private int moveTime      = 20;
		private bool canAttack    = true;
		private int maxAttackTime = 10;
		private int attackTime    = 10;

		// Properties
		public int Health		{ get { return eHealth; }		set { eHealth     = value;     if (eHealth<0) eHealth = 0; } }
		public int HealthRep	{ get { return eHealthRep; } }
		public int Armor		{ get { return eArmor; }		set { eArmor	  = value;	   if (eArmor<0) eArmor   = 0; } }
		public int Strength		{ get { return eStrength; }		set { eStrength   = value; } }
		public int Dexterity	{ get { return eDexterity; }	set { eDexterity  = value; } }
		public int Wisdom		{ get { return eWisdom; }		set { eWisdom     = value; } }
		public bool CanMove		{ get { return canMove; }		set { canMove	  = value; } }
		public bool CanAttack	{ get { return canAttack; }		set { canAttack	  = value; } }
		public float MaxMana	{ get { return eMaxMana; }		set { eMaxMana    = value; } }
		public float HealthMana	{ get { return eHealthMana; }	set { eHealthMana = value; } }
		public float AttackMana	{ get { return eAttackMana; }	set { eAttackMana = value; } }
		public int XP			{ get { return eXP; }			set { eXP		  = value; } }

		// ======================================
		// ============== Methods ===============
		// ======================================

		// Constructor
		public Entity()
		{
			rnd = new Random();
		}

		/*
			Provides movement and attack updating
			functionality for all entities.
		*/
		public void Update()
		{
			// Handle move time
			if (!canMove) moveTime--;
			if (moveTime == 0)
			{
				moveTime = maxMoveTime;
				canMove  = true;
			}

			// Handle attack time
			if (!canAttack) attackTime--;
			if (attackTime == 0)
			{
				attackTime = maxAttackTime;
				canAttack  = true;
			}
		}

		/*
			Sets the player's default hard coded
			starting values for all stats.
		*/
		public void ConstructPlayer()
		{
			eMaxHealth  = 100;
			eHealth     = 100;
			eHealthRep  = 9;
			eArmor      = 5;
			eStrength   = 10;
			eDexterity  = 10;
			eWisdom     = 10;
			eMaxMana    = 100f;
			eHealthMana = eMaxMana;
			eAttackMana = eMaxMana;
		}

		/*
			Creates a random enemy with random
			stats (that still have bounds).
		*/
		public void ConstructMonster()
		{
			eMaxHealth    = rnd.Next(20, 100);
			eHealth       = eMaxHealth;
			eArmor        = rnd.Next(1, 9);
			eStrength     = rnd.Next(7, 14);
			eDexterity    = rnd.Next(7, 14);
			eWisdom       = rnd.Next(7, 14);
			CalculateHealthRep();
			GenerateXP();

			maxMoveTime   = 60;
			moveTime      = maxMoveTime;
			maxAttackTime = 90;
			attackTime    = maxAttackTime;
		}

		/*
			Returns the correct representation
			of the entitie's current health.
		*/
		public void CalculateHealthRep()
		{
			float ch = eHealth;
			float mh = eMaxHealth;
			float hr = (ch/mh)*10;
			if (hr == 10) hr = 9;
			eHealthRep = (int)hr;
		}

		/*
			Determines the amount of xp that
			the player should receive from 
			defeating this entity.
		*/
		public void GenerateXP()
		{
			eXP += eMaxHealth;
			eXP += eArmor;
			eXP += eStrength;
			eXP += eDexterity;
			eXP += eWisdom;
		}

		/*
			Levels up the character based on
			their desired stat change.
		*/
		public void LevelUp(string color)
		{
			switch (color)
			{
				case "red":
					eStrength  += rnd.Next(1, 4);
					eMaxHealth += eStrength;
					eHealth     = eMaxHealth;
					break;
				case "green":
					eDexterity += rnd.Next(1, 4);
					eMaxHealth += eStrength;
					eHealth     = eMaxHealth;
					break;
				case "blue":
					eWisdom    += rnd.Next(1, 4);
					eMaxHealth += eStrength;
					eHealth     = eMaxHealth;
					break;
			}
		}
	}
}
