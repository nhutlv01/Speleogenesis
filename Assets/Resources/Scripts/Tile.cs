using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	public List<Tile> Neighbors = new List<Tile>();
	public string[] TileTypes = {"Armor", "EnemyWeapon", "Potion"};
	public string[] SubTypes = {"Enemy", "Weapon"};
	public string type = "";
	public string subtype ="";

	// Use this for initialization
	void Start () {
		NotificationCenter.DefaultCenter().AddObserver(this, "TimerTrigger");
		NotificationCenter.DefaultCenter().AddObserver(this, "PreEnemyAttack");
	}

	//Add neighbors to list
	public void AddNeighbor(Tile t)
	{
		Neighbors.Add (t);
	}
	//Add neighbors to list
	public void RemoveNeighbor(Tile t)
	{
		Neighbors.Remove(t);
	}

	//Remove neighbors to list
	public void RemoveNeighborAt(int i)
	{
		Neighbors.RemoveAt (i);
	}
	
	public void checkNeighbors()
	{
		for(int i = 0; i < Neighbors.Count; i++)
		{
			if(Neighbors[i] == null)
			{
				RemoveNeighborAt(i);
			}
		}
	}

	public void dead()
	{
		Vector2 force = new Vector2(Random.Range(-100, 100), Random.Range(-100,100));
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 1);
		GetComponent<Animator> ().SetTrigger("Death");
		GetComponent<CircleCollider2D> ().enabled = false;
		GetComponent<SliderJoint2D>().enabled = false;
		rb.gravityScale = 0;
		rb.fixedAngle = false;
		rb.AddForce(force);
		rb.AddTorque(Random.Range(-100,100));
		Destroy (this.gameObject, .533f);
	}

	void PreEnemyAttack()
	{
		if(subtype =="Enemy")
			GetComponent<Animator> ().SetTrigger("PreEnemyAttack");
	}

	public void TimerTrigger()
	{
		if(subtype == "Enemy")
			GetComponent<Animator> ().SetTrigger("EnemyAttack");
	}
} 
