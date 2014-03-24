﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	public List<Tile> Neighbors = new List<Tile>();
	public string[] TileTypes = {"Armor", "EnemyWeapon", "Potion"};
	public string[] SubTypes = {"Enemy", "Weapon"};
	public string type = "";
	public string subtype ="";
	//public GameObject paperTilePrefab;

	// Use this for initialization
	void Start () {
		CreateTile ();
		//GameObject s = Instantiate(paperTilePrefab, gameObject.transform.position, new Quaternion(0,0,Random.Range(0,3)*90,0)) as GameObject;
		//s.transform.parent = transform;
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

	void CreateTile()
	{
		string tilePath;
		type = TileTypes [Random.Range (0, TileTypes.Length)];
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		//if this is a 
		if (type == "EnemyWeapon") {
			subtype = SubTypes [Random.Range(0, SubTypes.Length)];
			tilePath = "TileTypes/" + subtype;
		}
		else
			tilePath = "TileTypes/" + type;
		sr.sprite = Resources.Load<Sprite>(tilePath);
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

} 
