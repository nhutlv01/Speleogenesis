using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	public List<Tile> Neighbors = new List<Tile>();
	public string[] TileTypes = {"Armor", "Weapon", "Enemy", "Potion"};
	public string type = "";
	public GameObject paperTilePrefab;

	// Use this for initialization
	void Start () {
		CreateTile ();
		GameObject s = Instantiate(paperTilePrefab, gameObject.transform.position, new Quaternion(0,0,Random.Range(0,3)*90,0)) as GameObject;
		s.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Add neighbors to list
	public void AddNeighbor(Tile t)
	{
		Neighbors.Add (t);

	}

	//Remove neighbors to list
	public void RemoveNeighbor(Tile t)
	{
		Neighbors.Remove (t);
	}

	void OnMouseDown()
	{
		Debug.Log ("Clicked", gameObject);
	}

	void CreateTile()
	{
		type = TileTypes [Random.Range (0, TileTypes.Length)];
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		string tilePath = "TileTypes/" + type;
		sr.sprite = Resources.Load<Sprite>(tilePath);


	}

} 
