using UnityEngine;
using System.Collections;

public class Prod : MonoBehaviour {

	public Tile owner;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.tag == "Tile")
			owner.AddNeighbor (c.GetComponent<Tile> ());
	}

	void OnTriggerExit2D(Collider2D c)
	{
		if (c.tag == "Tile")
			owner.RemoveNeighbor (c.GetComponent<Tile> ());
	}
}
