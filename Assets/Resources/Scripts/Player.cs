using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

	public GameObject healthBar;
	public GameObject manaBar;
	public GameObject xpBar;
	public GameObject salvageBar;

	/////////////////////////////////
	//Character Types
	/////////////////////////////////
	public enum eCharTypes {Strength, Agility, Intelligence};

	/////////////////////////////////
	//Level
	/////////////////////////////////
	public float level = 1;
	public float itemLevel = 1;
	public eCharTypes charType = eCharTypes.Strength;

	/////////////////////////////////
	//Main Attributes
	/////////////////////////////////

	public float strength = 0;
	public float agility = 0;
	public float intelligence = 0;

	public float strengthMultiplier = 1;
	public float agilityMultiplier = 1;
	public float intelligenceMultiplier = 1;

	/////////////////////////////////
	//Health
	/////////////////////////////////
	public float currentHealth = 0;
	public float maxHealth = 0;
	public float healthRegen = 0;

	/////////////////////////////////
	//Mana
	/////////////////////////////////
	public float currentMana = 0;
	public float maxMana = 0;
	public float manaRegen = 0;

	/////////////////////////////////
	//Experience
	/////////////////////////////////
	public float currentXP = 0;
	public float maxXP = 0;
	public float xpMultiplier = 1;

	/////////////////////////////////
	//Salvage
	/////////////////////////////////
	public float currentSalvage = 0;
	public float maxSalvage = 0;
	public float salvageMultiplier = 1;

	/////////////////////////////////
	//Misc
	/////////////////////////////////
	public float attackPower = 0;
	public float armor = 0;

	/////////////////////////////////
	//Health Functions
	/////////////////////////////////
	void calcMaxHealth() {
		maxHealth = strength * 19;
	}

	void calcHealthRegen() {
		healthRegen = Convert.ToInt32(Math.Ceiling(strength * .4));
	}

	/////////////////////////////////
	//Mana Functions
	/////////////////////////////////
	void calcMaxMana() {
		maxMana = intelligence * 13;
	}

	void calcManaRegen() {
		manaRegen = Convert.ToInt32(Math.Ceiling(intelligence *.4));
	}

	/////////////////////////////////
	//XP Functions
	/////////////////////////////////
	void calcMaxXP()
	{
		maxXP = level * xpMultiplier * 100;
	}

	/////////////////////////////////
	//Salvage Functions
	/////////////////////////////////
	void calcMaxSalvage()
	{
		maxSalvage = itemLevel * salvageMultiplier * 100;
	}


	/////////////////////////////////
	//Misc
	/////////////////////////////////
	void calcAttackPower()
	{
		attackPower = strength;
	}

	void calcArmor()
	{
		armor = agility / 7;
	}

	void updateStats()
	{
		calcMaxHealth();
		calcHealthRegen();
		calcMaxMana();
		calcManaRegen();
		calcMaxXP ();
		calcMaxSalvage ();
		calcAttackPower();
		calcArmor();
	}

	/////////////////////////////////
	//Unity-Related
	/////////////////////////////////
	// Use this for initialization
	void Start () {
		//Set up initial values (initalization above not working correctly.
		level = 1;
		itemLevel = 1;
		currentXP = 0;
		currentSalvage = 0;
		strengthMultiplier = 1;
		agilityMultiplier = 1;
		intelligenceMultiplier = 1;
		xpMultiplier = 1;
		salvageMultiplier = 1;

		//Set up character
		if(charType == eCharTypes.Strength)
		{
			strength = 25;
			agility = 20;
			intelligence = 18;
		}
		else if(charType == eCharTypes.Agility)
		{
			strength = 20;
			agility = 25;
			intelligence = 18;
		}
		else if(charType == eCharTypes.Intelligence)
		{
			strength = 18;
			agility = 20;
			intelligence = 25;
		}

		updateStats ();

		currentHealth = maxHealth;
		currentMana = maxMana;


	}
	
	// Update is called once per frame
	void Update () {
		UpdateBars ();
	}

	void UpdateBars(){
		/////////////////////////////////
		//Bars
		/////////////////////////////////
		/// Health
		Bar b = healthBar.GetComponentInChildren<Bar> ();
		b.current = currentHealth;
		b.max = maxHealth;
		b.percent = currentHealth / maxHealth * 100;
		/// Mana
		b = manaBar.GetComponentInChildren<Bar> ();
		b.current = currentMana;
		b.max = maxMana;
		b.percent = currentMana / maxMana * 100;
		//XP
		b = xpBar.GetComponentInChildren<Bar> ();
		b.current = currentXP;
		b.max = maxXP;
		b.percent = currentXP / maxXP * 100;
		//Salvage
		b = salvageBar.GetComponentInChildren<Bar> ();
		b.current = currentSalvage;
		b.max = maxSalvage;
		b.percent = currentSalvage / maxSalvage * 100;
	}

}
