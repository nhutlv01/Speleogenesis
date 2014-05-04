using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	//Health
	public float currentHealth;
	public float maxHealth;

	//Damage
	public float attackDamage;

	//Armor
	public float armor;


	/*
	 * Enemy::Enemy()
	 * Constructor
	 */
	public Enemy()
	{
		Debug.Log ("Enemy Constructor Called");
	}


	public virtual void Attack()
	{

	}

	public virtual void Dead()
	{

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
